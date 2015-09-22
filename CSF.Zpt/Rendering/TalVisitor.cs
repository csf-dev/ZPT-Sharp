using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Visitor type which is used to work upon an <see cref="Element"/> and perform TAL-related functionality.
  /// </summary>
  public class TalVisitor : ElementVisitor
  {
    #region methods

    /// <summary>
    /// Visit the given element and perform modifications as required.
    /// </summary>
    /// <returns><c>true</c> if the element is to remain; <c>false</c> if not</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public override bool Visit(Element element, Model model)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion
  }
}

