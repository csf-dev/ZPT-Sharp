//
// MyClass.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2018 Craig Fowler
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Text.RegularExpressions;
using CSF.Zpt.ExpressionEvaluators.PathExpressions;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;

namespace CSF.Zpt.ExpressionEvaluators.PipeExpressions
{
  public class PipeExpresionEvaluator : ExpressionEvaluatorBase
  {
    #region constants

    const string MatchPattern = @"([^ ]+) +([^ ]+)";
    static readonly Regex ExpressionMatcher = new Regex(MatchPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
    static readonly string Prefix = "pipe";


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

    public override ExpressionResult Evaluate(Expression expression, IRenderingContext context, ITalesModel model)
    {
      if(model == null)
        throw new ArgumentNullException(nameof(model));
      if(context == null)
        throw new ArgumentNullException(nameof(context));
      if(expression == null)
        throw new ArgumentNullException(nameof(expression));

      var trimmedContent = expression.Content.Trim();
      var expressionMatch = ExpressionMatcher.Match(trimmedContent);

      if(!expressionMatch.Success)
      {
        throw new ModelEvaluationException("The pipe expression must be well-formed")
        {
          ExpressionText = expression.ToString(),
          ElementName = context.Element.Name
        };
      }
        
      var valueExpression = ExpressionCreator.Create(PathExpressionEvaluator.GetPrefix(), expressionMatch.Groups[1].Value);
      var pipeExpression = ExpressionCreator.Create(PathExpressionEvaluator.GetPrefix(), expressionMatch.Groups[2].Value);

      var valueExpressionResult = model.Evaluate(valueExpression, context);
      var pipeExpressionResult = model.Evaluate(pipeExpression, context);

      var pipeFunction = pipeExpressionResult.Value as Func<object,object>;
      if(pipeFunction == null) return new ExpressionResult(null);

      var pipedValue = pipeFunction(valueExpressionResult.Value);

      return new ExpressionResult(pipedValue);
    }

    #endregion
  }
}
