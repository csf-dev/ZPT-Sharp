//  
//  InverseBooleanExpression.cs
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

namespace CraigFowler.Web.ZPT.Tales.Expressions
{
  /// <summary>
  /// <para>Represents an 'inverse boolean' <see cref="TalesExpression"/>.</para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// These expression types contain a 'child' expression.  When evaluating an inverse boolean expression, the child
  /// expression is first evaluated and then the boolean inverse is returned.
  /// </para>
  /// </remarks>
  public class InverseBooleanExpression : TalesExpression
  {
    #region constants
    
    /// <summary>
    /// <para>Read-only.  Constant representing the registered prefix for this type of expression.</para>
    /// </summary>
    public const string Prefix = "not:";
    
    #endregion
    
    #region fields
    
    private TalesExpression inner;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Rea-only.  Gets the 'inner' <see cref="TalesExpression"/> for this instance.</para>
    /// </summary>
    public TalesExpression InnerExpression
    {
      get {
        return inner;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        inner = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Overridden.  Gets the value of this expression.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Object"/>, which will always be a <see cref="System.Boolean"/>.
    /// </returns>
    public override object GetValue()
    {
      return !this.InnerExpression.GetBooleanValue();
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>
    /// Initialises this instance with the given information.  This includes determining the
    /// <see cref="InnerExpression"/>.
    /// </para>
    /// </summary>
    /// <param name="expression">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="context">
    /// A <see cref="TalesContext"/>
    /// </param>
    internal InverseBooleanExpression(string expression, TalesContext context) : base(expression, context)
    {
      this.InnerExpression = context.CreateExpression(this.ExpressionBody);
    }
    
    #endregion
  }
}
