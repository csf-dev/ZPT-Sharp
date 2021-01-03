using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Implementation of <see cref="ICreatesCSharpExpressions" /> which uses the Roslyn
    /// Scripting API to create instances of <see cref="CSharpExpression" />.
    /// </summary>
    public class ExpressionCompiler : ICreatesCSharpExpressions
    {
        const string parametersParamName = "__zptCSharpScriptParameters__";

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

            var scriptBody = GetScriptBody(description);
            var scriptOptions = GetScriptOptions(description);

            return CSharpScript.EvaluateAsync<CSharpExpression>(scriptBody, scriptOptions, cancellationToken: token);
        }

        /// <summary>
        /// Creates a string which corresponds to a statement lambda that
        /// matches the delegate <see cref="CSharpExpression" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The full returned lambda string would look something like this when returned:
        /// </para>
        /// <code>
        /// __zptCSharpScriptParameters__ => {
        ///     dynamic variable1 = __zptCSharpScriptParameters__["variable1"];
        ///     dynamic variable2 = __zptCSharpScriptParameters__["variable2"];
        /// 
        ///     return variable1 + variable2;
        /// }
        /// </code>
        /// <para>
        /// In that sample, the references <c>variable1</c> &amp; <c>variable2</c> are two sample
        /// variables which are 'in scope' for the expression.  These lines would be replaced by
        /// however many lines of code are required to describe all of the variables which are
        /// defined at the point where the expression is being evaluated.
        /// </para>
        /// <para>
        /// Additionally, the <c>variable1 + variable2</c> part of the lambda is a stand-in for
        /// the actual expression body which is being evaluated.  Note that the final semicolon
        /// is not a part of the expression, nor is it expected to be included by the expression.
        /// </para>
        /// <para>
        /// Finally, if TALES 'type' expressions have been used to create explicit type designations
        /// for variables, then those variables won't be treated as <c>dynamic</c> objects.  Instead
        /// they will be strongly-typed and there will be a cast operator used in their assignment
        /// from the dicionary of input parameters.
        /// </para>
        /// </remarks>
        /// <param name="description">The expression identity.</param>
        /// <returns>The script body.</returns>
        string GetScriptBody(ExpressionDescription description)
        {
            return $@"{parametersParamName} => {{
    {GetVariableAssignments(description)}

    return {description.Expression};
}}";
        }

        string GetVariableAssignments(ExpressionDescription description)
        {
            var assignments = description.InScopeVariableNames
                .Select(name => GetVariableAssignment(name, description));
            return string.Join("\n    ", assignments);
        }

        string GetVariableAssignment(string variableName, ExpressionDescription description)
        {
            var typeName = GetTypeName(variableName, description);
            if(typeName == null)
            {
                // The variable has no specified type, so we treat it as a dynamic object
                return $"dynamic {variableName} = {parametersParamName}[\"{variableName}\"];";
            }

            // The variable has specified type info, so it's a cast
            return $"{typeName} {variableName} = ({typeName}) {parametersParamName}[\"{variableName}\"];";
        }

        string GetTypeName(string variableName, ExpressionDescription description)
            => description.TypeDesignations.FirstOrDefault(x => x.VariableName == variableName)?.Type;

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
                .WithImports(description.UsingNamespaces.Select(x => x.Namespace).ToList())
                .AddReferences(description.AssemblyReferences.Select(x => x.Name).ToList())
                .AddReferences(GetType().Assembly)
                .AddReferences("Microsoft.CSharp");
        }
    }
}