using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    /// <summary>
    /// An object which evaluates an expression and returns a value which may be used by the DOM.
    /// </summary>
    public interface IEvaluatesDomValueExpression
    {
        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method also deals with the differentiation between <c>structure</c> and <c>text</c>
        /// expressions.  Structure expressions are inserted directly into the DOM without escaping or encoding.
        /// Text expressions are treated explicitly as text and are encoded/escaped to ensure that they are not
        /// accidentally treated as markup.
        /// </para>
        /// <para>
        /// Expressions are treated as text by default.  An expression is only treated as structure if:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>It is prefixed by a keyword <c>structure</c> and a single space, for example <c>structure myExpression</c></description>
        /// </item>
        /// <item>
        /// <description>The expression result implements <see cref="IGetsStructuredMarkup"/> and the expression is NOT prefixed by
        /// the keyword <c>text</c> and a single space, for example <c>text myExpression</c></description>
        /// </item>
        /// </list>
        /// <para>
        /// Apart from aborting the treatment of <see cref="IGetsStructuredMarkup"/> as structure, the <c>text</c> prefix
        /// keyword for expressions is redundant, as it is the default.  It is supported though, for situations where you wish
        /// to be explicit.
        /// </para>
        /// </remarks>
        /// <returns>The expression result.</returns>
        /// <param name="expression">An expression which might be prefixed to indicate that it is to be treated as structure.</param>
        /// <param name="context">Context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<DomValueExpressionResult> EvaluateExpressionAsync(string expression,
                                                               ExpressionContext context,
                                                               CancellationToken cancellationToken = default);
    }
}
