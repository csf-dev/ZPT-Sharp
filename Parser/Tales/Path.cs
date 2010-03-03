//  
//  Path.cs
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

namespace CraigFowler.Web.ZPT.Tales
{
  /// <summary>
  /// <para>Represents an individual path used within a <see cref="PathExpression"/>.</para>
  /// </summary>
  public class Path
  {
    private const char PARTS_SEPARATOR = '/';
    
    private string rawPath;
    
    public string Text
    {
      get {
        return rawPath;
      }
      private set {
        rawPath = value;
      }
    }
    
    public Queue<string> Parts
    {
      get {
        Queue<string> output = new Queue<string>();
        
        foreach(string part in Text.Split(new char[] {PARTS_SEPARATOR}))
        {
          if(!String.IsNullOrEmpty(part))
          {
            output.Enqueue(part);
          }
        }
        
        return output;
      }
    }
      
    public Path(string path)
    {
      Text = path;
    }
  }
}
