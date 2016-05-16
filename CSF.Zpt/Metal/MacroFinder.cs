using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Type which finds the usage of METAL macros.
  /// </summary>
  public class MacroFinder
  {
    #region fields

    private static log4net.ILog _logger;

    #endregion

    #region methods

    /// <summary>
    /// Examines the given <see cref="ZptElement"/> and - if it uses a METAL macro - gets that macro from the given
    /// <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="ZptElement"/> instance representing the macro used, or a <c>null</c> reference.</returns>
    /// <param name="element">The element to examine for macro usage.</param>
    /// <param name="model">The METAL model.</param>
    public virtual ZptElement GetUsedMacro(RenderingContext context)
    {
      return this.GetReferencedMacro(context, ZptConstants.Metal.UseMacroAttribute);
    }

    /// <summary>
    /// Examines the given <see cref="ZptElement"/> and - if it extends a METAL macro - gets that macro from the given
    /// <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="ZptElement"/> instance representing the macro extended, or a <c>null</c> reference.</returns>
    /// <param name="macro">The macro element to examine for extension.</param>
    /// <param name="model">The METAL model.</param>
    public virtual ZptElement GetExtendedMacro(RenderingContext context)
    {
      return this.GetReferencedMacro(context, ZptConstants.Metal.ExtendMacroAttribute);
    }

    /// <summary>
    /// Examines the given <see cref="ZptElement"/> and - if it has an attribute referencing another macro - gets that
    /// macro from the given <see cref="Model"/>.
    /// </summary>
    /// <returns>Either an <see cref="ZptElement"/> instance representing the referenced macro, or a <c>null</c> reference.</returns>
    /// <param name="element">The element to examine for the reference.</param>
    /// <param name="model">The METAL model.</param>
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
          _logger.Warn(ex);
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

    #region constructor

    static MacroFinder()
    {
      _logger = log4net.LogManager.GetLogger(typeof(MacroFinder));
    }

    #endregion
  }
}

