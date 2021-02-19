using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which evaluates and returns the result of a TALES expression.
    /// This is the core interface for an "expression evaluator".
    /// </summary>
    /// <remarks>
    /// <para>
    /// TALES - Template Attribute Language Expression Syntax - is an extensible language.
    /// Expressions may be prefixed with a type designator such as <c>expression_type:expression_body</c>,
    /// where <c>expression_type</c> indicates which expression evaluator should be used to handle the
    /// expression body.
    /// </para>
    /// <para>
    /// ZptSharp uses a registry of expression evaluators (and their type prefixes) such that prefixed
    /// expressions are automatically routed to their correct evaluator implementation.
    /// Additionally, the core logic ensures that the prefix is stripped from the expression before
    /// the evaluator receives it.
    /// </para>
    /// </remarks>
    /// <seealso cref="IRegistersExpressionEvaluator"/>
    /// <seealso cref="IRemovesPrefixFromExpression"/>
    public interface IEvaluatesExpression
    {
        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method must decisively evaluate the expression, if an expression cannot be evaluated
        /// to a conclusive result then this method must throw <see cref="EvaluationException"/> or
        /// an exception derived from an evaluation exception.
        /// It is acceptable to return <see langword="null"/> from this method though, if that is the
        /// result of evaluating the expression.
        /// </para>
        /// <para>
        /// Whilst the API of this method is asynchronous (returning a task), it will not be suitable
        /// or appropriate for all implementations to act asynchronously.
        /// Thus, implementors are not certain to honour the <paramref name="cancellationToken"/> if
        /// it is used to cancel/abort evalaution early.
        /// Implementors are encouraged to act asynchronously when it is appropriate to do so, but may
        /// choose to act synchronously and return an already-completed task.
        /// </para>
        /// <para>
        /// Note that any expression result might also be an <see cref="AbortZptActionToken"/>,
        /// which is a special value indicating that the current ZPT operation should be aborted.
        /// </para>
        /// </remarks>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <exception cref="EvaluationException">Thrown if evaluating the expression fails.</exception>
        /// <exception cref="System.AggregateException">Thrown if evaluating the expression fails asynchronously.</exception>
        /// <seealso cref="EvaluationException"/>
        /// <seealso cref="AbortZptActionToken"/>
        /// <seealso cref="ExpressionContext"/>
        Task<object> EvaluateExpressionAsync(string expression,
                                             ExpressionContext context,
                                             CancellationToken cancellationToken = default);
    }
}
