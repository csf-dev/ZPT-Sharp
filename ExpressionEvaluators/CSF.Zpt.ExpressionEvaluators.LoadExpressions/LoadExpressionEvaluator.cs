using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.ExpressionEvaluators.LoadExpressions
{
  /// <summary>
  /// Implementation of <see cref="ExpressionEvaluatorBase"/> which evaluates a 'child' expression, converts the
  /// result into a document which may be rendered, renders it and then returns the result as a string.
  /// </summary>
  public class LoadExpressionEvaluator : ExpressionEvaluatorBase
  {
    #region constants

    private static readonly string Prefix = "load";

    #endregion

    #region properties

    /// <summary>
    /// Gets the expression prefix handled by the current evaluator instance.
    /// </summary>
    /// <value>The prefix.</value>
    public override string ExpressionPrefix
    {
      get {
        return Prefix;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Evaluate the specified expression, for the given element and model.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="context">The rendering context for the expression being evaluated.</param>
    /// <param name="model">The ZPT model, providing the context for evaluation.</param>
    public override ExpressionResult Evaluate(Expression expression, IRenderingContext context, ITalesModel model)
    {
      if(expression == null)
      {
        throw new ArgumentNullException(nameof(expression));
      }
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var innerExpressionResult = GetExpressionResult(expression, context, model);

      if(innerExpressionResult.CancelsAction
         || ReferenceEquals(innerExpressionResult.Value, null))
      {
        return innerExpressionResult;
      }

      return new ExpressionResult(RenderDocument(innerExpressionResult.Value, context));
    }

    /// <summary>
    /// Gets the result of the given 'inner' expression.
    /// </summary>
    /// <returns>The expression result.</returns>
    /// <param name="expression">Expression.</param>
    /// <param name="context">Context.</param>
    /// <param name="model">Model.</param>
    protected virtual ExpressionResult GetExpressionResult(Expression expression,
                                                           IRenderingContext context,
                                                           ITalesModel model)
    {
      if(expression == null)
      {
        throw new ArgumentNullException(nameof(expression));
      }
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var trimmedContent = expression.Content.TrimStart();
      return model.Evaluate(ExpressionCreator.Create(trimmedContent), context);
    }

    /// <summary>
    /// Renders a document and returns its result.
    /// </summary>
    /// <returns>The document.</returns>
    /// <param name="document">Document.</param>
    /// <param name="context">Context.</param>
    protected string RenderDocument(object document, IRenderingContext context)
    {
      if(document == null)
      {
        throw new ArgumentNullException(nameof(document));
      }
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion
  }
}

