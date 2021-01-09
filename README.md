# ZptSharp

**ZptSharp** *(formerly ZPT-Sharp)* is a library for .NET which renders HTML/XML documents from model data.
It is ideally suited for use in MVC applications, as the **View** technology.

ZPT is an *attribute-based* syntax; as such it avoids the need to add 'alien markup' to your source files.
ZPT source files remain syntactically valid HTML/XML and may even be opened/previewed locally in web browsers, offline from their applications.

## Documentation

[Documentation for the current production release] of ZptSharp is all available on the ZptSharp website.
[Documentation for the upcoming/next version] is also available, as a draft.

[Documentation for the current production release]: https://csf-dev.github.io/ZPT-Sharp/
[Documentation for the upcoming/next version]: https://csf-dev.github.io/ZPT-Sharp/_vnext/

## Source control branches

The main branches for this repository as as follows:

* The **master** branch represents the latest development/next-release code
* The **production** branch represents the most recent release version

The master branch currently represents work toward [the v2.0.0 milestone].

## Continuous integration status

The status badges below are linked to the **master** branch.
They provide a summary of the health of the build & test process.

| Environment   | Status |
| ------------- | ------ |
| Windows       | [![AppVeyor (Windows) build status](https://ci.appveyor.com/api/projects/status/apc1gw18xjkr2fn3/branch/master?svg=true)](https://ci.appveyor.com/project/craigfowler/zpt-sharp/branch/master) |
| Linux         | [![Travis (Linux) build status](https://api.travis-ci.org/csf-dev/ZPT-Sharp.svg?branch=master)](https://travis-ci.org/github/csf-dev/ZPT-Sharp) |
| Code quality  | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ZptSharp&metric=alert_status)](https://sonarcloud.io/dashboard?id=ZptSharp) |
| Test coverage | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ZptSharp&metric=coverage)](https://sonarcloud.io/dashboard?id=ZptSharp) |

[the v2.0.0 milestone]: https://github.com/csf-dev/ZPT-Sharp/milestone/17
[the v1.1.0 release]: https://github.com/csf-dev/ZPT-Sharp/releases/tag/v1.1.0