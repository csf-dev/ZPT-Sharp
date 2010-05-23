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
  public class InverseBooleanExpression : TalesExpression
  {
    #region constants
    
    public const string Prefix = "not:";
    
    #endregion
    
    #region fields
    
    private TalesExpression inner;
    
    #endregion
    
    #region properties
    
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
    
    public override object GetValue()
    {
      return !this.InnerExpression.GetBooleanValue();
    }
    
    #endregion
    
    #region constructor
    
    internal InverseBooleanExpression(string expression, TalesContext context) : base(expression, context)
    {
      this.InnerExpression = context.CreateExpression(this.ExpressionBody);
    }
    
    #endregion
  }
}
