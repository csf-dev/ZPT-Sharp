//  
//  TalesNamespaceOperationPart.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 Craig Fowler
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
  /// <para>A <see cref="ITalesPathPart"/> that represents a TALES namespace operation.</para>
  /// </summary>
  public class TalesNamespaceOperationPart : ITalesPathPart
  {
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the raw <see cref="System.String"/> text for this path piece.</para>
    /// </summary>
    public string Text
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the name/identifier for the namespace module that will provide the operation.</para>
    /// </summary>
    public string NamespaceModuleIdentifier
    {
      get {
        return this.Text.Split(new char[] {TalesPath.NamespacePrefixSeparator}, 2)[0];
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the name/identifier for the operation that will be performed on the TALES path output.
    /// </para>
    /// </summary>
    public string OperationIdentifier
    {
      get {
        return this.Text.Split(new char[] {TalesPath.NamespacePrefixSeparator}, 2)[1];
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Returns a human-readable representation of the current instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public override string ToString ()
    {
      return this.Text;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance.</para>
    /// </summary>
    /// <param name="text">
    /// A <see cref="System.String"/>
    /// </param>
    public TalesNamespaceOperationPart (string text)
    {
      this.Text = text;
    }
    
    #endregion
  }
}

