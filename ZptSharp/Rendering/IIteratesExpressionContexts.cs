﻿using System;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which iterates over expression contexts, beginning with a single context and executing a process
    /// over every one of its children.
    /// See also: <seealso cref="IGetsExpressionContextIterator"/> for a factory which may be
    /// used to create instances of this type.
    /// </summary>
    public interface IIteratesExpressionContexts
    {
        /// <summary>
        /// Iterate over the specified <paramref name="context"/>, as well as all of its children.
        /// </summary>
        /// <returns>A task indicating when processing is complete.</returns>
        /// <param name="context">The context over which to iterate.</param>
        Task IterateContextAndChildrenAsync(ExpressionContext context);
    }
}
