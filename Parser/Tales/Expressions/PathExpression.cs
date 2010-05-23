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
using System.Reflection;

namespace CraigFowler.Web.ZPT.Tales.Expressions
{
  /// <summary>
  /// <para>Represents a <see cref="TalesExpression"/> that represents a 'path' type expression.</para>
  /// </summary>
  public class PathExpression : TalesExpression
  {
    #region constants
    
    private const char PATH_SEPARATOR   = '|';
    public const string Prefix          = "path:";
    
    #endregion
    
    #region fields
    
    private List<TalesPath> paths;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets a queue of the paths that are present in the expression body of this expression.</para>
    /// <para>Independant paths are searated by the pipe character '|'.</para>
    /// </summary>
    public List<TalesPath> Paths
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
      bool success = false;
      object output = null;
      
      for(int i = 0; !success && i < this.Paths.Count; i++)
      {
        try
        {
          output = EvaluatePath(this.Paths[i]);
          success = true;
        }
        catch(Exception)
        {
          success = false;
        }
      }
      
      if(!success)
      {
        throw new FormatException("Could not evaluate any of the given paths.");
      }
      
      return output;
    }
    
    #endregion
    
    #region private methods
    
    /// <summary>
    /// <para>Extracts a <see cref="List"/> of <see cref="TalesPath"/> from the given string.</para>
    /// </summary>
    /// <param name="expression">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="List<TalesPath>"/>
    /// </returns>
    private List<TalesPath> ExtractPaths(string expression)
    {
      List<TalesPath> output = new List<TalesPath>();
      
      foreach(string path in expression.Split(new char[] {PATH_SEPARATOR}))
      {
        output.Add(new TalesPath(path));
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Overloaded. Evaluates a single <see cref="TalesPath"/> to an object reference.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    private object EvaluatePath(TalesPath path)
    {
      object rootReference, output;
      bool rootFound;
      
      if(path == null)
      {
        throw new ArgumentNullException("path");
      }
      else if(path.Parts.Count == 0)
      {
        throw new ArgumentOutOfRangeException("path", "This path has no parts.");
      }

      rootReference = this.Context.GetRootObject(path.Parts[0], out rootFound);
      
      if(!rootFound)
      {
        throw new FormatException("The context does not contain a root object for the beginning of this path.");
      }
      
      if(path.Parts.Count == 1)
      {
        output = rootReference;
      }
      else
      {
        output = EvaluatePath(path, 1, rootReference);
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Overloaded.  Evaluates a portion of a <see cref="TalesPath"/> to an object reference.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <param name="pathPosition">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <param name="parentObject">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    private object EvaluatePath(TalesPath path, int pathPosition, object parentObject)
    {
      object output = null, thisObject = null;
      MemberInfo[] members;
      MemberInfo applicableMember;
      
      if(pathPosition < path.Parts.Count)
      {
        if(parentObject == null)
        {
          throw new ArgumentNullException("currentObject");
        }
        
        members = parentObject.GetType().GetMember(path.Parts[pathPosition]);
        
        if(members.Length == 0)
        {
          throw new FormatException(String.Format("Could not find member '{0}' within the path.",
                                                  path.Parts[pathPosition]));
        }
        
        // FIXME: Really we should be smarter about choosing a member here, but for now we just take the first one.
        applicableMember = members[0];
        
        switch(applicableMember.MemberType)
        {
        case MemberTypes.Property:
          PropertyInfo property = applicableMember as PropertyInfo;
          if(!property.CanRead || !property.GetGetMethod().IsPublic)
          {
            throw new NotSupportedException("Property is not readable");
          }
          thisObject = property.GetValue(parentObject, null);
          break;
        case MemberTypes.Field:
          FieldInfo field = applicableMember as FieldInfo;
          if(!field.IsPublic)
          {
            throw new NotSupportedException("Field is not readable");
          }
          thisObject = field.GetValue(parentObject);
          break;
        case MemberTypes.Method:
          throw new NotImplementedException("Methods aren't supported yet");
        default:
          throw new NotSupportedException(String.Format("Unsupported member type: '{0}'",
                                                        applicableMember.MemberType.ToString()));
        }
        
        pathPosition ++;
        output = EvaluatePath(path, pathPosition, thisObject);
      }
      else
      {
        output = parentObject;
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
