using System;
using CSF.Zpt.Rendering;

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
      var attrib = element.GetTalAttribute(ZptConstants.Tal.OmitTagAttribute);

      if(element.IsInNamespace(ZptConstants.Metal.Namespace)
         || element.IsInNamespace(ZptConstants.Tal.Namespace))
      {
        // Special case, for all elements in the "special" namespaces, we treat them with the omit-tag functionality
        var children = element.Omit();
        output = new AttributeHandlingResult(new ZptElement[0], false, children);
      }
      else if(attrib != null)
      {
        // Normal handling by detecting an attribute and using its value
        var result = model.Evaluate(attrib.Value, element);
        if(!result.CancelsAction && result.GetResultAsBoolean())
        {
          var children = element.Omit();
          output = new AttributeHandlingResult(new ZptElement[0], false, children);
        }
        else
        {
          output = new AttributeHandlingResult(new [] { element }, true);
        }
      }
      else
      {
        output = new AttributeHandlingResult(new [] { element }, true);
      }

      return output;
    }

    #endregion
  }
}

