# Building and testing
The build and test process for ZptSharp 2.0 has been simplified compared to 1.x.

## Dependencies
The only dependency to build & test ZptSharp from source is [a **.NET Core SDK** for .NET Core version 3.1] or higher. All further build & test dependencies will be fetched by dotnet.

[a **.NET Core SDK** for .NET Core version 3.1]: https://dotnet.microsoft.com/download/dotnet-core/3.1

## Build
The solution builds using `dotnet build`. Optionally, add `-c Release` for a release-configuration build.

## Running tests
The tests are run using `dotnet test`; this is sufficient to get pass/fail information. If you would like further diagnostic output from the tests then use whichever of the following is appropriate to your environment.

```
dotnet test -s Tools/Linux.runsettings
dotnet test -s Tools\Windows.runsettings
```

*Mac users should also use the runsettings file for Linux.*

These runsettings files configure the test run so that detailed diagnostic output is written to a `.TestResults` directory in the root of the solution:

* An XML results file
* 'Expected' and 'Actual' renderings from any failed integration tests (where applicable)

### Integration tests
The integration tests are an excellent source of information for how ZPT works. They are included in any normal test run. The integration test classes set up a model and application state, then use ZptSharp to render that using a template. This is then compared with an expected output for the test. The test passes is the actual & expected renderings match.

The source for the integration tests is found at:

* [The integration test classes]
* [The test files (source and expected)]

[The integration test classes]: https://github.com/csf-dev/ZPT-Sharp/tree/master/ZptSharp.Tests/IntegrationTests
[The test files (source and expected)]: https://github.com/csf-dev/ZPT-Sharp/tree/master/ZptSharp.Tests/TestFiles