# ZptSharp compatibility
ZptSharp is written with compatiblity in mind. The NuGet packages use *multi-targeting* to offer a wide variety of compatible framework versions.

| Framework      | Min version |
| -------------- | ----------- |
| .NET Framework | 4.5.0       |
| .NET Core      | 2.0         |
| .NET           | 5.0         |
| .NET Standard  | 2.0         |

The only notable .NET release *with which ZptSharp is not compatible* is .NET Core 1.x.

## Limitation: AngleSharp does not support .NET Framework 4.5.x
The plugin package
which provides [AngleSharp] integration does not support .NET Framework 4.5.x; AngleSharp itself does not support that version. To use ZptSharp and the AngleSharp integration with .NET Framework you will need to use Framework version 4.6.0 or higher. .NET Core, .NET Standard & .NET 5+ are unaffected.

If you are limited to .NET Framework 4.5.x then you must use the [HTML Agility Pack] integration instead.

[AngleSharp]: http://anglesharp.github.io/
[HTML Agility Pack]: https://html-agility-pack.net/