using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Encapsulates the TALEs built-in variable names.
  /// </summary>
  public class BuiltinContextsContainer : ITalesPathHandler
  {
    #region constants

    private const string
      NOTHING   = "nothing",
      DEFAULT   = "default",
      OPTIONS   = "options",
      REPEAT    = "repeat",
      ATTRS     = "attrs",
      TEMPLATE  = "template",
      CONTAINER = "container",
      HERE      = "here";

    #endregion

    #region fields

    private object _nothing, _default, _model;
    private NamedObjectWrapper _options;
    private RepetitionMetadataCollectionWrapper _repeat;
    private Lazy<OriginalAttributeValuesCollection> _attrs;
    private ITemplateFileFactory _templateFileFactory;

    #endregion

    #region properties

    /// <summary>
    /// Gets the 'nothing' object instance (a <c>null</c> reference).
    /// </summary>
    /// <value>The nothing object.</value>
    public object Nothing
    {
      get {
        return _nothing;
      }
    }

    /// <summary>
    /// Gets the 'default' object instance (equal to <see cref="ZptConstants.CancellationToken"/>).
    /// </summary>
    /// <value>The default object.</value>
    public object Default
    {
      get {
        return _default;
      }
    }

    /// <summary>
    /// Gets the model being rendered.
    /// </summary>
    /// <value>The model.</value>
    public object Model
    {
      get {
        return _model;
      }
    }

    /// <summary>
    /// Gets the template keyword options.
    /// </summary>
    /// <value>The options.</value>
    public NamedObjectWrapper Options
    {
      get {
        return _options;
      }
    }

    /// <summary>
    /// Gets the repeat variables.
    /// </summary>
    /// <value>The repeat.</value>
    public RepetitionMetadataCollectionWrapper Repeat
    {
      get {
        return _repeat;
      }
    }

    /// <summary>
    /// Gets the original attributes from the current <see cref="ZptElement"/> for which this instance was created.
    /// </summary>
    /// <value>The attributes.</value>
    public OriginalAttributeValuesCollection Attrs
    {
      get {
        return _attrs.Value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a built-in object by name (a TALES path fragment).
    /// </summary>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    /// <param name="currentContext">Gets the current rendering context.</param>
    public virtual bool HandleTalesPath(string pathFragment, out object result, RenderingContext currentContext)
    {
      bool output;

      switch(pathFragment)
      {
      case NOTHING:
        output = true;
        result = this.Nothing;
        break;

      case DEFAULT:
        output = true;
        result = this.Default;
        break;

      case OPTIONS:
        output = true;
        result = this.Options;
        break;

      case REPEAT:
        output = true;
        result = this.Repeat;
        break;

      case ATTRS:
        output = true;
        result = this.Attrs;
        break;

      case TEMPLATE:
        result = _templateFileFactory.CreateTemplateFile(currentContext.Element.OwnerDocument);
        output = (result != null);
        break;

      case CONTAINER:
        var sourceInfo = currentContext.Element.OwnerDocument.GetSourceInfo();
        result = (sourceInfo != null)? sourceInfo.GetContainer() : null;
        output = result != null;
        break;

      case HERE:
        result = this.Model;
        output = (result != null);
        break;

      default:
        output = false;
        result = null;
        break;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.BuiltinContextsContainer"/> class.
    /// </summary>
    /// <param name="options">Options.</param>
    /// <param name="repeat">Repeat.</param>
    /// <param name="attrs">Attrs.</param>
    /// <param name="templateFileFactory">A template-file factory.</param>
    /// <param name="model">The model being rendered.</param>
    public BuiltinContextsContainer(NamedObjectWrapper options,
                                    RepetitionMetadataCollectionWrapper repeat,
                                    Lazy<OriginalAttributeValuesCollection> attrs,
                                    ITemplateFileFactory templateFileFactory,
                                    object model = null)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }
      if(repeat == null)
      {
        throw new ArgumentNullException(nameof(repeat));
      }
      if(attrs == null)
      {
        throw new ArgumentNullException(nameof(attrs));
      }

      _nothing = null;
      _default = ZptConstants.CancellationToken;
      _options = options;
      _repeat = repeat;
      _attrs = attrs;
      _model = model;

        _templateFileFactory = templateFileFactory;

    }

    #endregion
  }
}

