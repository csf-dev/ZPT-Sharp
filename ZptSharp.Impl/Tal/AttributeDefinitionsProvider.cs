using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ZptSharp.Dom;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IGetsAttributeDefinitions"/> which parses and returns
    /// a collection of <see cref="AttributeDefinition"/> from an 'attributes' attribute value.
    /// </summary>
    public class AttributeDefinitionsProvider : IGetsAttributeDefinitions
    {
        const string
          attributeValuePattern = @"((?:[^;]|;;)+)\s*(?:;(?!;))?\s*",
          singleAttributeAssignmentPattern = @"^(?:(?:([^: ]+):)?([^ ]+) )(.+)$",
          escapedSemicolon = ";;",
          semicolon = ";";

        static readonly Regex
            attributeValue = new Regex(attributeValuePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant),
            singleAttributeAssignment = new Regex(singleAttributeAssignmentPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Gets the attribute definitions from the specified <paramref name="attributesAttributeValue"/>.
        /// </summary>
        /// <returns>The attribute definitions.</returns>
        /// <param name="attributesAttributeValue">The TAL 'attributes' attribute value.</param>
        /// <param name="element">The element node upon which the attributes are defined.</param>
        public IEnumerable<AttributeDefinition> GetDefinitions(string attributesAttributeValue, INode element)
        {
            return GetAttributeValueMatches(attributesAttributeValue, element)
                .Select(x => new AttributeDefinition
                {
                    Prefix = String.IsNullOrEmpty(x.Groups[1].Value)? null : x.Groups[1].Value,
                    Name = x.Groups[2].Value,
                    Expression = UnescapeSemicolons(x.Groups[3].Value)
                })
                .ToList();
        }

        IEnumerable<Match> GetAttributeValueMatches(string attributesAttributeValue, INode element)
        {
            var matches = attributeValue.Matches(attributesAttributeValue)
                .Cast<Match>()
                .Select(x => singleAttributeAssignment.Match(x.Groups[1].Value))
                .ToList();

            if (matches.Any(x => !x.Success))
            {
                var message = String.Format(Resources.ExceptionMessage.InvalidAttributesAttributeValue, element);
                throw new InvalidTalAttributeException(message);
            }

            return matches;
        }

        string UnescapeSemicolons(string val) => val?.Replace(escapedSemicolon, semicolon);
    }
}
