using System;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Enumerates the manners in which an <see cref="ExpressionContext"/> may be used to
    /// provide the root of a path expression.
    /// </summary>
    public enum RootScopeLimitation
    {
        /// <summary>
        /// There is no limitation upon the usage of the expression context in order to
        /// get the root scope of a path expression.  Any defined variable or any built-in
        /// may be used as the root reference.
        /// </summary>
        None = 0,

        /// <summary>
        /// The root of the path expression is limited to the usage of variables
        /// which have been explicitly defined at the global scope.  Local variables
        /// or other built-in contexts may not be referenced.
        /// </summary>
        GlobalVariablesOnly,

        /// <summary>
        /// The root of the path expression is limited to the usage of variables
        /// which have been explicitly defined at the local scope.  Global variables
        /// or other built-in contexts may not be referenced.
        /// </summary>
        LocalVariablesOnly,

        /// <summary>
        /// The root of the path expression is limited to the usage of variables
        /// which have been explicitly defined (at either local or global scope).
        /// Built-in contexts may not be referenced.
        /// </summary>
        DefinedVariablesOnly
    }
}
