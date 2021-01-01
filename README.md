**ZptSharp** *(formerly ZPT-Sharp)* is [a library for .NET] which renders HTML and/or XML documents from model data.
It is ideally suited for use in MVC applications, as the **View** technology.

ZPT is an *attribute-based* syntax; as such it avoids the need to add 'alien markup' to your source files.
ZPT source files remain syntactically valid HTML/XML and may even be opened/previewed locally in web browsers, offline from their applications.

[a library for .NET]: ZptSharp.Documentation/articles/Compatibility.md

## Continuous integration status
In this repository the **master** branch represents the latest development code
(some repositories would call this branch *develop*).  The **production** branch
is the latest released version (some repositories would call this branch *master*).

The master branch now represents the work towards [the v2.0.0 milestone].  For the
latest v1.x code, please see [the v1.1.0 release].  The status badges below show the
current CI status of the master branch.

| Environment   | Status |
| ------------- | ------ |
| Windows       | [![AppVeyor (Windows) build status](https://ci.appveyor.com/api/projects/status/apc1gw18xjkr2fn3/branch/master?svg=true)](https://ci.appveyor.com/project/craigfowler/zpt-sharp/branch/master) |
| Linux         | [![Travis (Linux) build status](https://api.travis-ci.org/csf-dev/ZPT-Sharp.svg?branch=master)](https://travis-ci.org/github/csf-dev/ZPT-Sharp) |
| Code quality  | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ZptSharp&metric=alert_status)](https://sonarcloud.io/dashboard?id=ZptSharp) |
| Test coverage | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ZptSharp&metric=coverage)](https://sonarcloud.io/dashboard?id=ZptSharp) |

See also: [information about building and testing ZptSharp].

[the v2.0.0 milestone]: https://github.com/csf-dev/ZPT-Sharp/milestone/17
[the v1.1.0 release]: https://github.com/csf-dev/ZPT-Sharp/releases/tag/v1.1.0
[information about building and testing ZptSharp]: ZptSharp.Documentation/articles/BuildAndTest.md