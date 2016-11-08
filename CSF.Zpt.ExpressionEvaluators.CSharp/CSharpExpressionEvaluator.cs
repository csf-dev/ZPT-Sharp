using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;
using System.Text;
using System.Dynamic;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharp
{
  public class CSharpExpressionEvaluator : IExpressionEvaluator
  {
    #region properties

    /// <summary>
    /// Gets the expression prefix handled by the current evaluator instance.
    /// </summary>
    /// <value>The prefix.</value>
    public string ExpressionPrefix { get { return "csharp"; } }

    #endregion

    #region methods

    /// <summary>
    /// Evaluate the specified expression, for the given element and model.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="context">The rendering context for the expression being evaluated.</param>
    /// <param name="model">The ZPT model, providing the context for evaluation.</param>
    public ExpressionResult Evaluate(Expression expression, IRenderingContext context, ITalesModel model)
    {
      var script = new StringBuilder();

      dynamic globalsContainer = new ExpandoObject();
      var globalsBuilder = (IDictionary<string,Object>) globalsContainer;


    }

    #endregion
  }
}

