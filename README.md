# ZPT-Sharp
ZPT-Sharp is an implementation of [Zope Page Templates] in .NET.
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
The current status of the project - as of `v0.7.0` - is that the software should be considered in an _alpha_ state.
There are presently no open issues with [the API Change label].
That said, I am currently have two issues open - one [for csharp expressions] and one [for python expressions] - which may impact the stability of the API.
These involve importing types from namespaces/modules, referencing assemblies and making Linq available.

I would certainly not recommend the project for production use yet.
At this stage I would welcome exploration, testing and (most importantly) bug-reports from interested parties.

[the API Change label]: https://github.com/csf-dev/ZPT-Sharp/labels/API%20change
[for csharp expressions]: https://github.com/csf-dev/ZPT-Sharp/issues/170
[for python expressions]: https://github.com/csf-dev/ZPT-Sharp/issues/175

## Packaging
It is planned, once ZPT-Sharp is in a **beta** state, to package it for Nuget and release it as pre-release software.
Once I have created a project of my own which makes use of ZPT-Sharp (and ironed out any kinks found), I will declare it stable and perform a full v1.0.0 release via Nuget.

It is planned (in the more distant future) to release ZPT-Sharp as a package for Debian GNU/Linux.
One of the restrictions of the DFSG however, is that all dependencies must themselves be open source and packaged.
I have begun some work to gather [the packaging dependencies] and list them, so that it may be performed at a later date.

[the packaging dependencies]: https://github.com/csf-dev/ZPT-Sharp/blob/master/DEPENDENCIES.md

---

All code is released under the permissive [MIT license].
Copyright © 2016 [CSF Software Limited] et al.

[MIT license]: https://github.com/csf-dev/ZPT-Sharp/blob/master/LICENSE
[CSF Software Limited]: http://csf-dev.com/
