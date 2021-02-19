using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsExpressionDescription" /> which creates expression description objects.
    /// </summary>
    public class ExpressionDescriptionFactory : IGetsExpressionDescription
    {
        /// <summary>
        /// Gets the expression description matching the parameters.
        /// </summary>
        /// <param name="expression">The expression body.</param>
        /// <param name="allTalesValues">A collection of all of the TALES variables which are in-scope</param>
        /// <param name="additionalReferences">An optional collection of additional assembly references for the generated expression.</param>
        /// <param name="additionalNamespaces">An optional collection of additional using namespaces for the generated expression.</param>
        /// <returns>An expression description.</returns>
        public ExpressionDescription GetDescription(string expression,
                                                    IDictionary<string, object> allTalesValues,
                                                    IEnumerable<AssemblyReference> additionalReferences = null,
                                                    IEnumerable<UsingNamespace> additionalNamespaces = null)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));
            if (allTalesValues is null)
                throw new System.ArgumentNullException(nameof(allTalesValues));

            var assemblyReferences = allTalesValues.Values.OfType<AssemblyReference>()
                .Union(additionalReferences ?? Enumerable.Empty<AssemblyReference>())
                .ToList();
            var usingNamespaces = allTalesValues.Values.OfType<UsingNamespace>()
                .Union(additionalNamespaces ?? Enumerable.Empty<UsingNamespace>())
                .ToList();

            var variableTypes = allTalesValues.Values.OfType<VariableType>().ToList();

            return new ExpressionDescription(expression, assemblyReferences, usingNamespaces, allTalesValues.Keys, variableTypes);
        }
    }
}