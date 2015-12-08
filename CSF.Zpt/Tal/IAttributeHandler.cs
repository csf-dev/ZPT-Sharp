using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Interface for a type which handles a type of TAL attribute upon an element.
  /// </summary>
  public interface IAttributeHandler
  {
    /// <summary>
    /// Handle the related attribute types which exist upon the element, if any.
    /// </summary>
    /// <returns>A response type providing information about the result of this operation.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    AttributeHandlingResult Handle(ZptElement element, Model model);
  }
}

