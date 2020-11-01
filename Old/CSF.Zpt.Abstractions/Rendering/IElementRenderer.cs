using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Service which renders a single 
  /// </summary>
  public interface IElementRenderer
  {
    /// <summary>
    /// Renders the current document, returning an <see cref="IZptElement"/> representing the rendered result.
    /// </summary>
    /// <returns>The result of the rendering process.</returns>
    /// <param name="model">An object to which the ZPT document is to be applied.</param>
    /// <param name="element">The original element to be rendered.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="IModelValueContainer"/>, to configure it.</param>
    IZptElement RenderElement(object model,
                              IZptElement element,
                              IRenderingSettings options,
                              Action<IModelValueContainer> contextConfigurator);
  }
}

