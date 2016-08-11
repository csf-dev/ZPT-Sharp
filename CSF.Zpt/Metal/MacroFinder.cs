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
    /// <param name="context">The rendering context.</param>
    public virtual ZptElement GetUsedMacro(RenderingContext context)
    {
      return this.GetReferencedMacro(context, ZptConstants.Metal.UseMacroAttribute);
    }

    /// <summary>
    /// Examines the given <see cref="ZptElement"/> and - if it extends a METAL macro - gets that macro from the given
    /// <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="ZptElement"/> instance representing the macro extended, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context.</param>
    public virtual ZptElement GetExtendedMacro(RenderingContext context)
    {
      return this.GetReferencedMacro(context, ZptConstants.Metal.ExtendMacroAttribute);
    }

    /// <summary>
    /// Examines the given <see cref="ZptElement"/> and - if it has an attribute referencing another macro - gets that
    /// macro from the given <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="ZptElement"/> instance representing the referenced macro, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context.</param>
    /// <param name="attributeName">The name of the desired attribute, which references a macro if present.</param>
    private ZptElement GetReferencedMacro(RenderingContext context, string attributeName)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      ZptElement output;
      var attrib = context.GetMetalAttribute(attributeName);

      if(attrib != null)
      {
        ExpressionResult result;

        try
        {
          result = context.MetalModel.Evaluate(attrib.Value, context);
          output = result.GetValue<ZptElement>().Clone();
        }
        catch(Exception ex)
        {
          string message = String.Format(Resources.ExceptionMessages.UnexpectedExceptionGettingMacro,
                                         attributeName,
                                         attrib.Value,
                                         context.Element.GetFullFilePathAndLocation());
          throw new RenderingException(message, ex);
        }

        if(output == null)
        {
          string message = String.Format(Resources.ExceptionMessages.CannotFindMacro,
                                         attributeName,
                                         attrib.Value,
                                         context.Element.GetFullFilePathAndLocation());
          throw new MacroNotFoundException(message);
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

