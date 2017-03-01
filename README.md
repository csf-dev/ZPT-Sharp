# ZPT-Sharp
ZPT-Sharp is an implementation of [Zope Page Templates] for .NET.
It is primarily intended for use as as a `ViewEngine` for **ASP.NET MVC** v5.
It also provides offline page-rendering via the **ZptBuilder.exe** command-line utility, as well
as exposing **a page-rendering API** for your own applications.

[Zope Page Templates]: https://docs.zope.org/zope2/zope2book/ZPT.html

## More information
The best source for **documentation** and reference material is [the project website].
Information about **building the source code** is available in the file [BUILDING.md].
Information specific to **running the unit/integration tests** is available in the file [UNIT_TESTS.md].

[the project website]: http://csf-dev.github.io/ZPT-Sharp/
[BUILDING.md]: https://github.com/csf-dev/ZPT-Sharp/blob/master/BUILDING.md
[UNIT_TESTS.md]: https://github.com/csf-dev/ZPT-Sharp/blob/master/UNIT_TESTS.md

## Current status
**[Current CI build status](https://github.com/csf-dev/ZPT-Sharp/blob/master/.travis.yml)** : [![Build Status](https://travis-ci.org/csf-dev/ZPT-Sharp.svg?branch=master)](https://travis-ci.org/csf-dev/ZPT-Sharp)

The current status is that - as of `v0.9.3` - the project has reached **release candidate 4**.
As of the `v0.9.3` release date, there are no planned code changes to the project 
There is good coverage of the project via integration tests and it has been used for some time now to build its own documentation website.

Evaluation & exploration of the project are welcome, along with feedback and suggestions for improvement.
Please report any bugs to [Github's issue tracker] if you find them.

[Github's issue tracker]: https://github.com/csf-dev/ZPT-Sharp/issues

## Getting ZPT-Sharp
ZPT-Sharp is packaged and ready for download and installation.
The recommended mechanism is to install one of the available [ZPT-Sharp NuGet bundles].
There is also a [downloadable ZIP package] available.

[ZPT-Sharp NuGet bundles]: http://csf-dev.github.io/ZPT-Sharp/nuget-packages.html
[downloadable ZIP package]: http://csf-dev.github.io/ZPT-Sharp/download.html

---

All code is released under the permissive [MIT license].
Copyright © 2017 [CSF Software Limited] et al.

[MIT license]: https://github.com/csf-dev/ZPT-Sharp/blob/master/LICENSE
[CSF Software Limited]: http://csf-dev.com/
