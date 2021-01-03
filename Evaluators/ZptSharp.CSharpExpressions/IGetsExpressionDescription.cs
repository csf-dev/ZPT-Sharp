using System.Collections.Generic;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// An object which gets an expression description from an expression body and
    /// a collection of all of the TALES variables which are in-scope.
    /// </summary>
    public interface IGetsExpressionDescription
    {
        /// <summary>
        /// Gets the expression identity matching the parameters.
        /// </summary>
        /// <param name="expression">The expression body.</param>
        /// <param name="allTalesValues">A collection of all of the TALES variables which are in-scope</param>
        /// <param name="additionalReferences">An optional collection of additional assembly references for the generated expression.</param>
        /// <param name="additionalNamespaces">An optional collection of additional using namespaces for the generated expression.</param>
        /// <returns>An expression description.</returns>
        ExpressionDescription GetDescription(string expression,
                                             IDictionary<string, object> allTalesValues,
                                             IEnumerable<AssemblyReference> additionalReferences = null,
                                             IEnumerable<UsingNamespace> additionalNamespaces = null);
    }
}