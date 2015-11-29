using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt
{
  /// <summary>
  /// Represents the object model, presented to a <see cref="ZptDocument"/> when it is rendered.
  /// </summary>
  public class RenderingContext
  {
    #region fields

    private Model _metalContext, _talContext;

    #endregion

    #region properties

    /// <summary>
    /// Gets the rendering context available to the METAL environment.
    /// </summary>
    /// <value>The METAL context.</value>
    public virtual Model MetalContext
    {
      get {
        return _metalContext;
      }
    }

    /// <summary>
    /// Gets the rendering context available to the TAL environment.
    /// </summary>
    /// <value>The TAL context.</value>
    public virtual Model TalContext
    {
      get {
        return _talContext;
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.RenderingContext"/> class.
    /// </summary>
    public RenderingContext() : this(new Model(), new Model()) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.RenderingContext"/> class.
    /// </summary>
    /// <param name="metalContext">The METAL context.</param>
    /// <param name="talContext">The TAL context.</param>
    public RenderingContext(Model metalContext, Model talContext)
    {
      if(metalContext == null)
      {
        throw new ArgumentNullException("metalContext");
      }
      if(talContext == null)
      {
        throw new ArgumentNullException("talContext");
      }

      _metalContext = metalContext;
      _talContext = talContext;
    }

    #endregion
  }
}

