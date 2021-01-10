# Building and testing

The build and test process for ZptSharp 2.0 has been simplified compared to 1.x.
*The only dependency to build ZptSharp* from source is [a **.NET Core SDK** for .NET Core version 3.1] or higher.
All further build dependencies will be fetched by dotnet.

The solution builds using `dotnet build`.
Optionally, add `-c Release` for a release-configuration build.

[a **.NET Core SDK** for .NET Core version 3.1]: https://dotnet.microsoft.com/download/dotnet-core/3.1

## Running tests

In order to run tests, *you will additionally need* either [.NET Framework 4.7.2] or an equivalent [Mono Framework] version installed.
This is because [a subset of the tests cannot run in a .NET Core environment].
The tests are run using `dotnet test`; this alone is sufficient to get pass/fail information.
You only need consider [more advanced options] if you would like detailed logs and/or diagnostic information.

[.NET Framework 4.7.2]: https://dotnet.microsoft.com/download/dotnet-framework/net472
[Mono Framework]: https://www.mono-project.com/
[a subset of the tests cannot run in a .NET Core environment]: FurtherTestingInfo.md
[more advanced options]: FurtherTestingInfo.md

## The integration tests

[The integration tests] are an excellent source of information for how ZPT works; they are included in any normal test run.
The integration test classes work in conjunction with [some files in the tests source directory].
The test first sets up a model and application state, then renders that using a source document (from the test files).
Another test file, in a parallel directory structure, is an 'expected rendering'.
The test compares the actual result of the rendering with the expected outcome and fails if they differ.

[The integration tests]: https://github.com/csf-dev/ZPT-Sharp/tree/master/ZptSharp.Tests/IntegrationTests
[some files in the tests source directory]: https://github.com/csf-dev/ZPT-Sharp/tree/master/ZptSharp.Tests/TestFiles