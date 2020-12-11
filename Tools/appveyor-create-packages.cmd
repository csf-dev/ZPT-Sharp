REM ---
REM Create the packages
REM ---
IF %APPVEYOR_REPO_TAG%==true (
    set packageversion=%APPVEYOR_REPO_TAG_NAME:~1%
    dotnet pack -c Release -o .Packages -p:Version=%packageversion%
) ELSE (
    set versionsuffix=ci-build.%APPVEYOR_BUILD_NUMBER%
    ECHO The version suffix is:%versionsuffix%
    dotnet pack -c Release -o .Packages --version-suffix=%versionsuffix%
)

REM ---
REM Upload those packages into AppVeyor as artifacts
REM ---
FOR %%F IN (.Packages\*.nupkg) DO appveyor PushArtifact %%F

REM ---
REM If we built from a tag then push those packages
REM ---
IF %APPVEYOR_REPO_TAG%==true (
    FOR %%F IN (.Packages\*.nupkg) DO ECHO dotnet push %%F --api-key KEY!
    REM FOR %%F IN (.Packages\*.nupkg) DO dotnet push %%F --api-key %NUGET_SECRET_API_KEY%
)
