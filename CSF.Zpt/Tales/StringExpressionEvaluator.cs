using System;
using CSF.Zpt.Rendering;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Implementation of <see cref="ExpressionEvaluatorBase"/> which creates string values, with optional interpolation.
  /// </summary>
  public class StringExpressionEvaluator : ExpressionEvaluatorBase
  {
    #region constants

    private static readonly string Prefix = "string";

    private const string
      ESCAPED_PLACEHOLDER   = @"\$\$",
      SINGLE_PLACEHOLDER    = @"$",
      REPLACEMENT_PATTERN   = @"\$(?:(?:\{([a-zA-Z0-9 /_.,~|?-]+)\})|([a-zA-Z0-9_/]+))";

    private static readonly Regex
      Unescaper             = new Regex(ESCAPED_PLACEHOLDER),
      ReplacementFinder     = new Regex(REPLACEMENT_PATTERN);

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
    /// <param name="element">The <see cref="ZptElement"/> for which the expression is being evaluated.</param>
    /// <param name="model">The ZPT model, providing the context for evaluation.</param>
    public override ExpressionResult Evaluate(Expression expression, RenderingContext context, TalesModel model)
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

      string source = expression.GetContent(), output;
      var escapeSequenceIndices = this.FindAndUnescapePlaceholders(source, out output);
      output = this.ApplyPlaceholderReplacements(output, escapeSequenceIndices, context, model);

      return new ExpressionResult(output);
    }

    /// <summary>
    /// Finds escaped placeholder symbols and also exposes an unescaped version of the input string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method exposes the result via an output variable, but returns a collection of the string indices at which
    /// a doubled-up placeholder character was found.
    /// </para>
    /// </remarks>
    /// <returns>A collection of the string indices at which escaped placeholders were found.</returns>
    /// <param name="input">The input string.</param>
    /// <param name="result">Exposes the unescaped output string.</param>
    private ISet<int> FindAndUnescapePlaceholders(string input, out string result)
    {
      var output = new HashSet<int>();
      int i = 0;

      result = Unescaper.Replace(input,
                                 match => {
        output.Add(match.Index - (i++));
        return SINGLE_PLACEHOLDER;
      });

      return output;
    }

    /// <summary>
    /// Applies placeholder replacements to the input string.
    /// </summary>
    /// <returns>The result of the application of placeholders.</returns>
    /// <param name="input">The input string.</param>
    /// <param name="escapedPlaceholderIndices">A collection containing the indices of escaped placeholder sequences.</param>
    /// <param name="element">The ZPT element.</param>
    /// <param name="model">The TALES model.</param>
    private string ApplyPlaceholderReplacements(string input,
                                                ISet<int> escapedPlaceholderIndices,
                                                RenderingContext context,
                                                TalesModel model)
    {
      var pathEvaluator = EvaluatorRegistry.GetEvaluator<PathExpressionEvaluator>();

      return ReplacementFinder.Replace(input, match => {
        string output;

        if(escapedPlaceholderIndices.Contains(match.Index))
        {
          output = match.Value;
        }
        else
        {
          string val = match.Groups[1].Success? match.Groups[1].Value : match.Groups[2].Value;
          var pathResult = pathEvaluator.Evaluate(new Expression(val),
                                                  context,
                                                  model);
          if(pathResult.Value == null)
          {
            output = String.Empty;
          }
          else
          {
            output = pathResult.Value.ToString();
          }
        }

        return output;
      });
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.StringExpressionEvaluator"/> class.
    /// </summary>
    /// <param name="registry">Registry.</param>
    public StringExpressionEvaluator(IEvaluatorRegistry registry) : base(registry) {}

    #endregion
  }
}

