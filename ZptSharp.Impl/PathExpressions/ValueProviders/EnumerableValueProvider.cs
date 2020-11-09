using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions.ValueProviders
{
    public class EnumerableValueProvider : IGetsValueFromObject
    {
        readonly IGetsValueFromObject wrapped;

        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            if (!(@object is IEnumerable<object> enumerable) || !Int32.TryParse(name, out var index))
                return wrapped.TryGetValueAsync(name, @object, cancellationToken);

            // Pointless trying anything further if the index is less than zero
            if (index < 0) return Task.FromResult(GetValueResult.Failure);

            // An optimisation in case the enumerable implements IList
            if(enumerable is System.Collections.IList list)
            {
                return Task.FromResult((index < list.Count) ? GetValueResult.For(list[index]) : GetValueResult.Failure);
            }

            var result = enumerable.Skip(index).Take(1);
            return Task.FromResult(result.Any() ? GetValueResult.For(result.Single()) : GetValueResult.Failure);
        }

        public EnumerableValueProvider(IGetsValueFromObject wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
