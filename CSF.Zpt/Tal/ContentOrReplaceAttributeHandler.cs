using System;
using System.Text.RegularExpressions;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;
using System.Linq;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="IAttributeHandler"/> which handles either a <c>tal:content</c> or
  /// <c>tal:replace</c> attribute.
  /// </summary>
  public class ContentOrReplaceAttributeHandler : IAttributeHandler
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
    public AttributeHandlingResult Handle(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      AttributeHandlingResult output;
      string attribName;

      var attrib = this.GetAttribute(context.Element, out attribName);

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
          if(attribName == ZptConstants.Tal.ContentAttribute)
          {
            context.Element.RemoveAllChildren();
            output = new AttributeHandlingResult(new [] { context }, true);
          }
          else
          {
            context.Element.Remove();
            output = new AttributeHandlingResult(new RenderingContext[0], false);
          }
        }
        else
        {
          if(attribName == ZptConstants.Tal.ContentAttribute)
          {
            context.Element.ReplaceChildrenWith(expressionResult.GetValue<string>(),
                                        mode == STRUCTURE_INDICATOR);
            output = new AttributeHandlingResult(new [] { context }, true);
          }
          else
          {
            var elements = context.Element.ReplaceWith(expressionResult.GetValue<string>(),
                                               mode == STRUCTURE_INDICATOR);
            output = new AttributeHandlingResult(elements.Select(x => context.CreateSiblingContext(x)).ToArray(), false);
          }
        }
      }
      else
      {
        output = new AttributeHandlingResult(new [] { context }, true);
      }

      return output;
    }

    /// <summary>
    /// Gets either the TAL 'content' or 'replace' attribute from the given element.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="element">The element from which to get an attribute.</param>
    /// <param name="attribName">Exposes the name of the attribute.</param>
    private ZptAttribute GetAttribute(ZptElement element, out string attribName)
    {
      ZptAttribute
        contentAttrib = element.GetTalAttribute(ZptConstants.Tal.ContentAttribute),
        replaceAttrib = element.GetTalAttribute(ZptConstants.Tal.ReplaceAttribute),
        output;

      if(contentAttrib != null
         && replaceAttrib != null)
      {
        string message = String.Format(ExceptionMessages.ContentAndReplaceAttributesCannotCoexist,
                                       element.Name);
        throw new ParserException(message) {
          SourceElementName = element.Name
        };
      }
      else if(contentAttrib != null)
      {
        output = contentAttrib;
        attribName = ZptConstants.Tal.ContentAttribute;
      }
      else
      {
        output = replaceAttrib;
        attribName = ZptConstants.Tal.ReplaceAttribute;
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

