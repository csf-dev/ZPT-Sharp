using System;
using System.Collections.Generic;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    /// <summary>
    /// An object which parses an attribute value and returns a collection of variable definitions.
    /// </summary>
    public interface IGetsVariableDefinitionsFromAttributeValue
    {
        /// <summary>
        /// Gets a collection of the variable definitions specified in the attribute value.
        /// This might be one or more separate definitions, semicolon-separated.
        /// </summary>
        /// <returns>The definitions.</returns>
        /// <param name="attributeValue">Attribute value.</param>
        /// <exception cref="FormatException">If the syntax/format of the attribute value is invalid.</exception>
        IEnumerable<VariableDefinition> GetDefinitions(string attributeValue);
    }
}
