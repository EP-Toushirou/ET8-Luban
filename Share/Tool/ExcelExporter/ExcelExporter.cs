using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace ET
{
    public static class ExcelExporter
    {
        /// <summary>
        /// 配置代码生成父目录
        /// </summary>
        const string GeneratedCodeBaseDir = "../Unity/Assets/Scripts/Model/Generate";

        /// <summary>
        /// 客户端配置代码生成目录
        /// </summary>
        const string ClientGeneratedCodeDir = $"{GeneratedCodeBaseDir}/Client/Config";

        /// <summary>
        /// 服务端配置代码生成目录
        /// (服务端因为机器人的存在必须包含客户端所有配置，所以单独的c字段没有意义，单独的c就表示cs，所以Server目录等价于ClientServer目录)
        /// </summary>
        const string ServerGeneratedCodeDir = $"{GeneratedCodeBaseDir}/Server/Config";

        /// <summary>
        /// 双端配置代码生成目录
        /// </summary>
        const string ClientServerGeneratedCodeDir = $"{GeneratedCodeBaseDir}/ClientServer/Config";

        /// <summary>
        /// 客户端配置代码生成目录
        /// </summary>
        const string ClientGeneratedBytesDir = "../Config/Excel/c";

        /// <summary>
        /// 客户端Unity配置加载目录
        /// </summary>
        const string UnityClientBytesDir = "../Unity/Assets/Bundles/Config";

        /// <summary>
        /// 服务端配置二进制数据生成目录
        /// </summary>
        const string ServerGeneratedBytesDir = "../Config/Excel/s";

        /// <summary>
        /// 双端配置二进制数据生成目录
        /// </summary>
        const string ClientServerGeneratedBytesDir = "../Config/Excel/cs";

        /// <summary>
        /// 服务端配置Json数据生成目录
        /// </summary>
        const string ServerGeneratedJsonDir = "../Config/Json/s";

        /// <summary>
        /// 双端配置Json数据生成目录
        /// </summary>
        const string ClientServerGeneratedJsonDir = "../Config/Json/cs";

        public static void Export()
        {
            string shellFileExt = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bat" : "sh";
            Run($"GenFuncConfig.{shellFileExt}", "../Tools/Luban/");
            Run($"GenStartConfig.{shellFileExt}", "../Tools/Luban/");

            // 覆盖新文件
            CopyClientBytesToUnity();
            CopyServerCodesToClientServerCodesInUnity();
            CopyServerDataToClientServerData();

            // 清除无用Meta
            RemoveUnusedMetaFiles(UnityClientBytesDir);
            RemoveUnusedMetaFiles(ClientGeneratedCodeDir);
            RemoveUnusedMetaFiles(ServerGeneratedCodeDir);
            RemoveUnusedMetaFiles(ClientServerGeneratedCodeDir);
        }

        /// <summary>
        /// 将客户端生成文件复制到Unity目录
        /// </summary>
        static void CopyClientBytesToUnity()
        {
            RemoveAllFilesExceptMeta(UnityClientBytesDir);
            FileHelper.CopyDirectory(ClientGeneratedBytesDir, UnityClientBytesDir);
        }

        /// <summary>
        /// 将服务端生成代码复制到双端生成代码目录
        /// </summary>
        static void CopyServerCodesToClientServerCodesInUnity()
        {
            if (!Directory.Exists(ServerGeneratedCodeDir))
            {
                return;
            }

            RemoveAllFilesExceptMeta(ClientServerGeneratedCodeDir);
            CopyDirectoryExceptMeta(ServerGeneratedCodeDir, ClientServerGeneratedCodeDir);
        }

        /// <summary>
        /// 从服务器目录复制生成数据文件到双端目录
        /// </summary>
        static void CopyServerDataToClientServerData()
        {
            if (Directory.Exists(ClientServerGeneratedBytesDir))
                Directory.Delete(ClientServerGeneratedBytesDir, true);

            if (Directory.Exists(ClientServerGeneratedJsonDir))
                Directory.Delete(ClientServerGeneratedJsonDir, true);

            FileHelper.CopyDirectory(ServerGeneratedBytesDir, ClientServerGeneratedBytesDir);
            FileHelper.CopyDirectory(ServerGeneratedJsonDir, ClientServerGeneratedJsonDir);
        }

        /// <summary>
        /// 复制除meta文件以外的所有文件到另一个目录
        /// </summary>
        static void CopyDirectoryExceptMeta(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new(srcDir);
            DirectoryInfo target = new(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }

                File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();
            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectoryExceptMeta(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
            }
        }

        /// <summary>
        /// 删除meta以外的所有文件
        /// </summary>
        static void RemoveAllFilesExceptMeta(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            DirectoryInfo targetDir = new(directory);
            FileInfo[] fileInfos = targetDir.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo info in fileInfos)
            {
                if (!info.Name.EndsWith(".meta"))
                {
                    File.Delete(info.FullName);
                }
            }
        }

        /// <summary>
        /// 删除多余的meta文件
        /// </summary>
        static void RemoveUnusedMetaFiles(string directory)
        {
            DirectoryInfo targetDir = new(directory);
            FileInfo[] fileInfos = targetDir.GetFiles("*.meta", SearchOption.AllDirectories);
            foreach (FileInfo info in fileInfos)
            {
                string pathWithoutMeta = info.FullName.Remove(info.FullName.LastIndexOf(".meta", StringComparison.Ordinal));
                if (!File.Exists(pathWithoutMeta) && !Directory.Exists(pathWithoutMeta))
                {
                    File.Delete(info.FullName);
                }
            }
        }

        static void Run(string cmd, string workDirectory)
        {
            Process process = new();
            try
            {
                string app = "cmd.exe";
                string arguments = "/c";
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    app = "bash";
                    arguments = "-c";
                }

                ProcessStartInfo start = new(app);

                process.StartInfo = start;
                start.Arguments = arguments + " \"" + cmd + "\"";
                start.CreateNoWindow = true;
                start.ErrorDialog = true;
                start.UseShellExecute = false;
                start.WorkingDirectory = workDirectory;

                if (start.UseShellExecute)
                {
                    start.RedirectStandardOutput = false;
                    start.RedirectStandardError = false;
                    start.RedirectStandardInput = false;
                }
                else
                {
                    start.RedirectStandardOutput = true;
                    start.RedirectStandardError = true;
                    start.RedirectStandardInput = true;
                    start.StandardOutputEncoding = System.Text.Encoding.UTF8;
                    start.StandardErrorEncoding = System.Text.Encoding.UTF8;
                }

                bool endOutput = false;
                bool endError = false;

                process.OutputDataReceived += (_, args) =>
                {
                    if (args.Data != null)
                    {
                        Log.Console(args.Data);
                    }
                    else
                    {
                        endOutput = true;
                    }
                };

                process.ErrorDataReceived += (_, args) =>
                {
                    if (args.Data != null)
                    {
                        Log.Console(args.Data);
                    }
                    else
                    {
                        endError = true;
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                while (!endOutput || !endError)
                {
                }

                process.CancelOutputRead();
                process.CancelErrorRead();
            }
            catch (Exception e)
            {
                Log.Console(e.ToString());
            }
            finally
            {
                process.Close();
            }
        }
    }
}