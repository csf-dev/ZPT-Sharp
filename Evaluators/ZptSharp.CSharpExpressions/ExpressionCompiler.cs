using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Logging;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Implementation of <see cref="ICreatesCSharpExpressions" /> which uses the Roslyn
    /// Scripting API to create instances of <see cref="CSharpExpression" />.
    /// </summary>
    public class ExpressionCompiler : ICreatesCSharpExpressions
    {
        readonly ILogger logger;
        readonly IGetsScriptBody scriptBodyFactory;

        /// <summary>
        /// Gets the compiled expression.
        /// </summary>
        /// <param name="description">The expression identifier.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>The compiled expression.</returns>
        public Task<CSharpExpression> GetExpressionAsync(ExpressionDescription description, CancellationToken token = default)
        {
            if (description is null)
                throw new System.ArgumentNullException(nameof(description));

            var scriptBody = scriptBodyFactory.GetScriptBody(description);
            var scriptOptions = GetScriptOptions(description);

            if(logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(@"Attempting to compile the following C# script:
{scriptBody}",
                                scriptBody);
            }

            return CSharpScript.EvaluateAsync<CSharpExpression>(scriptBody, scriptOptions, cancellationToken: token);
        }

        /// <summary>
        /// Gets a <see cref="ScriptOptions" /> object which describes the
        /// <c>using</c> namespaces and assembly references which are made available
        /// to the executing script.
        /// </summary>
        /// <param name="description">The expression identity.</param>
        /// <returns>The script options.</returns>
        ScriptOptions GetScriptOptions(ExpressionDescription description)
        {
            return ScriptOptions.Default
                .AddImports("System")
                .AddImports(description.UsingNamespaces.Select(x => x.Namespace).ToList())
                .AddReferences(description.AssemblyReferences.Select(x => x.Name).ToList())
                .AddReferences(GetType().Assembly)
                .AddReferences("Microsoft.CSharp");
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ExpressionCompiler" />.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <param name="scriptBodyFactory">The script body factory.</param>
        public ExpressionCompiler(ILogger<ExpressionCompiler> logger,
                                  IGetsScriptBody scriptBodyFactory)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.scriptBodyFactory = scriptBodyFactory ?? throw new System.ArgumentNullException(nameof(scriptBodyFactory));
        }
    }
}