using System.Collections.Generic;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// A singleton object which holds the global configuration for C# expression evaluation.
    /// </summary>
    public class GlobalExpressionConfigStore : IConfiguresCSharpExpressionGlobals
    {
        /// <summary>
        /// Gets a collection of globally-available assembly references which are
        /// added to all C# expressions and which do not need an 'assemblyref' expression
        /// in-scope.
        /// </summary>
        /// <value>The global assembly references.</value>
        public ICollection<AssemblyReference> GlobalAssemblyReferences { get; } = new List<AssemblyReference>();

        /// <summary>
        /// Gets a collection of globally-available using namespaces which are
        /// added to all C# expressions and which do not need an 'using' expression
        /// in-scope.
        /// </summary>
        /// <value>The global using namespaces.</value>
        public ICollection<UsingNamespace> GlobalUsingNamespaces { get; } = new List<UsingNamespace>();
    }
}