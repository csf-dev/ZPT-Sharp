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
    public ZptElement[] Elements
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

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tal.AttributeHandlingResult"/> class.
    /// </summary>
    /// <param name="elements">Elements.</param>
    /// <param name="continueHandling">If set to <c>true</c> continue handling.</param>
    public AttributeHandlingResult(ZptElement[] elements, bool continueHandling)
    {
      if(elements == null)
      {
        throw new ArgumentNullException("elements");
      }

      this.Elements = elements;
      this.ContinueHandling = continueHandling;
    }

    #endregion
  }
}

