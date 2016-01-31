using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Type which finds the usage of METAL macros.
  /// </summary>
  public class MacroFinder
  {
    #region methods

    /// <summary>
    /// Examines the given <see cref="ZptElement"/> and - if it uses a METAL macro - gets that macro from the given
    /// <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="ZptElement"/> instance representing the macro used, or a <c>null</c> reference.</returns>
    /// <param name="element">The element to examine for macro usage.</param>
    /// <param name="model">The METAL model.</param>
    public virtual ZptElement GetUsedMacro(ZptElement element, Model model)
    {
      return this.GetReferencedMacro(element, model, ZptConstants.Metal.UseMacroAttribute);
    }

    /// <summary>
    /// Examines the given <see cref="ZptElement"/> and - if it extends a METAL macro - gets that macro from the given
    /// <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="ZptElement"/> instance representing the macro extended, or a <c>null</c> reference.</returns>
    /// <param name="macro">The macro element to examine for extension.</param>
    /// <param name="model">The METAL model.</param>
    public virtual ZptElement GetExtendedMacro(ZptElement macro, Model model)
    {
      return this.GetReferencedMacro(macro, model, ZptConstants.Metal.ExtendMacroAttribute);
    }

    /// <summary>
    /// Examines the given <see cref="ZptElement"/> and - if it has an attribute referencing another macro - gets that
    /// macro from the given <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="ZptElement"/> instance representing the referenced macro, or a <c>null</c> reference.</returns>
    /// <param name="element">The element to examine for the reference.</param>
    /// <param name="model">The METAL model.</param>
    /// <param name="attributeName">The name of the desired attribute, which references a macro if present.</param>
    private ZptElement GetReferencedMacro(ZptElement element, Model model, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      ZptElement output;
      var attrib = element.GetMetalAttribute(attributeName);

      if(attrib != null)
      {
        ExpressionResult result;

        try
        {
          result = model.Evaluate(attrib.Value, element);
          output = result.GetValue<ZptElement>();
        }
        catch(Exception)
        {
          output = null;
        }
      }
      else
      {
        output = null;
      }

      return output;
    }

    #endregion
  }
}

