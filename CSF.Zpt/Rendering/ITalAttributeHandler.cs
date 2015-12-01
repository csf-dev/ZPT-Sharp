using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which handles a type of TAL attribute upon an element.
  /// </summary>
  public interface ITalAttributeHandler
  {
    /// <summary>
    /// Handle the related attribute types which exist upon the element, if any.
    /// </summary>
    /// <returns>A collection of elements which are present in the DOM after this handler has completed its work.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    ZptElement[] Handle(ZptElement element, Model model);
  }
}

