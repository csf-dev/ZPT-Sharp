using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Rendering
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

    #endregion

    #region methods

    /// <summary>
    /// Creates and returns a new child rendering context.
    /// </summary>
    /// <returns>The child context.</returns>
    public virtual RenderingContext CreateChildContext()
    {
      return new RenderingContext(this.MetalModel.CreateChildModel(), this.TalModel.CreateChildModel());
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RenderingContext"/> class.
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

