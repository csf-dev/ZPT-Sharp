# Further information about testing ZptSharp

In order to just see the pass/fail results of all unit & integration tests, then all you need do is use `dotnet test`, as described in the [build & test] documentation.  This page goes into further detail about the tests.

[build & test]: BuildAndTest.md

## Two test projects

There are two test projects in ZptSharp:

| Project                               | Description                                         |
| -------                               | -----------                                         |
| `ZptSharp.Tests`                      | The vast majority of tests                          |
| `MvcViewEngines/ZptSharp.Mvc5.Tests`  | Tests for the A<span>SP.N</span>ET MVC5 view engine |

The tests are separated into two projects because they include tests *which must run on different target frameworks*.
Most of the tests for ZptSharp (in the **ZptSharp.Tests** project) run using .NET Core.
The tests for the MVC5 view engine (the **ZptSharp.Mvc5.Tests** project) must run using .NET Framework.

## Getting test logs & diagnostic info

In order to get meaningful test log output, *the two test projects must be run separately*.
If they are run together then one project's log with overwrite the other.
This appears to be a limitation when using `dotnet test` combined with the NUnit test adapter & the NUnit test logger.

### Command to run tests

To run tests and get execution logs & diagnostic information, use the following command:

```text
dotnet test [Project] --settings [Settings file]
```

* The `[Project]` placeholder is filled by the path to a test project, as shown in the table above.
* The `[Settings file]` placeholder is filled by one of two settings files:
  * Windows users should use `Tools\Windows.runsettings`
  * Linux/Mac users should use `Tools/Linux.runsettings`

For example, to run the MVC5 view engine tests on a Windows environment:

```text
dotnet test MvcViewEngines/ZptSharp.Mvc5.Tests --settings Tools\Windows.runsettings
```

### The logs & diagnostic output

These runsettings files configure the test run so that detailed diagnostic output is written to a `.TestResults` directory in the root of the solution.  This includes an **XML results file** from the test run and also detailed results of any failed integration tests.

The integration test results are written only for tests which failed; nothing will be written for passing tests.
For every integration test which fails, two files will be written: an **expected** rendering and an **actual** rendering.
These files may be compared in order to analyse any discrepancies.

## Code coverage reports

In addition, [Coverlet] is installed into both test projects and is capable of producing test-coverage reports when running tests.
The `dotnet test` command may be modified such that test-coverage data is also written to the `.TestResults` directory.
The best example showing how to accomplish this is found within [the AppVeyor continuous integration build script].  Test coverage is a CI metric, monitored by [SonarCloud].

[Coverlet]: https://github.com/coverlet-coverage/coverlet
[the AppVeyor continuous integration build script]: https://github.com/csf-dev/ZPT-Sharp/blob/master/Tools/appveyor-build.cmd
[SonarCloud]: https://sonarcloud.io/dashboard?id=ZptSharp
