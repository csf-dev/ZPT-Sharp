using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which gets a METAL macro for an element which has a matching METAL attribute.
    /// </summary>
    public interface IGetsMacro
    {
        /// <summary>
        /// Gets the METAL macro referenced by the specified element's attribute, if such an attribute is present.
        /// </summary>
        /// <returns>The METAL macro, or a null reference if the <paramref name="element"/>
        /// has no attribute matching the <paramref name="attributeSpec"/>.</returns>
        /// <param name="element">The element from which to get the macro.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="attributeSpec">An attribute spec.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <exception cref="MacroNotFoundException">If the element does have an attribute matching
        /// the <paramref name="attributeSpec"/> but no macro could be resolved from the attribute's expression.</exception>
        Task<MetalMacro> GetMacroAsync(IElement element,
                                       ExpressionContext context,
                                       AttributeSpec attributeSpec,
                                       CancellationToken token = default);

    }
}
