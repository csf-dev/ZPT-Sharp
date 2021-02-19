Each project in this directory is a different ZptSharp **expression evaluator**.
Expression evaluators extend the TALES syntax, enabling additional types of expressions.
Each expression evaluator is a class which implements `ZptSharp.Expressions.IEvaluatesExpression`.

Expression evaluators are resolved from dependency injection and are registered for use with `ZptSharp.Expressions.IRegistersExpressionEvaluator`.

Note that *not all expression evaluators are in this directory*.
There are many more found in the **ZptSharp.Impl** project, in the `Expressions` directory/namespace.
Expressions which are in this directory are mainly those which have significant external dependencies and are unsuitable for inclusion in the core.