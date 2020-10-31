﻿using System;
using ZptSharp.Config;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which may get an instance of <see cref="IRendersZptDocuments"/> for a
    /// configuration and optionally a specified implementation of <see cref="IReadsAndWritesDocument"/>.
    /// </summary>
    public interface IGetsZptDocumentRenderer
    {
        /// <summary>
        /// Gets the document renderer.
        /// </summary>
        /// <returns>The document renderer.</returns>
        /// <param name="config">Config.</param>
        /// <param name="readerWriter">A specific document reader/writer implementation.</param>
        IRendersZptDocuments GetDocumentRenderer(RenderingConfig config,
                                                 IReadsAndWritesDocument readerWriter = null);
    }
}
