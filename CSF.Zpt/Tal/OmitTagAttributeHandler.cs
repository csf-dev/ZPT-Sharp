using System;
using CSF.Zpt.Rendering;
using System.Linq;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Implementation of <see cref="IAttributeHandler"/> which handles a <c>tal:omit-tag</c> attribute.
  /// </summary>
  public class OmitTagAttributeHandler : IAttributeHandler
  {
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
      var attrib = context.GetTalAttribute(ZptConstants.Tal.OmitTagAttribute);

      if(attrib != null)
      {
        // Normal handling by detecting an attribute and using its value
        var result = context.TalModel.Evaluate(attrib.Value, context);
        if(!result.CancelsAction && result.GetValueAsBoolean())
        {
          var children = context.Element.Omit();
          output = new AttributeHandlingResult(new RenderingContext[0],
                                               false,
                                               children.Select(x => context.CreateSiblingContext(x)).ToArray());
        }
        else
        {
          output = new AttributeHandlingResult(new [] { context }, true);
        }
      }
      else
      {
        output = new AttributeHandlingResult(new [] { context }, true);
      }

      return output;
    }

    #endregion
  }
}

