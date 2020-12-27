ZptSharp is a .NET Framework/.NET Standard library for writing HTML or XML from templates, using [the ZPT syntax]. It may be used as any of:

* An ASP.NET MVC 5 view engine
* An ASP.NET Core MVC view engine
* A standalone command-line app
* A library for your own apps

[the ZPT syntax]: articles/ZptSyntax

## Syntax example
Here is a brief sample of ZPT syntax, as would be used in a view.

```html
<ul>
  <li tal:repeat="todo here/Todos">
    Remember to
    <span tal:replace="todo/Description">feed the dog</span>
  </li>
</ul>
```

## Learn more
* [Quick-start: ASP.NET MVC 5]
* [Quick-start: ASP.NET Core MVC]
* [Quick-start: Command-line app]
* [Quick-start: Consuming the API]
* [Tutorial: Learning ZPT]
* [ZPT syntax reference]
* [API reference]

[Quick-start: ASP.NET MVC 5]: articles/QuickStart/Mvc5
[Quick-start: ASP.NET Core MVC]: articles/QuickStart/MvcCore
[Quick-start: Command-line app]: articles/QuickStart/CliApp
[Quick-start: Consuming the API]: articles/QuickStart/ConsumingTheApi
[Tutorial: Learning ZPT]: articles/ZptTutorial
[ZPT syntax reference]: articles/ZptReference
[API reference]: articles/ApiReference