# Contributing to & extending ZptSharp

Contributions to ZptSharp are very welcome, whatever those contributions may be.
Good ways to contribute are:

* To [report issues and enhancement requests] to the GitHub issue tracker
* To answer [questions and discussions]
* To evaluate the documentation & suggest improvements
* To work on any of the open issues from the tracker

[report issues and enhancement requests]: https://github.com/csf-dev/ZPT-Sharp/issues
[questions and discussions]: https://github.com/csf-dev/ZPT-Sharp/discussions

## Extending ZptSharp

The two primary extension points of ZptSharp are **[document providers]** and **[expression evaluators]**.

Document providers provide the 'bridge' between an implementation of a markup DOM, such as HTML Agility Pack, and the simplified DOM abstraction used by ZptSharp.
If you wish to use a new DOM implementation (for example, a new HTML reading/writing library is released) then you may write a document provider to integrate it into ZptSharp.

Expression evaluators add new TALES expression types.
For example, imagine a library which supports evaluating Ruby expressions from .NET.
This could be integrated into ZptSharp with an expression evaluator, adding support for a hypothetical `ruby:` expression-type.

[document providers]: WritingDocumentProviders.md
[expression evaluators]: WritingExpressionEvaluators.md