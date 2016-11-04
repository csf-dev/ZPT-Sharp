﻿using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Interface for a type which provides batch-rendering services, parsing and rendering potentially multiple documents
  /// in a single job.
  /// </summary>
  public interface IBatchRenderer
  {
    /// <summary>
    /// Parse and render the documents found using the given batch rendering options.
    /// </summary>
    /// <param name="settings">Rendering settings.</param>
    /// <param name="batchOptions">Batch rendering options, indicating the source and destination files.</param>
    /// <returns>
    /// An object instance indicating the outcome of the rendering.
    /// </returns>
    IBatchRenderingResponse Render(IBatchRenderingOptions batchOptions,
                                   IRenderingSettings settings);
    
    /// <summary>
    /// Parse and render the documents found using the given batch rendering options.
    /// </summary>
    /// <param name="options">Rendering options.</param>
    /// <param name="batchOptions">Batch rendering options, indicating the source and destination files.</param>
    /// <returns>
    /// An object instance indicating the outcome of the rendering.
    /// </returns>
    IBatchRenderingResponse Render(IBatchRenderingOptions batchOptions,
                                   IRenderingOptions options = null);
  }
}
