# If not tagged and not master exit early
# Otherwise pick a base directory
if ($Env:APPVEYOR_REPO_TAG -eq "true") {
    $BaseDir = "docs/"
}
elseif ($Env:APPVEYOR_REPO_BRANCH -eq "master") {
    $BaseDir = "docs/_vnext/"
}
else {
    Write-Host "We are building neither a tagged commit nor the master branch, so the docs site does not need publishing"
    exit 0
}

Get-ChildItem -Path $BaseDir/* -Exclude _vnext,_legacy,README.md,.placeholder | Remove-Item -Recurse
Get-ChildItem -Path $BaseDir/ -Directory -Exclude _vnext,_legacy | Remove-Item

Copy-Item -Path "ZptSharp.Documentation/_site/*" -Destination $BaseDir -Recurse

if ($Env:APPVEYOR_REPO_TAG -eq "true") {
    Copy-Item -Path Tools/googled08187801d097dd8.html $BaseDir
}


