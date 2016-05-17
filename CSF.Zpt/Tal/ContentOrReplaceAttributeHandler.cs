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
      string attribName;

      var attrib = this.GetAttribute(context, out attribName);

      if(attrib != null)
      {
        string mode;
        var expressionResult = this.GetAttributeResult(attrib, context, out mode);

        if(expressionResult.CancelsAction && attribName == ZptConstants.Tal.ContentAttribute)
        {
          output = new AttributeHandlingResult(new [] { context }, true);
        }
        else if(expressionResult.CancelsAction && attribName == ZptConstants.Tal.ReplaceAttribute)
        {
          var children = context.Element.Omit();
          output = new AttributeHandlingResult(new RenderingContext[0],
                                               false,
                                               children.Select(x => context.CreateSiblingContext(x)).ToArray());
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
            var resultValue = expressionResult.GetValue<object>();
            context.Element.ReplaceChildrenWith((resultValue?? String.Empty).ToString(),
                                                mode == STRUCTURE_INDICATOR);
            output = new AttributeHandlingResult(new [] { context }, true);
          }
          else
          {
            var resultValue = expressionResult.GetValue<object>();
            var elements = context.Element.ReplaceWith((resultValue?? String.Empty).ToString(),
                                                       mode == STRUCTURE_INDICATOR);
            output = new AttributeHandlingResult(elements.Select(x => context.CreateSiblingContext(x, true)).ToArray(), true);
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
    private ZptAttribute GetAttribute(RenderingContext context, out string attribName)
    {
      ZptAttribute
        contentAttrib = context.GetTalAttribute(ZptConstants.Tal.ContentAttribute),
        replaceAttrib = context.GetTalAttribute(ZptConstants.Tal.ReplaceAttribute),
        output;

      if(contentAttrib != null
         && replaceAttrib != null)
      {
        string message = String.Format(ExceptionMessages.ContentAndReplaceAttributesCannotCoexist,
                                       context.Element.Name);
        throw new ParserException(message) {
          SourceElementName = context.Element.Name
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
                                                RenderingContext context,
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
          SourceElementName = context.Element.Name
        };
      }

      mode = match.Groups[1].Value;
      return context.TalModel.Evaluate(match.Groups[2].Value, context);
    }

    #endregion
  }
}

