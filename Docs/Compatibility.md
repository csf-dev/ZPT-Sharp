# ZptSharp compatibility with .NET versions
ZptSharp is designed with compatiblity in mind.  As such it is *multi-targeted* for maximum usability with a variety of .NET versions.  The table below shows this in more depth.  The only notable .NET release *with which ZptSharp is not compatible* is .NET Core 1.x.

| Compatible with                   | Framework version codes               |
| ------                            | -----------------                     |
| **.NET Framework 4.5.0** and up   | `net45`, `net46`, `net47`, `net48`    |
| **.NET Standard 2.0** and up      | `netstandard2.0`, `netstandard2.1`    |
| **.NET Core 2.0** and up          | *Covered by netstandard, above*       |
| **.NET 5** and up                 | *Covered by netstandard, above*       |

## Limitation: AngleSharp integration does not support `net45`
The compatibility table above relates to the core ZptSharp packages.  The plugin package which provides [AngleSharp] integration does not support .NET Framework 4.5.x, however.  This is because AngleSharp itself does not support that framework version.

If you wish to use AngleSharp as your document-rendering engine then you must use .NET Framework 4.6.0 or above, or .NET Standard 2.0 or above (including .NET Core 2.0 or above, and .NET 5 or above).  If you are limited to .NET Framework 4.5.x then you may use the [HTML Agility Pack] integration
for processing HTML documents instead.

[AngleSharp]: http://anglesharp.github.io/
[HTML Agility Pack]: https://html-agility-pack.net/