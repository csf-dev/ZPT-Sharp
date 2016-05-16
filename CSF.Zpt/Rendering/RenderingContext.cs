using System;
using CSF.Zpt.Rendering;
using System.Linq;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents the object model, presented to a <see cref="ZptDocument"/> when it is rendered.
  /// </summary>
  public class RenderingContext
  {
    #region fields

    private Model _metalContext, _talContext;
    private ZptElement _element;
    private RenderingOptions _renderingOptions;
    private IEnumerable<ZptAttribute> _originalAttributes;

    #endregion

    #region properties

    /// <summary>
    /// Gets the object model available to the METAL environment.
    /// </summary>
    /// <value>The METAL model.</value>
    public virtual Model MetalModel
    {
      get {
        return _metalContext;
      }
    }

    /// <summary>
    /// Gets the object model available to the TAL environment.
    /// </summary>
    /// <value>The TAL model.</value>
    public virtual Model TalModel
    {
      get {
        return _talContext;
      }
    }

    /// <summary>
    /// Gets the ZPT element.
    /// </summary>
    /// <value>The element.</value>
    public virtual ZptElement Element
    {
      get {
        return _element;
      }
    }

    /// <summary>
    /// Gets the rendering options.
    /// </summary>
    /// <value>The rendering options.</value>
    public virtual RenderingOptions RenderingOptions
    {
      get {
        return _renderingOptions;
      }
    }

    /// <summary>
    /// Gets the original attributes for the <see cref="Element"/> contained within the current instance.
    /// </summary>
    /// <value>The original attributes.</value>
    protected virtual IEnumerable<ZptAttribute> OriginalAttributes
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
                                          this.RenderingOptions))
        .ToArray();
    }

    /// <summary>
    /// Creates and returns a new sibling rendering context.
    /// </summary>
    /// <param name="element">The ZPT element for which the new context is to be created.</param>
    /// <returns>The sibling context.</returns>
    public virtual RenderingContext CreateSiblingContext(ZptElement element, bool cloneAttributes = false)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      var output = new RenderingContext(this.MetalModel.CreateSiblingModel(),
                                        this.TalModel.CreateSiblingModel(),
                                        element,
                                        this.RenderingOptions);

      if(cloneAttributes)
      {
        output.OriginalAttributes = this.OriginalAttributes;
      }

      return output;
    }

    public virtual ZptAttribute GetAttribute(ZptNamespace nspace, string attributeName)
    {
      var attrNamespace = this.Element.IsInNamespace(nspace)? ZptNamespace.Default : nspace;

      return this.OriginalAttributes.Where(x => x.IsMatch(attrNamespace, attributeName)).FirstOrDefault();
    }

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
    public RenderingContext(Model metalContext,
                            Model talContext,
                            ZptElement element,
                            RenderingOptions options)
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

      _originalAttributes = _element.GetAttributes();
    }

    #endregion
  }
}

