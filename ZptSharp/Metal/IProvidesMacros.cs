using System.Collections.Generic;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which can get METAL macros from a document.
    /// </summary>
    public interface IProvidesMacros
    {
        /// <summary>
        /// Gets a collection of all of the macros 
        /// </summary>
        /// <returns>The macros.</returns>
        IDictionary<string, MetalMacro> GetMacros();
    }
}
