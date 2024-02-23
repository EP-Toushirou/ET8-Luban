set WORKSPACE=..\..

set LUBAN_DLL=%WORKSPACE%\Tools\Luban\LubanRelease\Luban.dll
set CONF_ROOT=%WORKSPACE%\Unity\Assets\Config\Excel

::Client
dotnet %LUBAN_DLL% ^
    --customTemplateDir CustomTemplate ^
    -t Client ^
    -c cs-bin ^
    -d bin ^
    -d json ^
    --conf %CONF_ROOT%\__luban__.conf ^
    -x outputCodeDir=%WORKSPACE%\Unity\Assets\Scripts\Model\Generate\Client\Config ^
    -x bin.outputDataDir=%WORKSPACE%\Config\Excel\c ^
    -x json.outputDataDir=%WORKSPACE%\Config\Json\c ^
    -x lineEnding=CRLF ^
    

echo ==================== FuncConfig : GenClientFinish ====================

if %ERRORLEVEL% NEQ 0 (
    echo An error occurred, press any key to exit.
    pause
    exit /b
)

::Server
dotnet %LUBAN_DLL% ^
    --customTemplateDir CustomTemplate ^
    -t All ^
    -c cs-bin ^
    -d bin ^
    -d json ^
    --conf %CONF_ROOT%\__luban__.conf ^
    -x outputCodeDir=%WORKSPACE%\Unity\Assets\Scripts\Model\Generate\Server\Config ^
    -x bin.outputDataDir=%WORKSPACE%\Config\Excel\s ^
    -x json.outputDataDir=%WORKSPACE%\Config\Json\s ^
    -x lineEnding=CRLF ^
    

echo ==================== FuncConfig : GenServerFinish ====================

if %ERRORLEVEL% NEQ 0 (
    echo An error occurred, press any key to exit.
    pause
    exit /b
)

::StartConfig Release
dotnet %LUBAN_DLL% ^
    --customTemplateDir CustomTemplate ^
    -t Release ^
    -c cs-bin ^
    -d bin ^
    -d json ^
    --conf %CONF_ROOT%\StartConfig\__luban__.conf ^
    -x outputCodeDir=%WORKSPACE%\Unity\Assets\Scripts\Model\Generate\Server\Config\StartConfig ^
    -x bin.outputDataDir=%WORKSPACE%\Config\Excel\s\StartConfig\Release ^
    -x json.outputDataDir=%WORKSPACE%\Config\Json\s\StartConfig\Release ^
    -x lineEnding=CRLF ^
    

echo ==================== StartConfig : GenReleaseFinish ====================

if %ERRORLEVEL% NEQ 0 (
    echo An error occurred, press any key to exit.
    pause
    exit /b
)

::StartConfig Benchmark
dotnet %LUBAN_DLL% ^
    --customTemplateDir CustomTemplate ^
    -t Benchmark ^
    -d bin ^
    -d json ^
    --conf %CONF_ROOT%\StartConfig\__luban__.conf ^
    -x bin.outputDataDir=%WORKSPACE%\Config\Excel\s\StartConfig\Benchmark ^
    -x json.outputDataDir=%WORKSPACE%\Config\Json\s\StartConfig\Benchmark ^
    

echo ==================== StartConfig : GenBenchmarkFinish ====================

if %ERRORLEVEL% NEQ 0 (
    echo An error occurred, press any key to exit.
    pause
    exit /b
)

::StartConfig Localhost
dotnet %LUBAN_DLL% ^
    --customTemplateDir CustomTemplate ^
    -t Localhost ^
    -d bin ^
    -d json ^
    --conf %CONF_ROOT%\StartConfig\__luban__.conf ^
    -x bin.outputDataDir=%WORKSPACE%\Config\Excel\s\StartConfig\Localhost ^
    -x json.outputDataDir=%WORKSPACE%\Config\Json\s\StartConfig\Localhost ^
    

echo ==================== StartConfig : GenLocalhostFinish ====================

if %ERRORLEVEL% NEQ 0 (
    echo An error occurred, press any key to exit.
    pause
    exit /b
)

::StartConfig RouterTest
dotnet %LUBAN_DLL% ^
    --customTemplateDir CustomTemplate ^
    -t RouterTest ^
    -d bin ^
    -d json ^
    --conf %CONF_ROOT%\StartConfig\__luban__.conf ^
    -x bin.outputDataDir=%WORKSPACE%\Config\Excel\s\StartConfig\RouterTest ^
    -x json.outputDataDir=%WORKSPACE%\Config\Json\s\StartConfig\RouterTest ^
    

echo ==================== StartConfig : GenRouterTestFinish ====================

if %ERRORLEVEL% NEQ 0 (
    echo An error occurred, press any key to exit.
    pause
    exit /b
)