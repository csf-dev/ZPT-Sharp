﻿using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which creates an instance of <see cref="IRenderingContext"/> from the
  /// <see cref="IRenderingOptions"/>.
  /// </summary>
  public interface IRenderingContextFactory
  {
    /// <summary>
    /// Create a context instance.
    /// </summary>
    IRenderingContext Create(IZptElement element, IRenderingOptions options);

    /// <summary>
    /// Create a context instance.
    /// </summary>
    IRenderingContext Create(IZptElement element, IRenderingOptions options, object model);

    /// <summary>
    /// Adds a keyword option to contexts created by the current instance.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    void AddKeywordOption(string key, string value);
  }
}

