REM ---
REM Emit the version info for dotnet, for debugging purposes
REM ---
dotnet --version

dotnet-sonarscanner begin ^
    /k:"ZptSharp" ^
    /v:AppVeyor_build_%APPVEYOR_BUILD_NUMBER% ^
    /o:craigfowler-github ^
    /s:%APPVEYOR_BUILD_FOLDER%\.SonarQube.Analysis.xml ^
    /d:sonar.host.url="https://sonarcloud.io" ^
    /d:sonar.login=%SONARCLOUD_SECRET_KEY% ^
    /d:sonar.cs.nunit.reportsPaths=%APPVEYOR_BUILD_FOLDER%\.TestResults\TestResults.xml ^
    /d:sonar.cs.opencover.reportsPaths=%APPVEYOR_BUILD_FOLDER%\.TestResults\coverage.opencover.xml ^
    /d:sonar.branch.name=%APPVEYOR_REPO_BRANCH%
    
dotnet test ^
    /p:CollectCoverage=true ^
    /p:CoverletOutputFormat=\"json,opencover\" ^
    /p:CoverletOutput=\"../.TestResults/\" ^
    --test-adapter-path:. ^
    --logger:\"nunit;LogFilePath=../.TestResults\TestResults.xml\"

REM ---
REM 'Capture' the exit code from dotnet test for later use
REM ---
set exitcode=%errorlevel%

dotnet-sonarscanner end ^
    /d:"sonar.login=%SONARCLOUD_SECRET_KEY%"

REM ---
REM Upload all files in the test results directory to AppVeyor as artifacts
REM ---
FOR %%F IN (.TestResults\*.*) DO appveyor PushArtifact %%F

exit /B %exitcode%