REM ---
REM Determine if we should build the docs or not and if not then exit early
REM ---
Set "_shouldbuild="
IF %APPVEYOR_REPO_TAG%==true Set _shouldbuild=1
IF %APPVEYOR_REPO_BRANCH%==master Set _shouldbuild=1

IF NOT _shouldbuild EQU 1 EXIT /b 0

nuget install docfx.console -version 2.56.6 -o .docfx
Set docfxpath=".docfx\docfx.console.2.56.6\tools\docfx.exe"