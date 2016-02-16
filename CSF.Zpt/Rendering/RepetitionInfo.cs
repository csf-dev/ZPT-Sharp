using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Gets information about a repetition/iteration.
  /// </summary>
  public class RepetitionInfo
  {
    #region properties

    /// <summary>
    /// Gets a reference to the associated <see cref="ZptElement"/>, if applicable.
    /// </summary>
    /// <value>The associated element.</value>
    public ZptElement AssociatedElement
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the name of the repetition.
    /// </summary>
    /// <value>The name.</value>
    public string Name
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the count of items in the repetition.
    /// </summary>
    /// <value>The count.</value>
    public int Count
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the index of the current iteration.
    /// </summary>
    /// <value>The index.</value>
    public int Index
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the value associated with the current iteration.
    /// </summary>
    /// <value>The value.</value>
    public object Value
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Creates and returns a <see cref="RepetitionSummary"/> from the current instance.
    /// </summary>
    /// <returns>The summary.</returns>
    public RepetitionSummary ToSummary()
    {
      return new RepetitionSummary(this.Index, this.Count);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RepetitionInfo"/> class.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="index">Index.</param>
    /// <param name="count">Count.</param>
    /// <param name="value">Value.</param>
    /// <param name="element">Element.</param>
    public RepetitionInfo(string name, int index, int count, object value, ZptElement element)
    {
      if(name == null)
      {
        throw new ArgumentNullException("name");
      }

      this.Name = name;
      this.Index = index;
      this.Count = count;
      this.Value = value;
      this.AssociatedElement = element;
    }

    #endregion
  }
}

