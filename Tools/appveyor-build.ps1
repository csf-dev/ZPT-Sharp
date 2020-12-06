$BuildNumber = $Env:APPVEYOR_BUILD_NUMBER
$SonarCloudKey = $Env:SONARCLOUD_SECRET_KEY
$BuildPath = $Env:APPVEYOR_BUILD_FOLDER
$SonarCloudBuildName = ('AppVeyor_build_' + $BuildNumber)
$SonarScannerConfigFile = ($BuildPath + '\.SonarQube.Analysis.xml')
$TestResultRoot = ($BuildPath + '\.TestResults\')
$NUnitReportPaths = ($TestResultRoot + 'TestResults.xml')
$OpenCoverReportPaths = ($TestResultRoot + 'coverage.opencover.xml')

Write-Host "Using following dotnet version"
dotnet --version

Write-Host "Setting up SonarScanner"
dotnet-sonarscanner begin `
    /k:"ZptSharp" `
    /v:"$SonarCloudBuildName" `
    /o:craigfowler-github `
    /s:"$SonarScannerConfigFile" `
    /d:sonar.host.url="https://sonarcloud.io" `
    /d:sonar.login=$SonarCloudKey `
    /d:sonar.cs.nunit.reportsPaths="$NUnitReportPaths" `
    /d:sonar.cs.opencover.reportsPaths="$OpenCoverReportPaths"

Write-Host "Building and testing"
# Note that "%2c" is the escape sequence for a comma and "%3b" is the sequence for semicolon
dotnet build
dotnet test `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat="json%2copencover" `
    /p:CoverletOutput="..\.TestResults\" `
    --test-adapter-path:. `
    --logger:"nunit%3bLogFilePath=..\.TestResults\TestResults.xml"

$FinalExitCode = $LASTEXITCODE

Write-Host "Completing the SonarScanner process"
dotnet-sonarscanner end `
    /d:sonar.login=$SonarCloudKey

Write-Host "Uploading artifacts"
Get-ChildItem .TestResults\**\*.* | ForEach-Object { Push-AppveyorArtifact $_.FullName -FileName $_.Name }

Write-Host "Exiting script using exit code from dotnet test"
exit $FinalExitCode
