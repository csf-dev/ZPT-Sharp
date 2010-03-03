//  
//  TalesExpression.cs
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

namespace CraigFowler.Web.ZPT.Tales
{
  /// <summary>
  /// <para>Describes a single TALES expression.</para>
  /// </summary>
  public abstract class TalesExpression
  {
    #region constants
    
    private const string
      INVERSE_BOOLEN_PREFIX             = "not:",
      PATH_PREFIX                       = "path:",
      STRING_PREFIX                     = "string:";
    
    // This is only used for expressions that have no prefix.  They are assumed to be of this type.
    private const ExpressionType
      DEFAULT_EXPRESSION_TYPE           = ExpressionType.Path;
    
    #endregion
    
    #region fields
    
    private string rawExpression;
    private TalesContext context;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the raw expression that this instance represents.</para>
    /// </summary>
    public string Expression
    {
      get {
        return rawExpression;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        rawExpression = value;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the prefix portion of the <see cref="Expression"/> if present.  If the expression does not
    /// carry a prefix then this property will return an empty string.
    /// </para>
    /// <seealso cref="getPrefix(String)"/>
    /// </summary>
    public string Prefix
    {
      get {
        return getPrefix(Expression);
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the expression body, the part of the <see cref="Expression"/> that follows the
    /// <see cref="Prefix"/>.  This includes leading and trailing whitespace (if present).
    /// </para>
    /// </summary>
    public string Body
    {
      get {
        return Expression.Substring(Prefix.Length);
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the context that this instance works within.</para>
    /// </summary>
    public TalesContext Context
    {
      get {
        return context;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        context = value;
      }
    }
    
    #endregion
    
    #region public methods
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with the given raw expression text and context.</para>
    /// </summary>
    /// <param name="expressionText">
    /// A <see cref="System.String"/>.
    /// </param>
    /// <param name="expressionContext">
    /// A <see cref="TalesContext"/> - the context that this expression works within.
    /// </param>
    protected TalesExpression(string expressionText, TalesContext expressionContext)
    {
      Expression = expressionText;
      Context = expressionContext;
    }
    
    #endregion
    
    #region static methods
    
    /// <summary>
    /// <para>
    /// Creates a new instance of an object that implements <see cref="TalesExpression"/>, appropriate to the
    /// given <paramref name="expression"/> text.
    /// </para>
    /// </summary>
    /// <param name="expression">
    /// A <see cref="System.String"/>, the raw expression text.
    /// </param>
    /// <param name="context">
    /// A <see cref="TalesContext"/> - the context that this expression works within.
    /// </param>
    /// <returns>
    /// An object that implements <see cref="TalesExpression"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the expression type cannot be determined from <paramref name="expression"/> then this exception is raised.
    /// </exception>
    public static TalesExpression ExpressionFactory(string expression, TalesContext context)
    {
      TalesExpression output;
      ExpressionType type;
      
      type = determineExpressionType(expression);
      
      switch(type)
      {
      case ExpressionType.InverseBoolean:
        output = new InverseBooleanExpression(expression, context);
        break;
      case ExpressionType.Path:
        output = new PathExpression(expression, context);
        break;
      case ExpressionType.String:
        output = new StringExpression(expression, context);
        break;
      default:
        throw new NotSupportedException("Unsupported expression type");
      }
      
      return output;
    }
    
    #endregion
    
    #region private static methods
    
    /// <summary>
    /// <para>Parses the prefix on an expression and returns the expression type that matches.</para>
    /// </summary>
    /// <param name="input">
    /// A <see cref="System.String"/>, the raw expression text
    /// </param>
    /// <returns>
    /// An <see cref="ExpressionType"/>
    /// </returns>
    private static ExpressionType determineExpressionType(string input)
    {
      ExpressionType output;
      string prefix;
      
      if(input == null)
      {
        throw new ArgumentNullException("input");
      }
      else if(input == String.Empty)
      {
        throw new ArgumentOutOfRangeException("input", "Expression may not be empty");
      }
      
      prefix = getPrefix(input);
      
      switch(prefix)
      {
      case INVERSE_BOOLEN_PREFIX:
        output = ExpressionType.InverseBoolean;
        break;
      case PATH_PREFIX:
        output = ExpressionType.Path;
        break;
      case STRING_PREFIX:
        output = ExpressionType.String;
        break;
      default:
        output = DEFAULT_EXPRESSION_TYPE;
        break;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Gets the prefix component of the given expression.</para>
    /// </summary>
    /// <param name="expression">
    /// A <see cref="System.String"/>, the raw expression text.
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>, the prefix portion of the <paramref name="expression"/>.  If this is not present
    /// then an empty string is returned.
    /// </returns>
    private static string getPrefix(string expression)
    {
      string output = String.Empty;
      
      if(expression.StartsWith(INVERSE_BOOLEN_PREFIX))
      {
        output = INVERSE_BOOLEN_PREFIX;
      }
      else if(expression.StartsWith(PATH_PREFIX))
      {
        output = PATH_PREFIX;
      }
      else if(expression.StartsWith(STRING_PREFIX))
      {
        output = STRING_PREFIX;
      }
      
      return output;
    }
    
    #endregion
  }
}
