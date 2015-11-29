using System;

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
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      new TalDefineAttributeHandler().Handle(element, model);

      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <returns>The recursively.</returns>
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public override Element VisitRecursively(Element element, Model model)
    {
      var output = base.VisitRecursively(element, model);

      output.PurgeTalAttributes();

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.TalVisitor"/> class.
    /// </summary>
    /// <param name="options">Rendering options.</param>
    public TalVisitor(RenderingOptions options = null) : base(options: options)
    {
      
    }

    #endregion
  }
}

