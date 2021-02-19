using System;
using System.IO;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which can get an XHTML DTD based on an absolute Uri.
    /// </summary>
    public interface IGetsXhtmlDtds
    {
        /// <summary>
        /// Gets a stream containing the XHTML DTD, for the specified Uri.
        /// </summary>
        /// <returns>The XHTML DTD stream.</returns>
        /// <param name="uri">URI.</param>
        Stream GetXhtmlDtdStream(Uri uri);
    }
}
