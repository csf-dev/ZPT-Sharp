using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Represents a METAL slot definition, matched with the content which will be used to fill it.
  /// </summary>
  public class SlotToFill
  {
    #region properties

    /// <summary>
    /// Gets the rendering context for the slot definition.
    /// </summary>
    /// <value>The slot.</value>
    public IRenderingContext Slot
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the rendering context for the slot filler.
    /// </summary>
    /// <value>The filler.</value>
    public IRenderingContext Filler
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the name of the slot
    /// </summary>
    /// <value>The name.</value>
    public string Name
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.SlotToFill"/> class.
    /// </summary>
    /// <param name="slot">The slot definition.</param>
    /// <param name="filler">The slow filler.</param>
    /// <param name="name">The slot name.</param>
    public SlotToFill(IRenderingContext slot, IRenderingContext filler, string name)
    {
      if(slot == null)
      {
        throw new ArgumentNullException(nameof(slot));
      }
      if(filler == null)
      {
        throw new ArgumentNullException(nameof(filler));
      }
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      Slot = slot;
      Filler = filler;
      Name = name;
    }

    #endregion
  }
}

