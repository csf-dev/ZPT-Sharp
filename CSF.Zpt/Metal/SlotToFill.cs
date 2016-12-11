using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  public class SlotToFill
  {
    #region properties

    public IRenderingContext Slot
    {
      get;
      private set;
    }

    public IRenderingContext Filler
    {
      get;
      private set;
    }

    public string Name
    {
      get;
      private set;
    }

    #endregion

    #region constructor

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

