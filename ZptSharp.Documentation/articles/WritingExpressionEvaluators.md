# Writing expression evaluators

Expression evaluators are classes which implement the interface [`IEvaluatesExpression`].
You are encouraged to read the API documentation for this interface.

[`IEvaluatesExpression`]: xref:ZptSharp.Expressions.IEvaluatesExpression

## No need to deal with expression prefixes

TALES expression evaluators operate upon a prefix-basis; the prefix is used to determine which evaluator implementation should receive the expression.
When the implementation of `IEvaluatesExpression` receives the evaluation request, the prefix which lead to that implementation class *will have already been removed* from the expression.

For example, if you are creating a new `ruby:` expression type, you will register your evaluator implementation with the `ruby` prefix string.
Once a Ruby expression reaches your evaluator, the `ruby:` prefix will have already been removed from the expression string.

## Registering evaluators with their prefixes

The actual registration of an evaluator implementation class and the association of that implementation type with a prefix is performed during dependency injection set-up.
Typically, a new expression evaluator will add an extension method for [`IBuildsHostingEnvironment`] which is used to add its services to DI and also register the implementation with its prefix.
That extension method should register the implementation type and prefix like so:

```csharp
builder.ServiceRegistry.ExpresionEvaluatorTypes
  .Add("myprefix", typeof(MyEvaluatorType));
```

Where:

* `builder` is the `IBuildsHostingEnvironment`
* `"myprefix"` is the prefix for your expression type
* `MyEvaluatorType` is the concrete class which implements `IEvaluatesExpression`

[`IBuildsHostingEnvironment`]: xref:ZptSharp.Hosting.IBuildsHostingEnvironment

## Commonly-used services

Because ZptSharp is based upon dependency injection, any class may constructor-inject other service interfaces freely.

* Inject an `IEvaluatesExpression` in order to be able to evaluate nested expressions
* Inject [`IEvaluatesDomValueExpression`] to evaluate expressions which return content specifically for the DOM
* Add your own services to dependency injection and inject them, as required

[`IEvaluatesDomValueExpression`]: xref:ZptSharp.Tal.IEvaluatesDomValueExpression