$BuildNumber = $Env:APPVEYOR_BUILD_NUMBER
$SonarCloudKey = $Env:SONARCLOUD_SECRET_KEY
$BuildPath = $Env:APPVEYOR_BUILD_FOLDER
$SonarCloudBuildName = ('AppVeyor_build_' + $BuildNumber)
$SonarScannerConfigFile = ($BuildPath + '\.SonarQube.Analysis.xml')
$TestResultRoot = ($BuildPath + '\.TestResults\')
$NUnitReportPaths = ($TestResultRoot + 'TestResults.xml')
$OpenCoverReportPaths = ($TestResultRoot + 'coverage.opencover.xml')

# Just here to make it clear what build environment I'm using
dotnet --version

# Set up SonarScanner
dotnet-sonarscanner begin `
    /k:"ZptSharp" `
    /v:$SonarCloudBuildName `
    /o:craigfowler-github `
    /s:$SonarScannerConfigFile `
    /d:sonar.host.url="https://sonarcloud.io" `
    /d:sonar.login=$SonarCloudKey `
    /d:sonar.cs.nunit.reportsPaths=$NUnitReportPaths `
    /d:sonar.cs.opencover.reportsPaths=$OpenCoverReportPaths

# Build & test
dotnet build
dotnet test `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat="json,opencover" `
    /p:CoverletOutput="..\.TestResults\" `
    --test-adapter-path:. `
    --logger:"nunit;LogFilePath=..\.TestResults\TestResults.xml"

$FinalExitCode = $LASTEXITCODE

# Complete the SonarScanner process
dotnet-sonarscanner end `
    /d:sonar.login=$SonarCloudKey

# Upload artifacts
Get-ChildItem .TestResults\**\* | ForEach-Object { Push-AppveyorArtifact $_.FullName -FileName $_.Name }

# Ensure we exit with the same exit code that dotnet test emitted,
# so that the build fails where appropriate
exit $FinalExitCode
