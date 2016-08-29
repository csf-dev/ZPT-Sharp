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

    #region fields

    private static log4net.ILog _logger;

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
      var attrib = context.GetTalAttribute(ZptConstants.Tal.OnErrorAttribute);

      if(attrib != null)
      {
        string mode;
        var expressionResult = this.GetAttributeResult(attrib, context, out mode);
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

        _logger.InfoFormat(Resources.LogMessageFormats.TalErrorHandled,
                           context.Element.GetFullFilePathAndLocation());
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
    /// <param name="context">The rendering context.</param>
    /// <param name="mode">Exposes the mode (either <c>text</c>, <c>structure</c> or a <c>null</c> reference).</param>
    private ExpressionResult GetAttributeResult(IZptAttribute attribute,
                                                RenderingContext context,
                                                out string mode)
    {
      var match = ValueMatcher.Match(attribute.Value);

      if(!match.Success)
      {
        string message = String.Format(ExceptionMessages.ZptAttributeParsingError,
                                       ZptConstants.Tal.Namespace,
                                       attribute.Name,
                                       attribute.Value,
                                       context.Element.Name);
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

    #region constructor

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Tal.OnErrorAttributeHandler"/> class.
    /// </summary>
    static OnErrorAttributeHandler()
    {
      _logger = log4net.LogManager.GetLogger(typeof(OnErrorAttributeHandler));
    }

    #endregion
  }
}

