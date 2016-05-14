using System;
using System.Xml;
using System.IO;
using System.Linq;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="ZptDocument"/> based on a <c>System.Xml.XmlDocument</c>.
  /// </summary>
  public class ZptXmlDocument : ZptDocument
  {
    #region fields

    private XmlDocument _document;
    private SourceFileInfo _sourceFile;

    #endregion

    #region properties

    /// <summary>
    /// Gets the original XML document.
    /// </summary>
    /// <value>The original XML document.</value>
    public virtual XmlDocument Document
    {
      get {
        return _document;
      }
    }

    /// <summary>
    /// Gets information about the document's source file.
    /// </summary>
    /// <value>The source file.</value>
    public virtual SourceFileInfo SourceFile
    {
      get {
        return _sourceFile;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Renders the document to an <c>System.Xml.XmlDocument</c> instance.
    /// </summary>
    /// <returns>The rendered XML document.</returns>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    public XmlDocument RenderXml(RenderingOptions options = null)
    {
      var opts = this.GetOptions(options);
      var element = this.RenderElement(opts);

      var output = new XmlDocument();
      output.LoadXml(element.ToString());

      return output;
    }

    /// <summary>
    /// Gets a collection of elements in the document which are defined as METAL macros.
    /// </summary>
    /// <returns>Elements representing the METAL macros.</returns>
    internal override CSF.Zpt.Metal.MetalMacro[] GetMacros()
    {
      var xpath = String.Format("//*[@{0}:{1}]",
                                ZptConstants.Metal.Namespace.Prefix,
                                ZptConstants.Metal.DefineMacroAttribute);

      var nsManager = new XmlNamespaceManager(this.Document.CreateNavigator().NameTable);
      nsManager.AddNamespace(ZptConstants.Metal.Namespace.Prefix, ZptConstants.Metal.Namespace.Uri);

      return this.Document.DocumentElement
        .SelectNodes(xpath, nsManager)
        .Cast<XmlNode>()
        .Select(x => {
          var element = new ZptXmlElement(x, this.SourceFile);
          return new Metal.MetalMacro(element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute).Value, element);
        })
        .ToArray();
    }

    /// <summary>
    /// Renders an element to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="element">The element to render.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected override void Render(TextWriter writer,
                                   ZptElement element,
                                   RenderingOptions options)
    {
      if(writer == null)
      {
        throw new ArgumentNullException(nameof(writer));
      }
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      var xmlElement = element as ZptXmlElement;
      if(xmlElement == null)
      {
        string message = String.Format(ExceptionMessages.RenderedElementIncorrectType,
                                       typeof(ZptXmlElement).Name);
        throw new ArgumentException(message, "element");
      }

      var settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.IndentChars = "  ";

      using(var xmlWriter = XmlTextWriter.Create(writer, settings))
      {
        xmlElement.Node.WriteTo(xmlWriter);  
      }
    }

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected override ZptElement GetRootElement()
    {
      return new Rendering.ZptXmlElement(this.Document.DocumentElement, this.SourceFile, isRoot: true);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptXmlDocument"/> class.
    /// </summary>
    /// <param name="document">An XML document from which to create the current instance.</param>
    /// <param name="sourceFile">Information about the document's source file.</param>
    public ZptXmlDocument(XmlDocument document,
                          SourceFileInfo sourceFile)
    {
      if(document == null)
      {
        throw new ArgumentNullException(nameof(document));
      }
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }

      _document = document;
      _sourceFile = sourceFile;
    }

    #endregion
  }
}

