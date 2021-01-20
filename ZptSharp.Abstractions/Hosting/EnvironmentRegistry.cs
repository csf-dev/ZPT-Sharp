using System;
using System.Collections.Generic;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// A singleton (per DI environment) registry which is used to indicate which types of
    /// expression evaluator and document provider are to be enabled.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type is set-up by the dependency injection registration extension methods, with items added
    /// as each component of ZptSharp is enabled.  Specifically this class is used to track which
    /// document providers (see <see cref="Dom.IReadsAndWritesDocument"/>) and expression evaluators
    /// (see <see cref="Expressions.IEvaluatesExpression"/>) are enabled for use in the current environment.
    /// </para>
    /// <para>
    /// This registry is required because those two types of functionality are extensible and semi-plugin-based.
    /// Whilst they are not fully loaded at runtime (true plugins), they are selected optionally during
    /// dependency injection configuration.  This means that runtime services need this list of which are
    /// available.
    /// </para>
    /// </remarks>
    public class EnvironmentRegistry
    {
        /// <summary>
        /// Gets a collection of the expression evaluator types
        /// which are enabled for use in the ZptSharp environment.
        /// </summary>
        public IDictionary<string,Type> ExpresionEvaluatorTypes { get; } = new Dictionary<string,Type>();

        /// <summary>
        /// Gets a collection of the document provider types
        /// which are enabled for use in the ZptSharp environment.
        /// </summary>
        public ISet<Type> DocumentProviderTypes { get; } = new HashSet<Type>();
    }
}