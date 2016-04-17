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

      AttributeHandlingResult output;
      var attrib = context.Element.GetTalAttribute(ZptConstants.Tal.OnErrorAttribute);

      if(attrib != null)
      {
        string mode;
        var expressionResult = this.GetAttributeResult(attrib, context.Element, context.TalModel, out mode);
        if(expressionResult.CancelsAction)
        {
          output = new AttributeHandlingResult(new [] { context }, true);
        }
        else if(expressionResult.Value == null)
        {
          context.Element.RemoveAllChildren();
          output = new AttributeHandlingResult(new [] { context }, true);
        }
        else
        {
          context.Element.ReplaceChildrenWith(expressionResult.GetValue<string>(),
                                              mode == STRUCTURE_INDICATOR);
          output = new AttributeHandlingResult(new [] { context }, true);
        }
      }
      else
      {
        output = new AttributeHandlingResult(new [] { context }, true);
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

