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

using CraigFowler.Web.ZPT.Tales.Expressions;
using System;
using System.Collections;

namespace CraigFowler.Web.ZPT.Tales
{
  /// <summary>
  /// <para>Abstract base class for all TALES expressions.</para>
  /// </summary>
  public abstract class TalesExpression
  {
    #region constants
    
    // This is only used for expressions that have no prefix.  They are assumed to be of this type.
    private const ExpressionType
      DEFAULT_EXPRESSION_TYPE           = ExpressionType.Path;
    
    #endregion
    
    #region fields
    
    private string prefix, body;
    private TalesContext context;
    private ExpressionType type;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the raw text of the expression that this instance represents.</para>
    /// </summary>
    public virtual string ExpressionText
    {
      get {
        string output;
        
        if(this.ExpressionPrefix != null)
        {
          output = String.Format("{0}{1}", this.ExpressionPrefix, this.ExpressionBody);
        }
        else
        {
          output = this.ExpressionBody;
        }
        
        return output;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        this.ExpressionPrefix = GetExpressionPrefix(value);
        this.ExpressionBody = (this.ExpressionPrefix != null)? value.Substring(this.ExpressionPrefix.Length) : value;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the prefix portion of the <see cref="ExpressionText"/>, if present.</para>
    /// <para>If this expression has no prefix then this property will return a null reference.</para>
    /// <seealso cref="GetExpressionPrefix(String)"/>
    /// </summary>
    public string ExpressionPrefix
    {
      get {
        return prefix;
      }
      private set {
        if(String.IsNullOrEmpty(value))
        {
          prefix = null;
        }
        else
        {
          prefix = value;
        }
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the expression body, the part of the <see cref="ExpressionText"/> that follows the
    /// <see cref="ExpressionPrefix"/>.  This includes leading and trailing whitespace (if present).
    /// </para>
    /// </summary>
    public string ExpressionBody
    {
      get {
        return body;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        body = value;
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
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the <see cref="ExpressionType"/> that indicates which type of expression this instance
    /// represents.
    /// </para>
    /// </summary>
    public ExpressionType ExpressionType
    {
      get {
        return type;
      }
      protected set {
        type = value;
      }
    }
    
    #endregion
    
    #region private and abstract methods
    
    /// <summary>
    /// <para>Evauluates this expression and returns its value.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    public abstract object GetValue();
    
    /// <summary>
    /// <para>Invokes <see cref="GetValue"/> and then returns the boolean equivalent of the resolved value.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// From the TALES specification 1.3 - http://wiki.zope.org/ZPT/TALESSpecification13 relating to 'not' expressions:
    /// </para>
    /// <para>
    /// If the expression supplied does not evaluate to a boolean value, not  will issue a warning and coerce the
    /// expression's value into a boolean type based on the following rules:
    /// </para>
    /// <list type="number">
    /// <item>integer 0 is false</item>
    /// <item>integer > 0 is true</item>
    /// <item>an empty string or other sequence is false</item>
    /// <item>a non-empty string or other sequence is true</item>
    /// <item>a non-value (e.g. void, None, Nil, NULL, etc) is false</item>
    /// <item>all other values are implementation-dependent</item>
    /// </list>
    /// <para>
    /// If no expression string is supplied, an error should be generated.
    /// </para>
    /// <para>This implementation further defines that:</para>
    /// <list type="number">
    /// <item>
    /// Any nonzero number (or that does not become zero upon casting to a <see cref="System.Single"/>) is true.  This
    /// includes negative numbers.
    /// </item>
    /// <item>
    /// Item 5 in the list above (checking for null values) is performed before items 3 and 4.
    /// </item>
    /// <item>
    /// Item 4 refers to sequences.  In this implementation then an object is considered a sequence if it implements
    /// <see cref="ICollection"/>.  In this case its <see cref="ICollection.Count"/> property is checked to determine
    /// its length.
    /// </item>
    /// <item>
    /// An implementation-specific step that is taken is the attempt to use the <see cref="IConvertible"/> interface
    /// also with <see cref="Convert.ToBoolean(Object)"/> to perform the conversion to boolean.
    /// </item>
    /// <item>
    /// Finally, if the value still cannot be coerced to <see cref="Boolean"/> by any other means, false is returned.
    /// </item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public bool GetBooleanValue()
    {
      object resolvedValue = GetValue();
      bool output = false, success = false;
      
      // If the value is already boolean then return that
      if(resolvedValue is Boolean)
      {
        output = (bool) resolvedValue;
        success = true;
      }
      
      // If the value can be converted to a number then output true if the number is non-zero (false otherwise)
      if(!success)
      {
        try
        {
          float floatValue = (float) resolvedValue;
          output = (floatValue != 0);
          success = true;
        }
        catch(InvalidCastException)
        {
          // The resolved value is not a number, for it cannot be cast to a float.
          success = false;
        }
      }
      
      // If the value is null then output false
      if(!success && resolvedValue == null)
      {
        output = false;
        success = true;
      }
      
      // If the value is a string the output true if it is non-empty (false otherwise)
      if(!success && resolvedValue is String)
      {
        String stringValue = resolvedValue as String;
        output = (stringValue.Length > 0);
        success = true;
      }
      
      // If the value is an ICollection then output true if it has more than zero elements (false otherwise)
      if(!success && resolvedValue is ICollection)
      {
        ICollection iCollectionValue = resolvedValue as ICollection;
        output = (iCollectionValue.Count > 0);
        success = true;
      }
      
      // If the value is an IConvertible then output its conversion to boolean
      if(!success && resolvedValue is IConvertible)
      {
        try
        {
          IConvertible convertableValue = resolvedValue as IConvertible;
          output = Convert.ToBoolean(convertableValue);
          success = true;
        }
        catch(InvalidCastException)
        {
          // We could not convert to boolean in this way
          success = false;
        }
      }
      
      /* Return the output value.
       * If all of the attempts above failed to produce a conclusive output value then it will contain the default
       * value of 'false'.
       */
      return output;
    }
    
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
      this.ExpressionText = expressionText;
      this.Context = expressionContext;
      this.ExpressionType = ExpressionType.Unknown;
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
    /// <exception cref="FormatException">
    /// If the expression type cannot be determined from <paramref name="expression"/> then this exception is raised.
    /// </exception>
    internal static TalesExpression ExpressionFactory(string expression, TalesContext context)
    {
      TalesExpression output;
      ExpressionType type;
      
      type = DetermineExpressionType(expression);
      
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
        throw new FormatException("Could not determine the type of the expression.");
      }
      
      output.ExpressionType = type;
      
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
    private static ExpressionType DetermineExpressionType(string input)
    {
      ExpressionType output;
      string prefix;
      
      if(input == null)
      {
        throw new ArgumentNullException("input");
      }
      else if(input == String.Empty)
      {
        throw new ArgumentOutOfRangeException("input", "Empty expressions are not permitted");
      }
      
      prefix = GetExpressionPrefix(input);
      
      switch(prefix)
      {
      case InverseBooleanExpression.Prefix:
        output = ExpressionType.InverseBoolean;
        break;
      case PathExpression.Prefix:
        output = ExpressionType.Path;
        break;
      case StringExpression.Prefix:
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
    private static string GetExpressionPrefix(string expression)
    {
      string output = String.Empty;
      
      if(expression.StartsWith(InverseBooleanExpression.Prefix))
      {
        output = InverseBooleanExpression.Prefix;
      }
      else if(expression.StartsWith(PathExpression.Prefix))
      {
        output = PathExpression.Prefix;
      }
      else if(expression.StartsWith(StringExpression.Prefix))
      {
        output = StringExpression.Prefix;
      }
      
      return output;
    }
    
    #endregion
  }
}
