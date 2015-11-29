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
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    void Handle(Element element, Model model);
  }
}

