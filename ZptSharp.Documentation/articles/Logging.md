# Logging ZptSharp operations

ZptSharp integrates with [the Microsoft.Extensions.Logging framework]. By default it is configured to use a null/no-operation logger.

When [configuring services in dependency injection], if you add a logger implementation then you may receive ZptSharp log messages. Messages are generally recorded at `Debug` and `Trace` levels.

[the Microsoft.Extensions.Logging framework]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0
[configuring services in dependency injection]: ../api/index.md#Adding-ZptSharp-to-DI