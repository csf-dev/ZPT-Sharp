using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// A specialisation of <see cref="IGetsNamedTalesValue"/> which can provide
    /// a dictionary of many named TALES values.
    /// </summary>
    public interface IGetsDictionaryOfNamedTalesValues : IGetsNamedTalesValue
    {
        /// <summary>
        /// Gets a dictionary of every available named TALES value, exposed by the current instance.
        /// </summary>
        /// <returns>The named values.</returns>
        Task<IDictionary<string, object>> GetAllNamedValues();
    }
}
