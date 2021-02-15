# ZptSharp View Engines

ZptSharp provides two view engines, allowing AS<span>P.N</span>ET MVC applications to display views using ZPT document templates.
The table below shows the applicable packages and MVC versions.

| NuGet Package         | MVC version                       | Intended usage            |
| -------------         | -----------                       | --------------            |
| **ZptSharp.MvcCore**  | AS<span>P.N</span>ET Core MVC 2+  | .NET Core 2.0+ or .NET 5+ |
| **ZptSharp.Mvc5**     | AS<span>P.N</span>ET MVC5         | .NET Framework 4.6.1+     |

Install the package for your MVC version/runtime (as well as at least one [document provider package], and any additional expression evaluators, if applicable) into your MVC Web Application project.

[document provider package]: NuGetPackages.md#Document-providers

## Activating the view engine

As well as installing the appropriate package (above) to your web application, you must activate the view engine in your web app setup.
Common to both framework-specific packages is the `ZptSharpViewEngine` constructor.
This makes use of a builder action/callback in which an object is configured with the optional/elective functionality.
To have a useful ZptSharp environment, _at least one document provider and one expression evaluator must be activated here_.
Full documentation for this set-up is available in the reference for [the ZptSharp DI builder object]; the examples below demonstrate a typical usage.

[the ZptSharp DI builder object]: xref:ZptSharp.Hosting.IBuildsHostingEnvironment

### AS<span>P.N</span>ET Core MVC

To activate the ZptSharp view engine for AS<span>P.N</span>ET Core MVC you must add code similar to the below to your application starup, where services are configured.
This may include a modification to an existing usage of `AddControllersWithViews()` in order to add the view options, where you add the ZptSharp view engine.

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

### AS<span>P.N</span>ET MVC5

To activate the ZptSharp view engine for AS<span>P.N</span>ET MVC5 you must add code similar to the below to either your `Global.asax.cs` within the `Application_Start` handler, or adapt it as middleware if you are using OWIN.

```csharp
var viewEngine = new ZptSharp.Mvc5.ZptSharpViewEngine(builder => {
    builder
        .AddHapZptDocuments()
        .AddStandardZptExpressions();
});
ViewEngines.Engines.Insert(0, viewEngine);
```

## Writing views

When using the ZptSharp view engine, HTML views (using either the HTML Agility Pack or AngleSharp document providers) must have filename extensions of either `.pt` (page template), `.htm` or `.html`.
When using XML views, they must have extensions of either `.xml` or `.xhtml`.

Views are kept in the same file/directory structure as usual, so by default a subdirectory of `Views` based upon the controller name, and then a file-name which matches the Action.
Controllers may return view results which specify other view-names, just as with other view-engines.

## Added TALES contexts/variables for MVC

When using the ZptSharp view engine, a few additional TALES contexts (essentially, predefined variables) are added automatically.
These contexts/variables are accessible to all views rendered by the view engine and _they are in addition to_ [the global contexts built-into Zptsharp].
The table below shows these extra contexts; the **Framework** column shows whether each context is available for AS<span>P.N</span>ET Core MVC, AS<span>P.N</span>ET MVC5 or **both** of these frameworks.

| Name          | Framework | Description                                                                                           |
| ----          | --------- | -----------                                                                                           |
| `Context`     | Both      | The current `HttpContext`                                                                             |
| `Model`       | Both      | The current model, an alias for `here`                                                                |
| `Request`     | Both      | The HTTP request, equivalent to `HttpContext.Request`                                                 |
| `request`     | Both      | The HTTP request, an alias for `Request`                                                              |
| `Response`    | Both      | The HTTP response, equivalent to `HttpContext.Response`                                               |
| `RouteData`   | Both      | The route data, equivalent to `ViewContext.RouteData`                                                 |
| `TempData`    | Both      | The temporary data, equivalent to `ViewContext.TempData`                                              |
| `Url`         | Both      | The current request URL                                                                               |
| `User`        | Both      | The current user principal, equivalent to `HttpContext.User`                                          |
| `ViewBag`     | Both      | The view bag, equivalent to `ViewContext.ViewBag`                                                     |
| `ViewContext` | Both      | The current view context                                                                              |
| `ViewData`    | Both      | The view data, equivalent to `ViewContext.ViewData`                                                   |
| `Views`       | Both      | An object which provides access to the directory containing views, typically `~/Views` in your app    |
| `Application` | MVC5 only | The application state, roughly equivalent to `HttpContext.Application`                                |
| `Cache`       | MVC5 only | The cache object, equivalent to `HttpContext.Cache`                                                   |
| `Server`      | MVC5 only | The server utility object, equivalent to `HttpContext.Server`                                         |
| `Session`     | MVC5 only | The session-state object, equivalent to `HttpContext.Session`                                         |

[the global contexts built-into Zptsharp]: ZptReference/Tales/GlobalContexts.md