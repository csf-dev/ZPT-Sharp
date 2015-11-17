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
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public override Element Visit(Element element, Model model)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion
  }
}

