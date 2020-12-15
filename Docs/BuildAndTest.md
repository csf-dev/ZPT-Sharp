# Building and testing ZptSharp
As of the v2.x branch, the build and test process has been significantly
simplified (compared to the v1.x process).  The solution now builds using simply:

```
dotnet build
```

## Running unit tests
Running tests is performed via the following command.  You may omit the `--logger` option
if you wish.  Including this option causes detailed test rules to be written to file.

```
dotnet test --logger=nunit
```

## Build dependencies
In order to build the solution you will require
a [a **.NET Core SDK** for .NET Core version 3.1] or higher.
There are no other dependencies beyond those which dotnet will fetch for you.

[a **.NET Core SDK** for .NET Core version 3.1]: https://dotnet.microsoft.com/download/dotnet-core/3.1
