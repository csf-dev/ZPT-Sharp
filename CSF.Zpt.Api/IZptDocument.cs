using System;
using CSF.Zpt.Rendering;
using System.IO;
using CSF.Zpt.Tales;

namespace CSF.Zpt
{
  /// <summary>
  /// Represents a ZPT document.
  /// </summary>
  public interface IZptDocument
  {
    /// <summary>
    /// Renders the document to a <c>System.String</c>.
    /// </summary>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    string Render(RenderingOptions options = null,
                  Action<RenderingContext> contextConfigurator = null);

    /// <summary>
    /// Renders the document to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    void Render(TextWriter writer,
                RenderingOptions options = null,
                Action<RenderingContext> contextConfigurator = null);

    /// <summary>
    /// Gets a collection of elements in the document which are defined as METAL macros.
    /// </summary>
    /// <returns>Elements representing the METAL macros.</returns>
    ITalesPathHandler GetMacros();

    /// <summary>
    /// Gets information about the source file for the current instance.
    /// </summary>
    /// <returns>The file info.</returns>
    ISourceInfo GetSourceFileInfo();
  }
}

