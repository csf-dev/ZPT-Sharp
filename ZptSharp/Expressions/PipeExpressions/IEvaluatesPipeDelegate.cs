namespace ZptSharp.Expressions.PipeExpressions
{
    /// <summary>
    /// An object which evaluates a 'pipe' expression delegate and gets the result.
    /// </summary>
    public interface IEvaluatesPipeDelegate
    {
        /// <summary>
        /// Evaluates the result of the pipe expression delegate and returns the result.
        /// </summary>
        /// <param name="source">The source object which is to be transformed by the delegate.</param>
        /// <param name="pipeDelegate">The delegate function which forms the pipe operation.</param>
        /// <returns>The transformed result of executing the pipe delegate with the source object as a parameter.</returns>
        object EvaluateDelegate(object source, object pipeDelegate);
    }
}