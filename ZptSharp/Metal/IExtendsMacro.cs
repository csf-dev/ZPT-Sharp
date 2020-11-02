using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which applies macro extension, where applicable.
    /// </summary>
    public interface IExtendsMacro
    {
        /// <summary>
        /// Extends the specified macro using the specified expression context.  Returns the macro, after
        /// all applicable extension has been performed.
        /// </summary>
        /// <returns>The fully-extended macro.</returns>
        /// <param name="macro">The macro to extend.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="slotFillers">A collection of slot-fillers which are already defined at a parent level.</param>
        /// <param name="token">An optional cancellation token.</param>
        Task<MetalMacro> ExtendMacroAsync(MetalMacro macro, ExpressionContext context, IDictionary<string,SlotFiller> slotFillers, CancellationToken token = default);
    }
}
