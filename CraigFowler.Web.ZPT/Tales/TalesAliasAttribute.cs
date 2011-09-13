//  
//  TalesAliasAttribute.cs
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
  /// <para>Used to indicate an alias that TALES may use to refer to a member.</para>
  /// </summary>
  /// <remarks>
  /// <para>This attribute is valid on properties, methods and fields.</para>
  /// </remarks>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
  public class TalesAliasAttribute : Attribute
  {
    private string alias;
    
    /// <summary>
    /// <para>Gets and sets the string alias that TALES may use to refer to the member.</para>
    /// </summary>
    public string Alias
    {
      get {
        return alias;
      }
      set {
        if(String.IsNullOrEmpty(value))
        {
          throw new ArgumentException("Alias cannot be null or empty", "value");
        }
        
        alias = value;
      }
    }
    
    /// <summary>
    /// <para>Default constructor initialises this instance with the given <paramref name="memberAlias"/>.</para>
    /// </summary>
    /// <param name="memberAlias">
    /// A <see cref="System.String"/>
    /// </param>
    public TalesAliasAttribute(string memberAlias)
    {
      this.Alias = memberAlias;
    }
  }
}
