﻿using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.SourceAnnotation
{
  /// <summary>
  /// Performs source annotation tasks upon METAL elements.
  /// </summary>
  public interface ISourceAnnotator
  {
    /// <summary>
    /// Processes source annotation and adds comments before/after the <paramref name="targetContext"/> if appropriate.
    /// </summary>
    /// <param name="targetContext">Target context.</param>
    void WriteAnnotationIfAppropriate(IRenderingContext targetContext);
  }
}
