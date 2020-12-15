**ZptSharp** *(formerly ZPT-Sharp)* is [a widely compatible .NET library], for MVC-style binding to
HTML and/or XML document templates. It is *attribute-based* and avoids the
need to add 'alien markup' which make template files hard to read/understand
and impossible for some tools to work-upon.

[a widely compatible .NET library]: Docs/Compatibility.md

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
[information about building and testing ZptSharp]: Docs/BuildAndTest.md