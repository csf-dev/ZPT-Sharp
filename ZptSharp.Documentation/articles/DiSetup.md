# Dependency injection setup

The recommended way to consume ZptSharp from your own application is to integrate it with your dependency injection configuration.
There are two steps to this:

1. Register ZptSharp services with an `IServiceCollection`
2. Set-up the appropriate services with the `IServiceProvider`

Both of these tasks is performed using *extension methods* in the `ZptSharp` namespace.

## Registering services

These are the extension methods for `IServiceCollection`, used to add the ZptSharp services to the service collection.
Each registers a specific set of services with that service collection.
Of these, the only one which is fully mandatory is `AddZptSharp`.
For a usable ZptSharp environment, developers *must also add one of the three document providers* as well.
Some expression evaluators are included within the ZptSharp core, so additional ones need be added only if they are required.

| Method                        | NuGet Package                 | Summary                                       |
| ------                        | -------------                 | -------                                       |
| [`AddZptSharp`]               | ZptSharp.Impl                 | Adds the ZptSharp core logic                  |
| [`AddAngleSharpZptDocuments`] | ZptSharp.AngleSharp           | Adds the AngleSharp document provider         |
| [`AddHapZptDocuments`]        | ZptSharp.HtmlAgilityPack      | Adds the HTML Agility Pack document provider  |
| [`AddXmlZptDocuments`]        | ZptSharp.Xml                  | Adds the XML document provider                |
| [`AddZptCSharpExpressions`]   | ZptSharp.CSharpExpressions    | Adds the C# expression evaluator              |
| [`AddZptPythonExpressions`]   | ZptSharp.PythonExpressions    | Adds the Python expression evaluator          |

[`AddZptSharp`]: xref:ZptSharp.ServiceCollectionExtensions.AddZptSharp(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`AddAngleSharpZptDocuments`]: xref:ZptSharp.AngleSharpServiceCollectionExtensions.AddAngleSharpZptDocuments(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`AddHapZptDocuments`]: xref:ZptSharp.HapServiceCollectionExtensions.AddHapZptDocuments(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`AddXmlZptDocuments`]: xref:ZptSharp.XmlServiceCollectionExtensions.AddXmlZptDocuments(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`AddZptCSharpExpressions`]: xref:ZptSharp.CSharpServiceCollectionExtensions.AddZptCSharpExpressions(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`AddZptPythonExpressions`]: xref:ZptSharp.PythonServiceCollectionExtensions.AddZptPythonExpressions(Microsoft.Extensions.DependencyInjection.IServiceCollection)
