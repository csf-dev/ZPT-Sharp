using System;
using CSF.Zpt.Rendering;
using System.Linq;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents the object model, presented to a <see cref="IZptDocument"/> when it is rendered.
  /// </summary>
  public class RenderingContext
  {
    #region fields

    private IModel _metalContext, _talContext;
    private IZptElement _element;
    private IRenderingOptions _renderingOptions;
    private string _sourceAnnotationRoot;
    private IEnumerable<IZptAttribute> _originalAttributes;

    #endregion

    #region properties

    /// <summary>
    /// Gets the object model available to the METAL environment.
    /// </summary>
    /// <value>The METAL model.</value>
    public virtual IModel MetalModel
    {
      get {
        return _metalContext;
      }
    }

    /// <summary>
    /// Gets the object model available to the TAL environment.
    /// </summary>
    /// <value>The TAL model.</value>
    public virtual IModel TalModel
    {
      get {
        return _talContext;
      }
    }

    /// <summary>
    /// Gets the ZPT element.
    /// </summary>
    /// <value>The element.</value>
    public virtual IZptElement Element
    {
      get {
        return _element;
      }
    }

    /// <summary>
    /// Gets the rendering options.
    /// </summary>
    /// <value>The rendering options.</value>
    public virtual IRenderingOptions RenderingOptions
    {
      get {
        return _renderingOptions;
      }
    }

    /// <summary>
    /// Gets the 'virtual' root path for the purpose of source annotation comments.
    /// </summary>
    /// <value>The source annotation root path.</value>
    public virtual string SourceAnnotationRootPath
    {
      get {
        return _sourceAnnotationRoot;
      }
    }

    /// <summary>
    /// Gets the original attributes for the <see cref="Element"/> contained within the current instance.
    /// </summary>
    /// <value>The original attributes.</value>
    protected virtual IEnumerable<IZptAttribute> OriginalAttributes
    {
      get {
        return _originalAttributes;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        _originalAttributes = value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Creates and returns a collection of child contexts, from the current instance.
    /// </summary>
    /// <returns>The child contexts.</returns>
    public virtual RenderingContext[] GetChildContexts()
    {
      return this.Element
        .GetChildElements()
        .Select(x => new RenderingContext(this.MetalModel.CreateChildModel(),
                                          this.TalModel.CreateChildModel(),
                                          x,
                                          this.RenderingOptions,
                                          this.SourceAnnotationRootPath))
        .ToArray();
    }

    /// <summary>
    /// Creates and returns a new sibling rendering context.
    /// </summary>
    /// <returns>The sibling context.</returns>
    /// <param name="element">The ZPT element for which the new context is to be created.</param>
    /// <param name="cloneAttributes">A value indicating whether or not the element's attributes should be cloned or not.</param>
    public virtual RenderingContext CreateSiblingContext(IZptElement element, bool cloneAttributes = false)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      var output = new RenderingContext(this.MetalModel.CreateSiblingModel(),
                                        this.TalModel.CreateSiblingModel(),
                                        element,
                                        this.RenderingOptions,
                                        this.SourceAnnotationRootPath);

      if(cloneAttributes)
      {
        output.OriginalAttributes = this.OriginalAttributes;
      }

      return output;
    }

    /// <summary>
    /// Gets an attribute matching the given namespace and attribute name.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference if no attribute is found.</returns>
    /// <param name="nspace">The attribute namespace.</param>
    /// <param name="attributeName">The attribute name.</param>
    public virtual IZptAttribute GetAttribute(ZptNamespace nspace, string attributeName)
    {
      return this.OriginalAttributes
        .Where(x => x.IsMatch(nspace, attributeName)
               || (this.Element.IsInNamespace(nspace) && x.IsMatch(ZptNamespace.Default, attributeName)))
        .FirstOrDefault();
    }

    /// <summary>
    /// Gets the original attributes present upon the element wrapped by the current instance.
    /// </summary>
    /// <returns>The original attributes.</returns>
    public virtual OriginalAttributeValuesCollection GetOriginalAttributes()
    {
      return new OriginalAttributeValuesCollection(this.OriginalAttributes);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Parameterless constructor for testing use only.
    /// </summary>
    protected RenderingContext() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RenderingContext"/> class.
    /// </summary>
    /// <param name="metalContext">The METAL context.</param>
    /// <param name="talContext">The TAL context.</param>
    /// <param name="element">The ZPT element for which this context is created.</param>
    /// <param name="options">The rendering options.</param>
    /// <param name="sourceAnnotationRoot">The source annotation root path.</param>
    public RenderingContext(IModel metalContext,
                            IModel talContext,
                            IZptElement element,
                            IRenderingOptions options,
                            string sourceAnnotationRoot = null)
    {
      if(metalContext == null)
      {
        throw new ArgumentNullException(nameof(metalContext));
      }
      if(talContext == null)
      {
        throw new ArgumentNullException(nameof(talContext));
      }
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      _metalContext = metalContext;
      _talContext = talContext;
      _element = element;
      _renderingOptions = options;
      _sourceAnnotationRoot = sourceAnnotationRoot;

      _originalAttributes = _element.GetAttributes();
    }

    #endregion
  }
}

