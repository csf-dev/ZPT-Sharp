IF %APPVEYOR_REPO_TAG%==true (
    SET package_version=%APPVEYOR_REPO_TAG_NAME:~1%
) ELSE (
    SET package_version=%APPVEYOR_BUILD_VERSION%
)

REM ---
REM Create the packages
REM ---
dotnet pack -c Release -o .Packages -p:PackageVersion=%package_version%

REM ---
REM Upload those packages into AppVeyor as artifacts
REM ---
FOR %%F IN (.Packages\*.nupkg) DO appveyor PushArtifact %%F

REM ---
REM If we built from a tag then push those packages
REM ---
IF "%APPVEYOR_REPO_TAG%"=="true" (
    FOR %%F IN (.Packages\*.nupkg) DO dotnet push %%F --api-key %NUGET_SECRET_API_KEY%
)
