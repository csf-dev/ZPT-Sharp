using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Type which finds the usage of METAL macros.
  /// </summary>
  public class MacroFinder : IMacroFinder
  {
    #region methods

    /// <summary>
    /// Examines the given <see cref="IRenderingContext"/> and - if it uses a METAL macro - gets that macro from the
    /// model contained within that context.
    /// </summary>
    /// <returns>Either an <see cref="IZptElement"/> instance representing the macro used, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context.</param>
    public virtual IZptElement GetUsedMacro(IRenderingContext context)
    {
      return this.GetReferencedMacro(context, ZptConstants.Metal.UseMacroAttribute);
    }

    /// <summary>
    /// Examines the given <see cref="IRenderingContext"/> and - if it extends a METAL macro - gets that macro from the
    /// model contained within that context.
    /// </summary>
    /// <returns>Either an <see cref="IZptElement"/> instance representing the macro extended, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context.</param>
    public virtual IZptElement GetExtendedMacro(IRenderingContext context)
    {
      return this.GetReferencedMacro(context, ZptConstants.Metal.ExtendMacroAttribute);
    }

    /// <summary>
    /// Examines the given <see cref="IZptElement"/> and - if it has an attribute referencing another macro - gets that
    /// macro from the given <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="IZptElement"/> instance representing the referenced macro, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context.</param>
    /// <param name="attributeName">The name of the desired attribute, which references a macro if present.</param>
    private IZptElement GetReferencedMacro(IRenderingContext context, string attributeName)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      IZptElement output;
      var attrib = context.GetMetalAttribute(attributeName);

      if(attrib != null)
      {
        ExpressionResult result;

        try
        {
          result = context.MetalModel.Evaluate(attrib.Value, context);
          output = result.GetValue<IZptElement>().Clone();
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

