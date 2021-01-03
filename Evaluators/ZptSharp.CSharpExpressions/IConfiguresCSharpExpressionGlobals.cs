using System.Collections.Generic;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// An object which may be used to configure the way in which C# expressions work.
    /// This includes adding global assembly references and/or 'using' namespaces.
    /// </summary>
    public interface IConfiguresCSharpExpressionGlobals
    {
        /// <summary>
        /// Gets a collection of globally-available assembly references which are
        /// added to all C# expressions and which do not need an 'assemblyref' expression
        /// in-scope.
        /// </summary>
        /// <value>The global assembly references.</value>
        ICollection<AssemblyReference> GlobalAssemblyReferences { get; }

        /// <summary>
        /// Gets a collection of globally-available namespaces which are
        /// added to all C# expressions and which do not need an 'using' expression
        /// in-scope.
        /// </summary>
        /// <value>The global using namespaces.</value>
        ICollection<UsingNamespace> GlobalUsingNamespaces { get; }
    }
}