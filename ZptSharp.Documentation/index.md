<div class="homepage">

# Intuitive HTML & XML document templates for .NET

ZptSharp is a document template language & renderer for .NET.
It may be used as a **View Engine** for AS<span>P.N</span>ET MVC5 or AS<span>P.N</span>ET Core MVC.
It also comes with a command-line rendering tool and may be consumed by your own applications as an API.

<section class="sample">

## Sample ZPT markup

```html
<ul>
  <li tal:repeat="todo here/Todos">
    Remember to
    <span tal:replace="todo/Description">feed the dog</span>
  </li>
</ul>
```

</section>

<section class="get_started">

## Get started

Here are some recommended first steps to learn about ZptSharp and the ZPT syntax.
[The documentation home page] contains more links & reference.

* [What is ZptSharp?]
* Quick start
  * [ASP.NET Core MVC]
  * [ASP.NET MVC 5]
  * [Consume the API]
  * [Command line renderer]
* [ZPT tutorial]

[What is ZptSharp?]: articles/WhatIsZptSharp.md
[ASP.NET MVC 5]: articles/QuickStart/Mvc5.md
[ASP.NET Core MVC]: articles/QuickStart/MvcCore.md
[Command line renderer]: articles/QuickStart/CliApp.md
[Consume the API]: articles/QuickStart/ConsumingTheApi.md
[ZPT tutorial]: articles/ZptTutorial/index.md
[The documentation home page]: articles/index.md

</section>

</div>
