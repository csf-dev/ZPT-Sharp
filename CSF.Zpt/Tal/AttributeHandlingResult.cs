using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Represents the response from <see cref="IAttributeHandler.Handle(ZptElement,Model)"/>.
  /// </summary>
  public class AttributeHandlingResult
  {
    #region properties

    /// <summary>
    /// Gets a collection of the elements which are exposed after a handling operation has completed.
    /// </summary>
    /// <value>The elements.</value>
    public RenderingContext[] Contexts
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Elements"/> should be handled by further TAL attribute-handlers,
    /// as if they were the same as the source element.
    /// </summary>
    /// <value><c>true</c> if handling should continue; otherwise, <c>false</c>.</value>
    public bool ContinueHandling
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a collection of elements which are newly-exposed after a handling operation has completed.  These
    /// elements must be processed from scratch and not included in further processing.
    /// </summary>
    /// <value>The newly-exposed elements.</value>
    public RenderingContext[] NewlyExposedContexts
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tal.AttributeHandlingResult"/> class.
    /// </summary>
    /// <param name="elements">The elements which remain after processing.</param>
    /// <param name="continueHandling">If set to <c>true</c> continue handling.</param>
    /// <param name="newlyExposedElements">An optional collection of elements which are newly-exposed but must be processed from scratch.</param>
    public AttributeHandlingResult(RenderingContext[] elements,
                                   bool continueHandling,
                                   RenderingContext[] newlyExposedElements = null)
    {
      if(elements == null)
      {
        throw new ArgumentNullException(nameof(elements));
      }

      this.Contexts = elements;
      this.ContinueHandling = continueHandling;
      this.NewlyExposedContexts = newlyExposedElements?? new RenderingContext[0];
    }

    #endregion
  }
}

