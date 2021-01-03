using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsScriptBody" /> which returns a string suitable for
    /// compilation (by the Roslyn scripting API) to a C# script.
    /// </summary>
    public class ScriptBodyFactory : IGetsScriptBody
    {
        const string parametersParamName = "_zptCSharpScriptParameters";

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
        /// <param name="description">The expression description.</param>
        /// <returns>The script body.</returns>
        public string GetScriptBody(ExpressionDescription description)
        {
            return $@"{parametersParamName} => {{
    {GetVariableAssignments(description)}

    return {description.Expression};
}}";
        }

        string GetVariableAssignments(ExpressionDescription description)
        {
            var assignments = description.InScopeVariableNames
                .Where(SyntaxFacts.IsValidIdentifier)
                .Where(x => x != "default")
                .Select(name => GetVariableAssignment(name, description));
            return string.Join("\n    ", assignments);
        }

        string GetVariableAssignment(string variableName, ExpressionDescription description)
        {
            var typeName = GetTypeName(variableName, description);
            if(typeName == null)
            {
                // The variable has no specified type, so we treat it as a dynamic object
                return $"var {variableName} = (dynamic) {parametersParamName}[\"{variableName}\"];";
            }

            // The variable has specified type info, so it's a cast
            return $"{typeName} {variableName} = ({typeName}) {parametersParamName}[\"{variableName}\"];";
        }

        string GetTypeName(string variableName, ExpressionDescription description)
            => description.TypeDesignations.FirstOrDefault(x => x.VariableName == variableName)?.Type;
    }
}