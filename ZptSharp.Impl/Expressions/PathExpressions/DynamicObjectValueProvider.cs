using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// A chain of responsibility class which get values from object that implement
    /// <see cref="System.Dynamic.IDynamicMetaObjectProvider"/> (IE: which are "dynamic objects").
    /// If the object does not implement that interface then the call is delegated straight
    /// to the wrapped implementation.
    /// </summary>
    public class DynamicObjectValueProvider : IGetsValueFromObject
    {
        readonly IGetsValueFromObject wrapped;

        /// <summary>
        /// Attempts to get a value for a named reference, from the specified object.
        /// If the <paramref name="object"/> is a dynamic object then an attempt will be made
        /// to get a value based upon the <paramref name="name"/>.  If the object
        /// is not a dynamic object then the call will be delegated to the wrapped provider.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            if (!(@object is System.Dynamic.IDynamicMetaObjectProvider))
                return wrapped.TryGetValueAsync(name, @object, cancellationToken);

            try
            {
                return Task.FromResult(GetValueResult.For(GetDynamicMember(@object, name)));
            }
            catch(Exception)
            {
                return Task.FromResult(GetValueResult.Failure);
            }
        }

        /// <summary>
        /// <para>
        /// Either gets a value from <paramref name="obj"/> using the specified
        /// <paramref name="memberName"/>, or raises an exception.
        /// </para>
        /// <para>
        /// This method is taken directly from the Stack Overflow answer https://stackoverflow.com/a/7108263/6221779.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that the SO answer linked above notes that it might be worth caching the <c>callSite</c> object.
        /// This is currently worth further analysis at a later point, when considering optimisation.
        /// </para>
        /// </remarks>
        /// <returns>The value of the dynamic member.</returns>
        /// <param name="obj">A dynamic object.</param>
        /// <param name="memberName">The member name.</param>
        static object GetDynamicMember(object obj, string memberName)
        {
            var binder = Binder.GetMember(CSharpBinderFlags.None, memberName, obj.GetType(),
                new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
            return callsite.Target(callsite, obj);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DynamicObjectValueProvider"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        public DynamicObjectValueProvider(IGetsValueFromObject wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }

    }
}
