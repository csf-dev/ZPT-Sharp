ZptSharp is a .NET Framework/.NET Standard library for writing HTML or XML from templates, using [the ZPT syntax]. It may be used as any of:

* An AS<span>P.N</span>ET MVC 5 view engine
* An AS<span>P.N</span>ET Core MVC view engine
* A standalone command-line app
* A library for your own apps

[the ZPT syntax]: articles/ZptSyntax.md

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

[Quick-start: ASP.NET MVC 5]: articles/QuickStart/Mvc5.md
[Quick-start: ASP.NET Core MVC]: articles/QuickStart/MvcCore.md
[Quick-start: Command-line app]: articles/QuickStart/CliApp.md
[Quick-start: Consuming the API]: articles/QuickStart/ConsumingTheApi.md
[Tutorial: Learning ZPT]: articles/ZptTutorial.md
[ZPT syntax reference]: articles/ZptReference.md
[API reference]: articles/ApiReference.md