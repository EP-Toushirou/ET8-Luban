#!/bin/bash

WORKSPACE=../..

LUBAN_DLL=$WORKSPACE/Tools/Luban/LubanRelease/Luban.dll
CONF_ROOT=$WORKSPACE/Unity/Assets/Config/Excel

# StartConfig Release
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -c cs-bin \
    -d bin \
    -d json \
    --conf $CONF_ROOT/StartConfig/Release/__luban__.conf \
    -x outputCodeDir=$WORKSPACE/Unity/Assets/Scripts/Model/Generate/Server/Config/StartConfig \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s/StartConfig/Release \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s/StartConfig/Release 

echo ==================== StartConfigGenReleaseFinish ====================

# StartConfig Benchmark
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -d bin \
    -d json \
    --conf $CONF_ROOT/StartConfig/Benchmark/__luban__.conf \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s/StartConfig/Benchmark \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s/StartConfig/Benchmark 

echo ==================== StartConfigGenBenchmarkFinish ====================

# StartConfig Localhost
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -d bin \
    -d json \
    --conf $CONF_ROOT/StartConfig/Localhost/__luban__.conf \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s/StartConfig/Localhost \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s/StartConfig/Localhost 

echo ==================== StartConfigGenLocalhostFinish ====================

# StartConfig RouterTest
dotnet $LUBAN_DLL \
    --customTemplateDir CustomTemplate \
    -t all \
    -d bin \
    -d json \
    --conf $CONF_ROOT/StartConfig/RouterTest/__luban__.conf \
    -x bin.outputDataDir=$WORKSPACE/Config/Excel/s/StartConfig/RouterTest \
    -x json.outputDataDir=$WORKSPACE/Config/Json/s/StartConfig/RouterTest 

echo ==================== StartConfigGenRouterTestFinish ====================