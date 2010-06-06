//  
//  RootObjectNotFoundException.cs
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

namespace CraigFowler.Web.ZPT.Tales.Exceptions
{
  public class RootObjectNotFoundException : TalesException
  {
    #region constants
    
    private const string DEFAULT_MESSAGE = "The root object in this TALES expression could not be found in the context.";
    
    #endregion
    
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
    
    #endregion
    
    #region constructor
    
    public RootObjectNotFoundException(TalesPath path) : this(path, null) {}
    
    public RootObjectNotFoundException(TalesPath path, Exception inner) : base(DEFAULT_MESSAGE, inner)
    {
      this.PermanentError = true;
      if(path.Parts.Count < 1)
      {
        throw new ArgumentOutOfRangeException("path", "Path has no pieces");
      }
      else
      {
        this.Identifier = path.Parts[0];
      }
    }
    
    public RootObjectNotFoundException(string identifier) : this(identifier, null) {}
    
    public RootObjectNotFoundException(string identifier, Exception inner) : base(DEFAULT_MESSAGE, inner)
    {
      this.PermanentError = true;
      this.Identifier = identifier;
    }
    
    #endregion
  }
}
