using System;
using CSF.Zpt.Rendering;
using System.Text.RegularExpressions;
using CSF.Zpt.Resources;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="IAttributeHandler"/> which handles a <c>tal:on-error</c> attribute.
  /// </summary>
  public class OnErrorAttributeHandler : IAttributeHandler
  {
    #region constants

    private const string
      ATTRIBUTE_VALUE_PATTERN                   = "^(?:(text|structure) )?(.+)$",
      STRUCTURE_INDICATOR                       = "structure";

    private static readonly Regex ValueMatcher  = new Regex(ATTRIBUTE_VALUE_PATTERN, RegexOptions.Compiled);

    #endregion

    #region methods

    /// <summary>
    /// Handle the related attribute types which exist upon the element, if any.
    /// </summary>
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public AttributeHandlingResult Handle(ZptElement element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      AttributeHandlingResult output;
      var attrib = element.GetTalAttribute(ZptConstants.Tal.OnErrorAttribute);

      if(attrib != null)
      {
        string mode;
        var expressionResult = this.GetAttributeResult(attrib, element, model, out mode);
        if(expressionResult.CancelsAction)
        {
          output = new AttributeHandlingResult(new [] { element }, true);
        }
        else if(expressionResult.Value == null)
        {
          element.RemoveAllChildren();
          output = new AttributeHandlingResult(new [] { element }, true);
        }
        else
        {
          element.ReplaceChildrenWith(expressionResult.GetValue<string>(),
                                      mode == STRUCTURE_INDICATOR);
          output = new AttributeHandlingResult(new [] { element }, true);
        }
      }
      else
      {
        output = new AttributeHandlingResult(new [] { element }, true);
      }

      return output;
    }

    /// <summary>
    /// Gets the result of the attribute value.
    /// </summary>
    /// <returns>The attribute result.</returns>
    /// <param name="attribute">The content or replace attribute.</param>
    /// <param name="element">The current element.</param>
    /// <param name="model">The model.</param>
    /// <param name="mode">Exposes the mode (either <c>text</c>, <c>structure</c> or a <c>null</c> reference).</param>
    private ExpressionResult GetAttributeResult(ZptAttribute attribute,
                                                ZptElement element,
                                                Model model,
                                                out string mode)
    {
      var match = ValueMatcher.Match(attribute.Value);

      if(!match.Success)
      {
        string message = String.Format(ExceptionMessages.ZptAttributeParsingError,
                                       ZptConstants.Tal.Namespace,
                                       attribute.Name,
                                       attribute.Value);
        throw new ParserException(message) {
          SourceAttributeName = attribute.Name,
          SourceAttributeValue = attribute.Value,
          SourceElementName = element.Name
        };
      }

      mode = match.Groups[1].Value;
      return model.Evaluate(match.Groups[2].Value, element);
    }

    #endregion
  }
}

