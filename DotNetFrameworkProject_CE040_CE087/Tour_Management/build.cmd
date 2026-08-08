@echo off
setlocal
set SCRIPT_DIR=%~dp0
if "%SCRIPT_DIR:~-1%"=="\" set SCRIPT_DIR=%SCRIPT_DIR:~0,-1%

pushd "%SCRIPT_DIR%" >nul 2>&1
if errorlevel 1 (
    echo Failed to change directory to "%SCRIPT_DIR%".
    exit /b 1
)

if exist "%SCRIPT_DIR%\Tour_Management.sln" (
    dotnet build "%SCRIPT_DIR%\Tour_Management.sln" %*
    set BUILD_EXIT=%ERRORLEVEL%
    popd >nul 2>&1
    exit /b %BUILD_EXIT%
)

if exist "%SCRIPT_DIR%\Tour_Management.csproj" (
    dotnet build "%SCRIPT_DIR%\Tour_Management.csproj" %*
    set BUILD_EXIT=%ERRORLEVEL%
    popd >nul 2>&1
    exit /b %BUILD_EXIT%
)

echo No solution or project file found next to build.cmd.
popd >nul 2>&1
exit /b 1
