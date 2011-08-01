//  
//  MetalMacroCollection.cs
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
using System.Collections.Generic;
namespace CraigFowler.Web.ZPT.Metal
{
  /// <summary>
  /// <para>
  /// Simple collection class to provide access to a collection of <see cref="MetalMacro"/> instances, indexed by
  /// <see cref="System.String"/> names.
  /// </para>
  /// </summary>
  public class MetalMacroCollection
  {
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets an underlying collection of macros.</para>
    /// </summary>
    protected Dictionary<string,MetalMacro> UnderlyingCollection
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Indexer gets and sets the macros into this instance.</para>
    /// </summary>
    /// <param name="key">
    /// A <see cref="System.String"/>
    /// </param>
    public MetalMacro this [string key]
    {
      get {
        if(!this.UnderlyingCollection.ContainsKey(key))
        {
          IndexOutOfRangeException ex = new IndexOutOfRangeException("Unknown macro name.");
          ex.Data["Macro name"] = key;
          throw ex;
        }
        
        return this.UnderlyingCollection[key];
      }
      set {
        this.UnderlyingCollection[key] = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Gets whether or not this collection contains the named macro.</para>
    /// </summary>
    /// <param name="name">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public bool ContainsMacro(string name)
    {
      return this.UnderlyingCollection.ContainsKey(name);
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance with an empty <see cref="UnderlyingCollection"/>.</para>
    /// </summary>
    public MetalMacroCollection ()
    {
      this.UnderlyingCollection = new Dictionary<string, MetalMacro>();
    }
    
    #endregion
  }
}

