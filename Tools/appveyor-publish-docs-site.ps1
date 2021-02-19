$ErrorActionPreference = "Stop"

# Choose a base directory for the site to be published from,
# if this branch is production then it's the main "docs/"" dir,
# if master then it's "docs/_vnext/".
# If neither then we're not publishing the docs site at all.
if ($Env:APPVEYOR_REPO_BRANCH -eq "production") {
    $BaseDir = "docs/"
}
elseif ($Env:APPVEYOR_REPO_BRANCH -eq "master") {
    $BaseDir = "docs/_vnext/"
}
else {
    Write-Host "We are building from neither the master or production branches; the docs site does not need publishing"
    exit 0
}

Write-Host "Publishing the docs site to $BaseDir"

Write-Host "Clearing $BaseDir ..."
Get-ChildItem -Path $BaseDir/* -Exclude _vnext,_legacy,README.md,.placeholder,.nojekyll | Remove-Item -Recurse
if ($Env:APPVEYOR_REPO_BRANCH -eq "production") {
    Get-ChildItem -Path $BaseDir/ -Directory -Exclude _vnext,_legacy | Remove-Item
}
else {
    Get-ChildItem -Path $BaseDir/ -Directory | Remove-Item
}

Write-Host "Copying built docs site to $BaseDir ..."
Copy-Item -Path "ZptSharp.Documentation/_site/*" -Destination $BaseDir -Recurse
if ($Env:APPVEYOR_REPO_BRANCH -eq "production") {
    Write-Host "Copying Google site auth file to $BaseDir ..."
    Copy-Item -Path Tools/googled08187801d097dd8.html $BaseDir
}

if($Env:APPVEYOR -eq "True") {
    Write-Host "Setting up git to publish site"
    git config --global user.name "AppVeyor (on behalf of Craig Fowler)"
    git config --global user.email "craig+appveyor@csf-dev.com"
    git config --global credential.helper store
    Set-Content -Path "$HOME\.git-credentials" -Value "https://$($Env:GITHUB_SECRET_KEY):x-oauth-basic@github.com`n" -NoNewline
}

# The git commands below could report warnings which cause the script to
# stop unless I change the error preference first
$ErrorActionPreference = "silentlycontinue"

Write-Host "Switching to a temp branch"
git checkout -b temp/publish-docs

Write-Host "Adding content"
git add --all docs/

Write-Host "Creating commit"
git commit -m "Auto-publish docs website [skip ci]"

if($Env:APPVEYOR -eq "True" -and $Env:APPVEYOR_REPO_BRANCH -eq "production") {
    Write-Host "Pushing to origin"
    git checkout master
    git pull
    git merge temp/publish-docs --no-ff -m "Merge newly-published docs [skip ci]"
    git push origin master
}
elseif ($Env:APPVEYOR -eq "True") {
    Write-Host "Pushing to origin"
    git push origin temp/publish-docs:master
}