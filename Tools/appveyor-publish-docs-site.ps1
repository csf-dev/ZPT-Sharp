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

# Clear the target directory except for
# readmes, placeholders and the _vnext and _legacy directories.
Get-ChildItem -Path $BaseDir/* -Exclude _vnext,_legacy,README.md,.placeholder,.nojekyll | Remove-Item -Recurse
Get-ChildItem -Path $BaseDir/ -Directory -Exclude _vnext,_legacy | Remove-Item

# Copy the built site from where it built into the base directory,
# and if we are prod, also the Google verification page
Copy-Item -Path "ZptSharp.Documentation/_site/*" -Destination $BaseDir -Recurse
if ($Env:APPVEYOR_REPO_BRANCH -eq "production") {
    Copy-Item -Path Tools/googled08187801d097dd8.html $BaseDir
}

# Set up git so that it can push
git config --global user.name "AppVeyor (on behalf of Craig Fowler)"
git config --global  user.email "craig+appveyor@csf-dev.com"
git config --global credential.helper store
Set-Content -Path "$HOME\.git-credentials" -Value "https://$($Env:GITHUB_SECRET_KEY):x-oauth-basic@github.com`n" -NoNewline

# Commit & push the amended repo, using a commit message that won't cause another build
# Because of autocrlf, the git add command could report warnings which cause the script to
# stop unless I change the error preference first
$ErrorActionPreference = "silentlycontinue"
git add --all docs/
git commit -m "Auto-publish docs website [skip ci]"
git push origin HEAD:$Env:APPVEYOR_REPO_BRANCH
