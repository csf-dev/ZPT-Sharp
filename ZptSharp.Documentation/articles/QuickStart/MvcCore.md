# Quick-start: ZptSharp with AS<span>P.N</span>ET Core MVC

_This guide is for users of .NET Core 2.0 or higher (including .NET 5+), using AS<span>P.N</span>ET Core MVC._
For .NET Framework, using AS<span>P.N</span>ET MVC5, [please follow the linked guide instead].

The following tutorial favours the use of the `dotnet` command-line tool, so as to avoid reliance upon Visual Studio.
_Full IDEs will provide GUI alternatives to almost all of these steps, which you may use instead if you prefer._

[please follow the linked guide instead]: ./Mvc5.md

## Create a new MVC project

From a command-line, enter the following command.

```text
dotnet new mvc -o MvcCoreSample
```

## Add the ZptSharp NuGet packages

From a command-line, use the following two commands:

```text
dotnet add MvcCoreSample\MvcCoreSample.csproj package ZptSharp.MvcCore
dotnet add MvcCoreSample\MvcCoreSample.csproj package ZptSharp.HtmlAgilityPack
```

_This represents a minimal set of packages in order for an AS<span>P.N</span>ET MVC application to serve HTML using ZptSharp._

## Activate the view-engine

Add the following code to your **Startup.cs** `ConfigureServices` method.
This includes a small change to the usage of `AddControllersWithViews()`: adding the view options, which tell it to use the ZptSharp view engine.
Other existing content in that file should remain as-is.

```csharp
var viewEngine = new ZptSharpViewEngine(builder => {
    builder.AddStandardZptExpressions();
    builder.AddHapZptDocuments();
});

services.AddControllersWithViews()
    .AddViewOptions(opts => {
        opts.ViewEngines.Insert(0, viewEngine);
    });
```

## Write your first view

Create a text file in the `Views/Home` directory named `Index.pt`.
Enter the following as the content for that file.

```html
<html>
<head>
<title>ZptSharp 'Hello world' example for ASP.NET Core MVC</title>
</head>
<body>
<h1>Example ASP.NET Core MVC web app</h1>
<p tal:content="here/Message">The greeting message appears here.</p>
</body>
</html>
```

## Write your controller action

The blank project should give you one controller already, named `HomeController`.
Rewrite the controller's `Index` action to the following:

```csharp
public IActionResult Index()
{
    return View(new { Message = "Hello world!" });
}
```

## Start the app & see the result!

You should now be able to compile and run your application:

```text
dotnet run --project MvcCoreSample\MvcCoreSample.csproj
```

You should see that your home page has the heading **Example AS<span>P.N</span>ET Core MVC web app** and underneath is the text **Hello world!**.
This "Hello World!" text came from the model supplied by the controller.
The corresponding text in the view source file reading "The greeting message appears here." has been replaced.

There is [a sample of what your application should look like] available in the source files of the solution.

[a sample of what your application should look like]: https://github.com/csf-dev/ZPT-Sharp/tree/master/Examples/ZptSharp.Examples.MvcCore

## Continue learning in the tutorial

You have now completed the quick-start for AS<span>P.N</span>ET Core MVC!
You have set-up a working MVC web application with views served by ZptSharp.
To continue learning about ZptSharp, please follow [the ZptSharp tutorial], which teaches the ZPT language.
Throughout the tutorial, _changes to the model_ are performed in the `HomeController`

[the ZptSharp tutorial]: ../ZptTutorial/index.md
