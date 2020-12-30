# MVC ViewEngines
This directory contains the following ViewEngine implementations:

| MVC package           | Version   | Minimum Dependency            | Intended usage        |
| --------------------- | --------- | ----------------------------- | --------------------- |
| [ASP.NET MVC]         | 5.0.0     | .NET Framework 4.6.1          | .NET Framework        |
| [ASP.NET Core MVC]    | 2.0.0     | .NET Standard 2.0             | .NET Core or .NET 5+  |

[ASP.NET MVC]: https://www.nuget.org/packages/Microsoft.AspNet.Mvc/5.0.0
[ASP.NET Core MVC]: https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Core/2.0.0

## How the source files are organised
The C# source files for these two projects are *shared*.
Those files are held within the [`CommonSource`](CommonSource/) directory.
They use *compiler flags*  (aka preprocessor directives) used to deal with the differences between frameworks.

These source files are *linked* to the two projects for MVC5 & MVC Core.

## Tests for MVC5
The ZptSharp.Mvc5 project has its own unit tests assembly.
This is because it can only be tested under .NET Framework and not using .NET Core.
All other tests for the solution run using .NET Core.