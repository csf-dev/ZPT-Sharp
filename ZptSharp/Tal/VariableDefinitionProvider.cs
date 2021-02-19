using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IGetsVariableDefinitionsFromAttributeValue"/> which parses
    /// and returns information about variable definitions from a TAL 'define' attribute value.
    /// </summary>
    public class VariableDefinitionProvider : IGetsVariableDefinitionsFromAttributeValue
    {
        const string
          variableDefinitionItemPattern = @"((?:[^;]|;;)+)\s*(?:;(?!;))?\s*",
          singleDefinitionPattern = @"^(?:(local|global)\s+)?([^\s]+)\s+(.+)$",
          escapedSemicolon = ";;",
          semicolon = ";";

        static readonly Regex
          SingleDefinition = new Regex(singleDefinitionPattern, RegexOptions.Compiled),
          DefinitionItem = new Regex(variableDefinitionItemPattern, RegexOptions.Compiled);

        /// <summary>
        /// Gets a collection of the variable definitions specified in the attribute value.
        /// This might be one or more separate definitions, semicolon-separated.
        /// </summary>
        /// <returns>The definitions.</returns>
        /// <param name="attributeValue">Attribute value.</param>
        /// <exception cref="FormatException">If the syntax/format of the attribute value is invalid.</exception>
        public IEnumerable<VariableDefinition> GetDefinitions(string attributeValue)
        {
            return (from definitionItemMatch in DefinitionItem.Matches(attributeValue).Cast<Match>()
                    let definitionItemValue = definitionItemMatch.Groups[1].Value
                    let definitionMatch = SingleDefinition.Match(definitionItemValue)
                    let definition = GetVariableDefinition(definitionMatch)
                    select definition)
                .ToList();
        }

        /// <summary>
        /// Converts a regex <see cref="Match"/> object into a <see cref="VariableDefinition"/>.
        /// </summary>
        /// <returns>The variable definition.</returns>
        /// <param name="match">Match.</param>
        VariableDefinition GetVariableDefinition(Match match)
        {
            if (!match.Success)
                throw new FormatException(Resources.ExceptionMessage.TalDefineAttributeMustBeWellFormed);

            return new VariableDefinition
            {
                Scope = match.Groups[1].Value,
                VariableName = match.Groups[2].Value,
                Expression = UnescapeSemicolons(match.Groups[3].Value),
            };
        }

        /// <summary>
        /// Define attributes use semicolons to separate definitions declared within
        /// the same attribute value.  To actually use a semicolon within an expression value, they
        /// must be escaped by doubling-them-up.  This method undoes that and converts back
        /// to single semicolons.
        /// </summary>
        /// <returns>The unescaped expression.</returns>
        /// <param name="expression">Expression.</param>
        string UnescapeSemicolons(string expression) => expression.Replace(escapedSemicolon, semicolon);

    }
}
