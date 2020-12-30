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
    /d:sonar.cs.nunit.reportsPaths=%APPVEYOR_BUILD_FOLDER%\.TestResults\TestResults.*.xml ^
    /d:sonar.cs.opencover.reportsPaths=%APPVEYOR_BUILD_FOLDER%\.TestResults\coverage.opencover.*.xml ^
    /d:sonar.branch.name=%APPVEYOR_REPO_BRANCH%
    
REM ---
REM The tests have to be run separately, otherwise the NUnit test results file
REM and the Coverlet results files will end up overwriting one another, which is
REM rather frustrating.
REM This workaround runs them separately and moves the results files to 'safe' filenames after
REM they have been executed.
REM ---
    
dotnet test ZptSharp.Tests ^
    /p:CollectCoverage=true ^
    /p:CoverletOutputFormat=\"json,opencover\" ^
    /p:CoverletOutput=\"../.TestResults/\" ^
    --logger:\"nunit;LogFilePath=../.TestResults\TestResults.xml\"
    
REM ---
REM 'Capture' the exit code from dotnet test for later use
REM ---
set generalexitcode=%errorlevel%
    
move .TestResults\TestResults.xml .TestResults\TestResults.ZptSharp.Tests.xml
move .TestResults\coverage.opencover.xml .TestResults\coverage.opencover.ZptSharp.Tests.xml
    
dotnet test MvcViewEngines\ZptSharp.Mvc5.Tests ^
    /p:CollectCoverage=true ^
    /p:CoverletOutputFormat=\"json,opencover\" ^
    /p:CoverletOutput=\"../../.TestResults/\" ^
    --logger:\"nunit;LogFilePath=../../.TestResults\TestResults.xml\"
      
REM ---
REM 'Capture' the exit code from dotnet test for later use
REM ---
set mvcexitcode=%errorlevel%

move .TestResults\TestResults.xml .TestResults\TestResults.ZptSharp.Mvc5.Tests.xml
move .TestResults\coverage.opencover.xml .TestResults\coverage.opencover.ZptSharp.Mvc5.Tests.xml 

dotnet-sonarscanner end ^
    /d:"sonar.login=%SONARCLOUD_SECRET_KEY%"

REM ---
REM Upload all files in the test results directory to AppVeyor as artifacts
REM ---
FOR %%F IN (.TestResults\*.*) DO appveyor PushArtifact %%F

REM If both exit codes are zero then this is exit 0, otherwise it will raise an error.
set /A "exitcode=generalexitcode+mvcexitcode"
exit /B %exitcode%