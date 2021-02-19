# Quick-start: ZptSharp & AS<span>P.N</span>ET MVC5

_This guide is for users of .NET Framework 4.6.1 through 4.8.x, using AS<span>P.N</span>ET MVC._
For .NET Core/.NET 5+, using AS<span>P.N</span>ET Core MVC, [please follow the linked guide instead].

[please follow the linked guide instead]: ./MvcCore.md

## Create a new MVC project

From Visual Studio (or whichever IDE), create a new AS<span>P.N</span>ET MVC project from the standard template.
This should create a minimal functional AS<span>P.N</span>ET MVC5 project.
If in doubt about the MVC version, check the NuGet package reference for **Microsoft.AspNet.Mvc**; it should be version 5.x.

## Add the ZptSharp NuGet packages

Add the following NuGet packages to your project:

* ZptSharp.Mvc5
* ZptSharp.HtmlAgilityPack

_This represents a minimal set of packages in order for an AS<span>P.N</span>ET MVC application to serve HTML using ZptSharp._

## Activate the view-engine

Add the following code to your **Global.asax.cs** `Application_Start` handler.
Other existing content in that file should remain as-is.

```csharp
var viewEngine = new ZptSharp.Mvc5.ZptSharpViewEngine(builder => {
    builder
        .AddHapZptDocuments()
        .AddStandardZptExpressions();
});
ViewEngines.Engines.Insert(0, viewEngine);
```

_If you wish to use OWIN instead of Global.asax.cs_ then the above code could also be quite easily transformed into some OWIN middleware.

## Write your first view

Create a text file in the `Views/Home` directory named `Index.pt`.
Enter the following as the content for that file.

```html
<html>
<head>
<title>ZptSharp 'Hello world' example for ASP.NET MVC5</title>
</head>
<body>
<h1>Example ASP.NET MVC5 web app</h1>
<p tal:content="here/Message">The greeting message appears here.</p>
</body>
</html>
```

## Write your controller action

The blank project should give you one controller already, named `HomeController`.
Rewrite the controller's `Index` action to the following:

```csharp
public ActionResult Index()
{
    return View(new { Message = "Hello world!" });
}
```

## Start the app & see the result!

You should now be able to compile and run your application.

You should see that your home page has the heading **Example AS<span>P.N</span>ET MVC5 web app** and underneath is the text **Hello world!**.
This "Hello World!" text came from the model supplied by the controller.
The corresponding text in the view source file reading "The greeting message appears here." has been replaced.

There is [a sample of what your application should look like] available in the source files of the solution.

[a sample of what your application should look like]: https://github.com/csf-dev/ZPT-Sharp/tree/master/Examples/ZptSharp.Examples.Mvc5

## Continue learning in the tutorial

You have now completed the quick-start for AS<span>P.N</span>ET MVC5!
You have set-up a working MVC web application with views served by ZptSharp.
To continue learning about ZptSharp, please follow [the ZptSharp tutorial], which teaches the ZPT language.
Throughout the tutorial, _changes to the model_ are performed in the `HomeController`

[the ZptSharp tutorial]: ../ZptTutorial/index.md
