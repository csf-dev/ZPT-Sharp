# Building and testing ZptSharp
As of the v2.x branch, the build and test process has been significantly
simplified (compared to the v1.x process).  The solution now builds using
the following command (add `-c Release` for a release-configuration build).

```
dotnet build
```

## Running unit tests
Running tests is performed via the following command.  You should use
either `Tools\Windows.runsettings` or `Tools/Linux.runsettings`
depending upon your current platform.
*Mac users should also use the runsettings file for Linux.*

```
dotnet test -s Tools/Linux.runsettings
```

The runsettings file will mean that test results are written to a `.TestResults`
directory in the root of the solution, along with output (where applicable)
from failed integration tests.  This output will show both the expected and
actual renderings from each test; you may then use file-comparison tools to
inspect differences.

## Build dependencies
In order to build the solution you will require
a [a **.NET Core SDK** for .NET Core version 3.1] or higher.
There are no other dependencies beyond those which dotnet will fetch for you.

[a **.NET Core SDK** for .NET Core version 3.1]: https://dotnet.microsoft.com/download/dotnet-core/3.1
