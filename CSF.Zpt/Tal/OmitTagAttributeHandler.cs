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
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion
  }
}

