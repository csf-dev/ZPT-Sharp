//  
//  DuplicateMemberException.cs
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
  /// <para>
  /// Represents a <see cref="TalesException"/> that was raised due to encountering an ambiguous/duplicate member during
  /// the traversal of a <see cref="Expressions.PathExpression"/>.
  /// </para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// This can be raised because either two members in the same type have been decorated with
  /// <see cref="TalesAliasAttribute"/> with identical values for the Alias property.
  /// </para>
  /// <para>
  /// It could also be raised because no members within the target type could be found with aliases matchig the target
  /// identifier, and the identifier resolves to an overloaded member.  Overloaded members are not supported.
  /// </para>
  /// </remarks>
  public class DuplicateMemberException : TalesException
  {
    #region properties
    
    /// <summary>
    /// <para>Gets and sets the identifier that was used to identify a member.</para>
    /// </summary>
    public string Identifier
    {
      get {
        string output;
        
        if(this.Data["identifier"] is string)
        {
          output = this.Data["identifier"] as string;
        }
        else
        {
          output = null;
        }
        
        return output;
      }
      set {
        this.Data["identifier"] = value;
      }
    }
    
    /// <summary>
    /// <para>
    /// Gets and sets whether the duplicate member was found by parsing a <see cref="TalesAliasAttribute"/> or not.
    /// </para>
    /// </summary>
    public bool UsedAlias
    {
      get {
        bool output;
        
        if(this.Data["used alias"] is bool)
        {
          output = (bool) this.Data["used alias"];
        }
        else
        {
          output = false;
        }
        
        return output;
      }
      set {
        this.Data["used alias"] = value;
      }
    }
    
    #endregion
    
    #region constructors
    
    public DuplicateMemberException(string identifier, bool alias) : base("The given identifier pointed to an " +
                                                                          "ambiguous member reference.")
    {
      this.PermanentError = true;
      this.Identifier = identifier;
      this.UsedAlias = alias;
    }
    
    #endregion
  }
}
