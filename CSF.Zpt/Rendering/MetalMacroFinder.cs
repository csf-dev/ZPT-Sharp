using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Type which finds the usage of METAL macros.
  /// </summary>
  public class MetalMacroFinder
  {
    #region methods

    /// <summary>
    /// Examines the given <see cref="Element"/> and - if it uses a METAL macro - gets that macro from the given
    /// <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="Element"/> instance representing the macro used, or a <c>null</c> reference.</returns>
    /// <param name="element">The element to examine for macro usage.</param>
    /// <param name="model">The METAL model.</param>
    public Element GetUsedMacro(Element element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      Element output = null;
      var useMacroAttrib = element.GetMetalAttribute(Metal.UseMacroAttribute);

      if(useMacroAttrib != null)
      {
        var result = model.Evaluate(useMacroAttrib.Value);
        if(result.EvaluationSuccess)
        {
          output = result.GetResult<Element>();
          output = this.ApplyMacroExtension(output);
        }
      }

      return output;
    }

    private Element ApplyMacroExtension(Element defineMacroElement)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion
  }
}

