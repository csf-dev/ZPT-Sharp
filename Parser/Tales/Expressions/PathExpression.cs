//  
//  PathExpression.cs
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
using System.Collections.Generic;

namespace CraigFowler.Web.ZPT.Tales.Expressions
{
  /// <summary>
  /// <para>Represents a <see cref="TalesExpression"/> that represents a 'path' type expression.</para>
  /// </summary>
  public class PathExpression : TalesExpression
  {
    #region constants
    
    private const char PATH_SEPARATOR = '|';
    
    #endregion
    
    #region fields
    
    private Queue<TalesPath> paths;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets a queue of the paths that are present in the expression body of this expression.</para>
    /// <para>Independant paths are searated by the pipe character '|'.</para>
    /// </summary>
    public Queue<TalesPath> Paths
    {
      get {
        return paths;
      }
      private set {
        paths = value;
      }
    }
    
    #endregion
    
    #region methods
    
    public override object GetValue()
    {
      throw new NotImplementedException();
    }
    
    #endregion
    
    #region private methods
    
    private Queue<TalesPath> ExtractPaths(string expression)
    {
      Queue<TalesPath> output = new Queue<TalesPath>();
      
      foreach(string path in expression.Split(new char[] {PATH_SEPARATOR}))
      {
        output.Enqueue(new TalesPath(path));
      }
      
      return output;
    }
    
    #endregion
    
    #region constructor
    
    internal PathExpression(string expression, TalesContext context) : base(expression, context)
    {
      this.Paths = ExtractPaths(ExpressionBody);
    }
    
    #endregion
  }
}
