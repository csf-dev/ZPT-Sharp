# Building and testing
The build and test process for ZptSharp 2.0 has been simplified compared to 1.x.

## Dependencies
The only dependency to build ZptSharp from source is [a **.NET Core SDK** for .NET Core version 3.1] or higher. All further build dependencies will be fetched by dotnet.

[a **.NET Core SDK** for .NET Core version 3.1]: https://dotnet.microsoft.com/download/dotnet-core/3.1

## Build
The solution builds using `dotnet build`. Optionally, add `-c Release` for a release-configuration build.

## Running tests
### Extra dependency
In order to run tests, you will additionally need either [.NET Framework 4.7.2] or an equivalent [Mono Framework] version installed.
This is because a subset of the tests cannot run in a .NET Core environment (see below).

[.NET Framework 4.7.2]: https://dotnet.microsoft.com/download/dotnet-framework/net472
[Mono Framework]: https://www.mono-project.com/

### In short, use `dotnet test`
The tests are run using `dotnet test`; this alone is sufficient to get pass/fail information.
You only need consider [more advanced options] if you would like detailed logs and/or diagnostic information.

[more advanced options]: FurtherTestingInfo.md

## Integration tests
The integration tests are an excellent source of information for how ZPT works. They are included in any normal test run. The integration test classes set up a model and application state, then use ZptSharp to render that using a template. This is then compared with an expected output for the test. The test passes is the actual & expected renderings match.

The source for the integration tests is found at:

* [The integration test classes]
* [The test files (source and expected)]

[The integration test classes]: https://github.com/csf-dev/ZPT-Sharp/tree/master/ZptSharp.Tests/IntegrationTests
[The test files (source and expected)]: https://github.com/csf-dev/ZPT-Sharp/tree/master/ZptSharp.Tests/TestFiles
