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
    /// Handle the related attribute types which exist upon the element, if any.
    /// </summary>
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public AttributeHandlingResult Handle(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      AttributeHandlingResult output;
      var attrib = context.Element.GetTalAttribute(ZptConstants.Tal.OmitTagAttribute);

      if(context.Element.IsInNamespace(ZptConstants.Metal.Namespace)
         || context.Element.IsInNamespace(ZptConstants.Tal.Namespace))
      {
        // Special case, for all elements in the "special" namespaces, we treat them with the omit-tag functionality
        var children = context.Element.Omit();
        output = new AttributeHandlingResult(new RenderingContext[0],
                                             false,
                                             children.Select(x => context.CreateSiblingContext(x)).ToArray());
      }
      else if(attrib != null)
      {
        // Normal handling by detecting an attribute and using its value
        var result = context.TalModel.Evaluate(attrib.Value, context.Element);
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

