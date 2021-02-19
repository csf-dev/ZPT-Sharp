# ZptSharp

ZptSharp *(formerly ZPT-Sharp)* is a library for .NET which renders HTML/XML documents from model data.
It is ideally suited for use in MVC applications, as the **View** technology.

ZPT is an *attribute-based* syntax; as such it avoids the need to add 'alien markup' to your source files.
ZPT source files remain syntactically valid HTML/XML and may even be opened/previewed locally in web browsers, offline from their applications.

## Documentation

ZptSharp's extensive documentation is hosted on **[the ZptSharp website]**.
The main website content reflects the most recent stable release of ZptSharp.
Documentation for other versions is available though:

* [Draft documentation for the upcoming/next version]
* Documentation for legacy versions
  * [Version 1.x]

[the ZptSharp website]: https://csf-dev.github.io/ZPT-Sharp/
[Draft documentation for the upcoming/next version]: https://csf-dev.github.io/ZPT-Sharp/_vnext/
[Version 1.x]: https://csf-dev.github.io/ZPT-Sharp/_legacy/v1.x/

## Source control branches

The main branches for this repository are as follows:

* The **master** branch represents the latest development/next-release code
* The **production** branch represents the most recent release version

## Continuous integration status

The status badges below are linked to the **master** branch.
They provide a summary of the health of the build & test process.

| Environment   | Status |
| ------------- | ------ |
| Windows       | [![AppVeyor (Windows) build status](https://ci.appveyor.com/api/projects/status/apc1gw18xjkr2fn3/branch/master?svg=true)](https://ci.appveyor.com/project/craigfowler/zpt-sharp/branch/master) |
| Linux         | [![Travis (Linux) build status](https://api.travis-ci.org/csf-dev/ZPT-Sharp.svg?branch=master)](https://travis-ci.org/github/csf-dev/ZPT-Sharp) |
| Code quality  | [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ZptSharp&metric=alert_status)](https://sonarcloud.io/dashboard?id=ZptSharp) |
| Test coverage | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ZptSharp&metric=coverage)](https://sonarcloud.io/dashboard?id=ZptSharp) |
