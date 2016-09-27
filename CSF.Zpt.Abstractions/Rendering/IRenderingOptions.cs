using System;
using System.Text;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Encapsulates the available options for rendering a <see cref="IZptDocument"/>.
  /// </summary>
  public interface IRenderingOptions
  {
    #region properties

    /// <summary>
    /// Gets the factory implementation with which to create <see cref="RenderingContext"/> instances.
    /// </summary>
    /// <value>The rendering context factory.</value>
    IRenderingContextFactory ContextFactory { get; }

    /// <summary>
    /// Gets or sets a value indicating whether source file annotations should be added to the rendered output.
    /// </summary>
    /// <value><c>true</c> if source file annotations are to be added; otherwise, <c>false</c>.</value>
    bool AddSourceFileAnnotation { get; }

    /// <summary>
    /// Gets the context visitors to be used when processing ZPT documents.
    /// </summary>
    /// <value>The context visitors.</value>
    IContextVisitor[] ContextVisitors { get; }

    /// <summary>
    /// Gets the encoding (eg: Unicode) for the rendered output.
    /// </summary>
    /// <value>The output encoding.</value>
    Encoding OutputEncoding { get; }

    /// <summary>
    /// Gets a value indicating whether the XML declaration should be omitted (where applicable).
    /// </summary>
    /// <value><c>true</c> if the XML declaration is to be omitted; otherwise, <c>false</c>.</value>
    bool OmitXmlDeclaration { get; }

    #endregion

    #region methods

    /// <summary>
    /// Creates a new root <see cref="IRenderingContext"/> instance.
    /// </summary>
    /// <param name="element">The root ZPT element</param>
    /// <returns>The root rendering context.</returns>
    IRenderingContext CreateRootContext(IZptElement element);

    /// <summary>
    /// Creates a new root <see cref="IRenderingContext"/> instance.
    /// </summary>
    /// <param name="element">The root ZPT element</param>
    /// <param name="model">The model to render</param>
    /// <returns>The root rendering context.</returns>
    IRenderingContext CreateRootContext(IZptElement element, object model);

    /// <summary>
    /// Gets an instance of <see cref="ITemplateFileFactory"/> from the current instance.
    /// </summary>
    /// <returns>The template file factory</returns>
    ITemplateFileFactory GetTemplateFileFactory();

    #endregion
  }
}

