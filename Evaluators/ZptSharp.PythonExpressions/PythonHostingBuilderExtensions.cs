using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.PythonExpressions;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods related to the use of ZPT Python expressions.
    /// </summary>
    public static class PythonHostingBuilderExtensions
    {
        /// <summary>
        /// Adds service registrations to the <paramref name="builder"/> in order
        /// to enable the use of ZPT "python" expressions.
        /// </summary>
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        /// <param name="builder">The self-hosting builder.</param>
        public static IBuildsHostingEnvironment AddZptPythonExpressions(this IBuildsHostingEnvironment builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ServiceCollection.AddSingleton(typeof(IGetsScriptEngine), s => new IronPythonScriptEngineContainer());
            builder.ServiceCollection.AddTransient<IEvaluatesPythonExpression, ScriptEngineEvaluator>();
            builder.ServiceCollection.AddTransient<PythonExpressionEvaluator>();
            builder.ServiceCollection.AddTransient<IGetsClassDefinitionScript, ClassDefinitionScriptFactory>();

            builder.ServiceRegistry.ExpresionEvaluatorTypes.Add(PythonExpressionEvaluator.ExpressionPrefix, typeof(PythonExpressionEvaluator));

            return builder;
        }
    }
}
