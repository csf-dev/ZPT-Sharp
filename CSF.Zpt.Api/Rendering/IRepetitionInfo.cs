using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  public interface IRepetitionInfo : ITalesPathHandler
  {
    ZptElement AssociatedElement { get; }

    object Value { get; }

    string Name { get; }
  }
}

