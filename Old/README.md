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
Stable version `v1.0.0` has been released!  Thankyou to anybody who helped test the various release candidates.

Whilst a stable is now available, future enhancements and development are planned.
Please report any suggestions, feature requests or bugs you find to [Github's issue tracker].

[Github's issue tracker]: https://github.com/csf-dev/ZPT-Sharp/issues

### Continuous integration builds
CI builds are configured via both Travis (for build & test on Linux/Mono) and AppVeyor (Windows/.NET).
Below are links to the most recent build statuses for these two CI platforms.

Platform | Status
-------- | ------
Linux/Mono (Travis) | [![Travis Status](https://travis-ci.org/csf-dev/ZPT-Sharp.svg?branch=master)](https://travis-ci.org/csf-dev/ZPT-Sharp)
Windows/.NET (AppVeyor) | [![AppVeyor status](https://ci.appveyor.com/api/projects/status/apc1gw18xjkr2fn3?svg=true)](https://ci.appveyor.com/project/craigfowler/zpt-sharp)

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
