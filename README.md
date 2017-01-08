# ZPT-Sharp
ZPT-Sharp is an implementation of [Zope Page Templates] for .NET.
Its main use case is as `ViewEngine` for **ASP.NET MVC** (presently, only MVC version 5 is supported).
It also provides offline page-rendering via the **ZptBuilder.exe** command-line utility.

[Zope Page Templates]: https://docs.zope.org/zope2/zope2book/ZPT.html

## More information
The best source for **documentation** and reference material is [the project website].
Information about **building the source code** is available in the file [BUILDING.md].
Information specific to **running the unit/integration tests** is available in the file [UNIT_TESTS.md].

[the project website]: http://csf-dev.github.io/ZPT-Sharp/
[BUILDING.md]: https://github.com/csf-dev/ZPT-Sharp/blob/master/BUILDING.md
[UNIT_TESTS.md]: https://github.com/csf-dev/ZPT-Sharp/blob/master/UNIT_TESTS.md

## Current status
The current status is that - as of `v0.8.1` - the project has reached a public **beta** release.
All of the important features are implemented and tested, and there are no future-planned changes which are not backwards-compatible.

I would not recommend using ZPT-Sharp quite yet for production use.
Whilst there is a good suite of integration tests for ZPT-Sharp, the second beta release will introduce a sample application which makes use of the MVC ViewEngine.
This sample application will approximate real world™ usage of ZPT-Sharp and may of course expose issues which need resolving before final release.
I would certainly welcome evaluation and exploration of the project though.
Please have a try of the view engine and/or the **ZptBuilder.exe** application and report any bugs to Github if you find them.

## Getting ZPT-Sharp
ZPT-Sharp is packaged and ready for download and installation.
The recommended mechanism is to install one of the available [ZPT-Sharp NuGet packages].
There are also a number of [downloadable Zip packages] linked from the documentation website.

[ZPT-Sharp NuGet packages]: http://csf-dev.github.io/ZPT-Sharp/nuget-packages.html
[downloadable Zip packages]: http://csf-dev.github.io/ZPT-Sharp/download.html

## Packaging
It is planned (in the more distant future) to release ZPT-Sharp as a package for Debian GNU/Linux.
One of the restrictions of the DFSG however, is that all dependencies must themselves be open source and packaged.
I have begun some work to gather [the packaging dependencies] and list them, so that it may be performed at a later date.

[the packaging dependencies]: https://github.com/csf-dev/ZPT-Sharp/blob/master/DEPENDENCIES.md

---

All code is released under the permissive [MIT license].
Copyright © 2017 [CSF Software Limited] et al.

[MIT license]: https://github.com/csf-dev/ZPT-Sharp/blob/master/LICENSE
[CSF Software Limited]: http://csf-dev.com/
