# ZptSharp NuGet packages

ZptSharp includes a number of packages; those which comprise the **core** of the library and others which you should select based upon your intended usage.

## ZptSharp core

The ZptSharp core is made of two packages:

* **[ZptSharp.Abstractions]**, which has very few dependencies and contains only abstractions and interfaces for the ZptSharp functionality.
* **[ZptSharp]** contains the majority of the ZptSharp logic.

A consuming application/solution must reference the **ZptSharp** package from the project/assembly [where it sets up dependency injection].
The project which sets up dependency injection must also reference any document provider(s) & extra expression evaluator packages which are desired.

Using dependency injection in a multi-project solution, projects which make use of ZptSharp's functionality need only reference **ZptSharp.Abstractions**.
This holds true regardless of which document providers or expression evaluators are used.
Under DI, ZptSharp is injected & consumed using only interfaces and abstractions.

A working ZptSharp environment *must include at least one document provider* package.

[ZptSharp.Abstractions]: https://www.nuget.org/packages/ZptSharp.Abstractions
[ZptSharp]: https://www.nuget.org/packages/ZptSharp
[where it sets up dependency injection]: ../API/index.md#adding-zptsharp-to-di

## Document providers

ZptSharp officially supports three document provider packages; you must use at least one in order for ZptSharp to be of any use.
Each document provider represents a different technology for the reading and writing of DOM/markup documents.

* **[ZptSharp.AngleSharp]** is a document provider for reading/writing HTML documents using the [AngleSharp] open source library.
* **[ZptSharp.HtmlAgilityPack]** is a document provider for reading/writing HTML documents using the [HTML Agility Pack] open source library.
* **[ZptSharp.Xml]** is a document provider for reading/writing XML documents using the `System.Xml.Linq` functionality.

[ZptSharp.AngleSharp]: https://www.nuget.org/packages/ZptSharp.AngleSharp
[AngleSharp]: https://anglesharp.github.io/
[ZptSharp.HtmlAgilityPack]: https://www.nuget.org/packages/ZptSharp.HtmlAgilityPack
[HTML Agility Pack]: https://html-agility-pack.net/
[ZptSharp.Xml]: https://www.nuget.org/packages/ZptSharp.Xml

## Expression evaluators

Expression evaluators extend the TALES expression-types which ZptSharp may evaluate. The core ZptSharp.Impl package contains the standard evaluators which should be suitable for most projects.

* **[ZptSharp.PythonExpressions]** allows the evaluation of expressions written in Python 2.x, via the use of [IronPython].
* **[ZptSharp.CSharpExpressions]** allows the evaluation of C# expressions, via [the Roslyn scripting API].

[ZptSharp.PythonExpressions]: https://www.nuget.org/packages/ZptSharp.PythonExpressions
[IronPython]: https://ironpython.net/
[ZptSharp.CSharpExpressions]: https://www.nuget.org/packages/ZptSharp.CSharpExpressions
[the Roslyn scripting API]: https://github.com/dotnet/roslyn

## Usage-specific packages

These packages are for particular applications of ZptSharp.

* **[ZptSharp.MvcCore]** is an
[ASP.NET Core MVC 2] View Engine. This is for using ZptSharp as the view technology for web applications written using .NET Core or .NET 5+.
* **[ZptSharp.Mvc5]** is an
[ASP.NET MVC 5] View Engine. This is for using ZptSharp as the view technology for web applications written using .NET Framework.
* **[ZptSharp.Cli]** is *a tool package* rather than a library. [It's a command-line bulk/batch rendering app].

[ZptSharp.MvcCore]: https://www.nuget.org/packages/ZptSharp.MvcCore
[ASP.NET Core MVC 2]: https://docs.microsoft.com/en-us/aspnet/core/mvc/overview
[ZptSharp.Mvc5]: https://www.nuget.org/packages/ZptSharp.Mvc5
[ASP.NET MVC 5]: https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/getting-started
[ZptSharp.Cli]: https://www.nuget.org/packages/ZptSharp.Cli
[It's a command-line bulk/batch rendering app]: CliRenderer.md
