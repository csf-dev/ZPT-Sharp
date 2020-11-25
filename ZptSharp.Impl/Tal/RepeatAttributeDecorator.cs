using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    public class RepeatAttributeDecorator : IProcessesExpressionContext
    {
        readonly IProcessesExpressionContext wrapped;

        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public RepeatAttributeDecorator(IProcessesExpressionContext wrapped, IGetsTalAttributeSpecs specProvider)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
