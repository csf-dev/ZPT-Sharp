//  
//  StringExpression.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2010 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Text.RegularExpressions;

namespace CraigFowler.Web.ZPT.Tales.Expressions
{
  /// <summary>
  /// <para>Represents a string <see cref="TalesExpression"/>.</para>
  /// </summary>
  public class StringExpression : TalesExpression
  {
    #region constants
    
    /// <summary>
    /// <para>Read-only.  Gets a constant that is the prefix for a string expression type.</para>
    /// </summary>
    public const string Prefix = "string:";
		
		/// <summary>
    /// <para>
    /// Read-only.  Gets a constant that represents an alternative acceptable prefix for a string expression type.
    /// </para>
		/// </summary>
		public const string AlternativePrefix = "str:";
    
    private const string
      LOCATE_REPLACEMENTS     = @"(?<=(?<!\$)(?:\$\$)*)\$(?:(?:\{(?'var'[-\w .,~/|]+)\})|(?'var'[-\w.,~/]+)|(?'var'\$))",
      PERFORM_REPLACEMENTS    = @"\$(?:(?:\{([\w/ ]+)\})|([\w/]+))",
      REPLACE_ESCAPED_DOLLARS = @"\$\$",
      
      /* Careful!
       * This makes it look like we're replacing two dollars with two dollars but in a regex replacement
       * specification a dollar symbol escapes itself!
       * This is because regex uses the syntax $1, $2 etc for numeric replacements.
       */
      DOLLAR_REPLACEMENT      = @"$$";
    
    private static readonly Regex
      MatchReplacements       = new Regex(LOCATE_REPLACEMENTS, RegexOptions.Compiled),
      ReplaceVariables        = new Regex(PERFORM_REPLACEMENTS, RegexOptions.Compiled),
      ReplaceEscapedDollars   = new Regex(REPLACE_ESCAPED_DOLLARS, RegexOptions.Compiled);
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Overridden.  Gets the value of this expression.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    public override object GetValue()
    {
      string output = this.ExpressionBody, pathExpression;
      MatchCollection variableReplacements;
      Match currentMatch;
      
      // Locate all of the places within the input string that require variable replacements
      variableReplacements = MatchReplacements.Matches(output);
      
      for(int i = variableReplacements.Count - 1; i >= 0; i--)
      {
        currentMatch = variableReplacements[i];
        pathExpression = currentMatch.Groups["var"].Value;
        
        /* If we have matched '$$' then this is just an escaped dollar symbol, otherwise we have a replacement
         * to perform, taking the identifier text as a path expression.
         */
        if(pathExpression != "$")
        {
          /* We need to construct and evaluate a path-type expression from the current context and replace with
           * whatever we find from that.
           */
          PathExpression replacement = new PathExpression(pathExpression, this.Context);
          output = ReplaceVariables.Replace(output, replacement.GetValue().ToString(), 1, currentMatch.Index);
        }
        else
        {
          // Just replace the doubled-up dollar symbols with their replacement
          output = ReplaceEscapedDollars.Replace(output, REPLACE_ESCAPED_DOLLARS, 1, currentMatch.Index);
        }
      }
      
      return output;
    }
    
    #endregion

    internal StringExpression(string expression, TalesContext context) : base(expression, context) {}
  }
}
