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
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public AttributeHandlingResult Handle(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var attrib = context.Element.GetTalAttribute(ZptConstants.Tal.DefineAttribute);

      if(attrib != null)
      {
        var itemMatches = ItemMatcher
          .Matches(attrib.Value)
          .Cast<Match>()
          .Select(x => Definition.Match(x.Groups[1].Value));

        if(itemMatches.Any(x => !x.Success))
        {
          string message = String.Format(Resources.ExceptionMessages.ZptAttributeParsingError,
                                         ZptConstants.Tal.Namespace,
                                         ZptConstants.Tal.DefineAttribute,
                                         attrib.Value);
          throw new ParserException(message) {
            SourceElementName = context.Element.Name,
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
            result = context.TalModel.Evaluate(unescapedExpression, context.Element);
          }
          catch(Exception ex)
          {
            string message = String.Format(ExceptionMessages.ExpressionEvaluationException,
                                           ZptConstants.Tal.Namespace,
                                           ZptConstants.Tal.DefineAttribute,
                                           item.Expression);
            throw new ModelEvaluationException(message, ex) {
              ExpressionText = item.Expression
            };
          }

          if(!result.CancelsAction)
          {
            if(item.Scope == GLOBAL_SCOPE)
            {
              context.TalModel.AddGlobal(item.Name, result.Value);
            }
            else
            {
              context.TalModel.AddLocal(item.Name, result.Value);
            }
          }
        }
      }

      return new AttributeHandlingResult(new [] { context }, true);
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

