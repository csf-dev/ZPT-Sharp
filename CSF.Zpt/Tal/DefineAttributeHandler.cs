using System;
using System.Text.RegularExpressions;
using System.Linq;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="IAttributeHandler"/> which handles a <c>tal:define</c> attribute.
  /// </summary>
  public class DefineAttributeHandler : IAttributeHandler
  {
    #region constants

    private const string
      GLOBAL_SCOPE        = "global",
      DEFINITION_PATTERN  = "^(?:(local|global) )?([^ ]+) (.+)$",
      ITEM_PATTERN        = @"((?:[^;]|;;)+)\s*(?:;(?!;))?\s*",
      ESCAPED_SEMICOLON   = ";;",
      SEMICOLON           = ";";

    private static readonly Regex
      Definition          = new Regex(DEFINITION_PATTERN, RegexOptions.Compiled),
      ItemMatcher         = new Regex(ITEM_PATTERN, RegexOptions.Compiled);

    #endregion

    #region methods

    /// <summary>
    /// Handle the related attribute types which exist upon the element, if any.
    /// </summary>
    /// <returns>A collection of elements which are present in the DOM after this handler has completed its work.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public ZptElement[] Handle(ZptElement element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      var attrib = element.GetTalAttribute(ZptConstants.Tal.DefineAttribute);

      if(attrib != null)
      {
        var itemMatches = ItemMatcher
          .Matches(attrib.Value)
          .Cast<Match>()
          .Select(x => Definition.Match(x.Groups[1].Value));

        if(itemMatches.Any(x => !x.Success))
        {
          string message = String.Format(Resources.ExceptionMessages.ZptAttributeParsingError,
                                         ZptConstants.Tal.DefaultPrefix,
                                         ZptConstants.Tal.DefineAttribute,
                                         attrib.Value);
          throw new ParserException(message) {
            SourceElementName = element.Name,
            SourceAttributeName = attrib.Name,
            SourceAttributeValue = attrib.Value
          };
        }

        var items = itemMatches
          .Select(x => new {  Scope = x.Groups[1].Value,
                              Name = x.Groups[2].Value,
                              Expression = x.Groups[3].Value })
          .ToArray();

        foreach(var item in items)
        {
          ExpressionResult result;
          var unescapedExpression = this.UnescapeSemicolons(item.Expression);

          try
          {
            result = model.Evaluate(unescapedExpression, element);
          }
          catch(Exception ex)
          {
            string message = String.Format(ExceptionMessages.ExpressionEvaluationException,
                                           ZptConstants.Tal.DefaultPrefix,
                                           ZptConstants.Tal.DefineAttribute,
                                           item.Expression);
            throw new ModelEvaluationException(message, ex) {
              ExpressionText = item.Expression
            };
          }

          if(!result.CancelsAction())
          {
            if(item.Scope == GLOBAL_SCOPE)
            {
              model.AddGlobal(item.Name, result.GetResult());
            }
            else
            {
              model.AddLocal(item.Name, result.GetResult());
            }
          }
        }
      }

      return new[] { element };
    }

    /// <summary>
    /// Unescapes doubled-up semicolons into singular semicolons.
    /// </summary>
    /// <returns>The unescaped string.</returns>
    /// <param name="expression">A source expression which may contain escaped semicolons.</param>
    private string UnescapeSemicolons(string expression)
    {
      if(expression == null)
      {
        throw new ArgumentNullException("expression");
      }

      return expression.Replace(ESCAPED_SEMICOLON, SEMICOLON);
    }

    #endregion
  }
}

