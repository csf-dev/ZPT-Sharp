using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An implementation of <see cref="IConfiguresRootContext"/> which allows setting
    /// of global variables into an expression context.
    /// </summary>
    public class RootContextConfigHelper : IConfiguresRootContext
    {
        readonly ExpressionContext context;

        /// <summary>
        /// Adds a named value to the root ZPT context.
        /// </summary>
        /// <param name="name">The name of the value to be added.</param>
        /// <param name="value">The value to be added for the name.</param>
        public void AddToRootContext(string name, object value)
        {
            context.GlobalDefinitions.Add(name, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootContextConfigHelper"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public RootContextConfigHelper(ExpressionContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
