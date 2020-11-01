# Running automated tests
There are a reasonable amount of unit tests for ZPT-Sharp.
At present, test coverage is not 100% and there are [a number of open tickets to improve that coverage].

All tests are built around [NUnit 2.6.4] and will require an installed NUnit test runner to execute.
The tests are separated into a number of projects/assemblies, each one covering a different assembly within ZPT-Sharp.
The test projects can be found in the codebase within the subdirectory **Tests**.

[a number of open tickets to improve that coverage]: https://github.com/csf-dev/ZPT-Sharp/labels/Testing
[NUnit 2.6.4]: http://nunit.org/index.php?p=docHome&r=2.6.4

As well as unit tests (one project for each tested assembly), there is also an integration tests project.
The integration tests are also built using NUnit; rather than testing individual classes, these test the complete rendering process.
The integration test fixtures all work in a similar manner:

* They load one or more source documents from a directory
* They set up an environment (the model) with some data
* The execute the rendering operation
* The result of the rendering is compared to an expected rendering

## Note on line-endings
Because the integration tests perform comparison between a rendered document and the expected output, it is important that **line endings** are correctly configured for your system.
The line-endings for all source files, as well as for the source/expected documents used with the integration tests *must* match the line endings used by your OS environment.
This means, for Windows your files must be `CRLF`-terminated.
On Linux/Mac your files must be `LF`-terminated.

If you are making use of [Git] for source control then you will need to make use of [the autocrlf configuration setting] in order to ensure that this is taken-care-of.
To initialise your git environment on a **Windows** machine, use the following command; on a **Linux** or **MacOS** machine, leave git with its default settings:

```
git config core.autocrlf true
```

[Git]: https://git-scm.com/
[the autocrlf configuration setting]: https://git-scm.com/book/tr/v2/Customizing-Git-Git-Configuration#idp31554304