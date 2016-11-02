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
The current status of the project - as of `v0.6.1` - is that the software should be considered in an _alpha_ state.
Whilst there are now no issues open with the **API Change** label, I still cannot promise that the API is completely stable.
I would certainly not recommend it for production use.
At this stage I would welcome exploration, testing and (most importantly) bug-reports from interested parties.

---

All code is released under the permissive [MIT license].
Copyright © 2016 [CSF Software Limited] et al.

[MIT license]: https://github.com/csf-dev/ZPT-Sharp/blob/master/LICENSE
[CSF Software Limited]: http://csf-dev.com/
