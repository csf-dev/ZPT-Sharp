//  
//  TalesMemberAttribute.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2011 Craig Fowler
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
  /// <para>Attribute marks a member and modifies how it behaves when accessed by TALES.</para>
  /// </summary>
  [AttributeUsage (AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field,
                   Inherited = true,
                   AllowMultiple = false)]
  public class TalesMemberAttribute : Attribute
  {
    #region fields
    
    private string talesAlias;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a <see cref="System.String"/> alias that this member may be referred to with when being
    /// accessed by TALES.
    /// </para>
    /// </summary>
    public string Alias
    {
      get {
        return talesAlias;
      }
      private set {
        if(String.IsNullOrEmpty(value))
        {
          throw new ArgumentException("TALES alias may not be null or empty.", "value");
        }
        
        talesAlias = value;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a value that indicates whether or not this member should be ignored or not when being accessed
    /// by TALES.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this property is true then this member will be ignored when TALES is searching for members to return within
    /// a path expression.
    /// </para>
    /// </remarks>
    public bool Ignore
    {
      get;
      private set;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this attribute in a 'do nothing' state.</para>
    /// </summary>
    protected TalesMemberAttribute ()
    {
      talesAlias = null;
      this.Ignore = false;
    }
    
    /// <summary>
    /// <para>Initialises this attribute with a string alias.</para>
    /// </summary>
    /// <param name="alias">
    /// A <see cref="System.String"/>
    /// </param>
    public TalesMemberAttribute (string alias) : this()
    {
      this.Alias = alias;
    }
    
    /// <summary>
    /// <para>
    /// Initialises this attribute with a value that indicates whether or not the member will be ignored or not.
    /// </para>
    /// </summary>
    /// <param name="ignore">
    /// A <see cref="System.Boolean"/>
    /// </param>
    public TalesMemberAttribute (bool ignore) : this()
    {
      this.Ignore = ignore;
    }
    
    #endregion
  }
}

