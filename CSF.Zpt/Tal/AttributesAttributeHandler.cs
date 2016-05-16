using System;
using CSF.Zpt.Rendering;
using System.Text.RegularExpressions;
using System.Linq;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="IAttributeHandler"/> which handles a <c>tal:attributes</c> attribute.
  /// </summary>
  public class AttributesAttributeHandler : IAttributeHandler
  {
    #region constants

    private const string
      ATTRIBUTE_PATTERN   = @"^(?:(?:([^:]+):)?([^ ]+) )([^ ]+)$",
      ITEM_PATTERN        = @"((?:[^;]|;;)+)\s*(?:;(?!;))?\s*",
      ESCAPED_SEMICOLON   = ";;",
      SEMICOLON           = ";";

    private static readonly Regex
      Attribute           = new Regex(ATTRIBUTE_PATTERN, RegexOptions.Compiled),
      ItemMatcher         = new Regex(ITEM_PATTERN, RegexOptions.Compiled);


    #endregion

    #region methods

    /// <summary>
    /// Handle the related attribute types which exist upon the element exposed by the given context, if any.
    /// </summary>
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="context">The rendering context, which exposes a ZPT element.</param>
    public AttributeHandlingResult Handle(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var attrib = context.GetTalAttribute(ZptConstants.Tal.AttributesAttribute);

      if(attrib != null)
      {
        var itemMatches = ItemMatcher
          .Matches(attrib.Value)
          .Cast<Match>()
          .Select(x => Attribute.Match(x.Groups[1].Value));

        if(itemMatches.Any(x => !x.Success))
        {
          string message = String.Format(Resources.ExceptionMessages.ZptAttributeParsingError,
                                         ZptConstants.Tal.Namespace,
                                         ZptConstants.Tal.AttributesAttribute,
                                         attrib.Value);
          throw new ParserException(message) {
            SourceElementName = context.Element.Name,
            SourceAttributeName = attrib.Name,
            SourceAttributeValue = attrib.Value
          };
        }

        var items = itemMatches
          .Select(x => new {  Prefix = x.Groups[1].Value,
                              AttributeName = x.Groups[2].Value,
                              Expression = x.Groups[3].Value })
          .ToArray();

        foreach(var item in items)
        {
          ExpressionResult result;
          var unescapedExpression = this.UnescapeSemicolons(item.Expression);

          try
          {
            result = context.TalModel.Evaluate(unescapedExpression, context);
          }
          catch(Exception ex)
          {
            string message = String.Format(Resources.ExceptionMessages.ExpressionEvaluationException,
                                           ZptConstants.Tal.Namespace,
                                           ZptConstants.Tal.AttributesAttribute,
                                           item.Expression);
            throw new ModelEvaluationException(message, ex) {
              ExpressionText = item.Expression
            };
          }

          if(!result.CancelsAction)
          {
            var nspace = new ZptNamespace(prefix: item.Prefix);
            if(result.Value == null)
            {
              if(String.IsNullOrEmpty(item.Prefix))
              {
                context.Element.RemoveAttribute(item.AttributeName);
              }
              else
              {
                context.Element.RemoveAttribute(nspace, item.AttributeName);
              }
            }
            else
            {
              if(String.IsNullOrEmpty(item.Prefix))
              {
                context.Element.SetAttribute(item.AttributeName, result.Value.ToString());
              }
              else
              {
                context.Element.SetAttribute(nspace, item.AttributeName, result.Value.ToString());
              }
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
        throw new ArgumentNullException(nameof(expression));
      }

      return expression.Replace(ESCAPED_SEMICOLON, SEMICOLON);
    }

    #endregion
  }
}

