using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which gets a METAL macro for an node which has a matching METAL attribute.
    /// </summary>
    public interface IGetsMacro
    {
        /// <summary>
        /// Gets the METAL macro referenced by the specified node's attribute, if such an attribute is present.
        /// </summary>
        /// <returns>The METAL macro, or a null reference if the <paramref name="node"/>
        /// has no attribute matching the <paramref name="attributeSpec"/>.</returns>
        /// <param name="node">The node from which to get the macro.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="attributeSpec">An attribute spec.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <exception cref="MacroNotFoundException">If the node does have an attribute matching
        /// the <paramref name="attributeSpec"/> but no macro could be resolved from the attribute's expression.</exception>
        Task<MetalMacro> GetMacroAsync(INode node,
                                       ExpressionContext context,
                                       AttributeSpec attributeSpec,
                                       CancellationToken token = default);
        /// <summary>
        /// Gets the METAL macro referenced by the specified node's attribute, if such an attribute is present.
        /// </summary>
        /// <returns>The METAL macro, or a null reference if the <paramref name="node"/>
        /// has no attribute matching any of the <paramref name="attributeSpecs"/>.</returns>
        /// <param name="node">The node from which to get the macro.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="attributeSpecs">A collection of attribute specs.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <exception cref="MacroNotFoundException">If the node does have an attribute matching
        /// the <paramref name="attributeSpecs"/> but no macro could be resolved from the attribute's expression.</exception>
        Task<MetalMacro> GetMacroAsync(INode node,
                                       ExpressionContext context,
                                       IEnumerable<AttributeSpec> attributeSpecs,
                                       CancellationToken token = default);

    }
}
