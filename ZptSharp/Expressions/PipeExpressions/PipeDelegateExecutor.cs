using System;
using System.Linq;

namespace ZptSharp.Expressions.PipeExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesPipeDelegate"/> which executes the pipe delegate and
    /// gets the result.
    /// </summary>
    public class PipeDelegateExecutor : IEvaluatesPipeDelegate
    {
        /// <summary>
        /// Evaluates the result of the pipe expression delegate and returns the result.
        /// </summary>
        /// <param name="source">The source object which is to be transformed by the delegate.</param>
        /// <param name="pipeDelegate">The delegate function which forms the pipe operation.</param>
        /// <returns>The transformed result of executing the pipe delegate with the source object as a parameter.</returns>
        public object EvaluateDelegate(object source, object pipeDelegate)
            => GetDelegate(pipeDelegate)(source);

        static Func<object,object> GetDelegate(object pipeDelegate)
        {
            if (pipeDelegate is null)
                throw new System.ArgumentNullException(nameof(pipeDelegate));
            if (!(pipeDelegate is Delegate dele))
            {
                var message = String.Format(Resources.ExceptionMessage.PipeExpressionDelegateObjectMustBeADelegate,
                                            nameof(Delegate),
                                            pipeDelegate.GetType().FullName);
                throw new ArgumentException(message, nameof(pipeDelegate));
            }

            var parameters = dele.Method.GetParameters();
            if (parameters.Length != 1 || dele.Method.ReturnType == typeof(void))
            {
                var message = String.Format(Resources.ExceptionMessage.PipeExpressionDelegateMustTakeOneParameterAndReturnAnObject,
                                            String.Join(", ", parameters.Select(x => x.ParameterType.Name)),
                                            dele.Method.ReturnType.Name);
                throw new ArgumentException(message, nameof(pipeDelegate));
            }

            return obj => dele.DynamicInvoke(obj);
        }
    }
}