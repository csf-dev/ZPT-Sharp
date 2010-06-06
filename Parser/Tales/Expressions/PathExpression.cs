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
    
    private const char PATH_SEPARATOR             = '|';
    private const string INDEXER_IDENTIFIER       = "Item";
    
    /// <summary>
    /// <para>The prefix used to indicate that the current expression is a path expression.</para>
    /// </summary>
    public const string Prefix                    = "path:";
    
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
      
      for(int i = 0;
          success == false && i < this.Paths.Count;
          i++)
      {
        output = EvaluatePath(this.Paths[i]);
        success = true;
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
    /// <exception cref="ArgumentNullException">
    /// If the <paramref name="path"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the <see cref="TalesPath.Parts.Count"/> of the parts in the <paramref name="path"/> is zero.
    /// </exception>
    /// <exception cref="TalesException">
    /// If there i a problem evaluating the <paramref name="path"/>.  This could be caused by a problem in fetching
    /// the root object reference from the <see cref="TalesExpression.Context"/> or if an unknown (or null) reference
    /// is followed whilst traversing the path.
    /// </exception>
    private object EvaluatePath(TalesPath path)
    {
      object rootReference, output;
      
      // Quick sanity check on the path parameter
      if(path == null)
      {
        throw new ArgumentNullException("path");
      }
      else if(path.Parts.Count == 0)
      {
        throw new ArgumentOutOfRangeException("path", "The path has no parts.");
      }
      
      // Make an attempt to get a reference to the root of the path expression from the current context
      try
      {
        rootReference = this.Context.GetRootObject(path.Parts[0]);
      }
      catch(ArgumentException ex)
      {
        TalesException talesException;
        talesException = new TalesException("Could not fetch the root object of the path from the current context",
                                            ex);
        talesException.Data.Add("path", path);
        talesException.Data.Add("context", this.Context);
        throw talesException;
      }
      
      // Now traverse the parts of the path
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
    /// <param name="position">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <param name="parentObject">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If the <paramref name="parentObject"/> is null and this is not the last step in traversing the
    /// <paramref name="path"/>.
    /// </exception>
    private object EvaluatePath(TalesPath path, int position, object parentObject)
    {
      object output = null, thisObject = null;
      MemberInfo currentMember;
      bool indexer;
      
      if(position < path.Parts.Count)
      {
        currentMember = SelectMember(parentObject, path.Parts[position], out indexer);
        
        switch(currentMember.MemberType)
        {
        case MemberTypes.Property:
          PropertyInfo property = currentMember as PropertyInfo;
          if(property.CanRead)
          {
            thisObject = InvokeMethod(property.GetGetMethod(), parentObject, path, ref position, false);
          }
          else
          {
            TalesException ex = new TalesException("Cannot traverse a non-readable property.");
            ex.Data.Add("member", currentMember);
            ex.Data.Add("target type", parentObject.GetType());
            throw ex;
          }
          break;
        case MemberTypes.Method:
          MethodInfo method = currentMember as MethodInfo;
          thisObject = InvokeMethod(method, parentObject, path, ref position, indexer);
          break;
        case MemberTypes.Field:
          FieldInfo field = currentMember as FieldInfo;
          if(field.IsPublic)
          {
            thisObject = field.GetValue(parentObject);
          }
          else
          {
            TalesException ex = new TalesException("Cannot traverse a non-readable field.");
            ex.Data.Add("member", currentMember);
            ex.Data.Add("target type", parentObject.GetType());
            throw ex;
          }
          break;
        default:
          TalesException ex = new TalesException("The member type encountered whilst traversing the path expression " +
                                                 "is not supported.");
          ex.Data.Add("member", currentMember);
          ex.Data.Add("target type", parentObject.GetType());
          throw ex;
        }
        
        // And now we recurse into ourself, traversing another piece of the path each time we go.
        position ++;
        output = EvaluatePath(path, position, thisObject);
      }
      else
      {
        output = parentObject;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Selects a member from a <paramref name="containingObject"/> based on its <see cref="MemberInfo.Name"/>.
    /// </para>
    /// </summary>
    /// <param name="containingObject">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <param name="memberIdentifier">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="MemberInfo"/>
    /// </returns>
    private MemberInfo SelectMember(object containingObject, string memberIdentifier, out bool isIndexer)
    {
      MemberInfo foundFromAlias = null, foundFromName = null, foundFromIndexer = null, output = null;
      Type containingType;
      object[] attributes;
      MemberInfo[] members;
      
      // Quick sanity-check for some impossible situations
      if(containingObject == null)
      {
        throw new ArgumentNullException("containingObject");
      }
      else if(String.IsNullOrEmpty(memberIdentifier))
      {
        throw new ArgumentException("Member identifier may not be null or empty.", "memberIdentifier");
      }
      
      isIndexer = false;
      
      // Get the type of the object that we were passed
      containingType = containingObject.GetType();
      
      // Perform a linear search through the type's members to find one with the alias we are looking for.
      foreach(MemberInfo member in containingType.GetMembers())
      {
        attributes = member.GetCustomAttributes(typeof(TalesAliasAttribute), true);
        
        foreach(object attribute in attributes)
        {
          if(((TalesAliasAttribute) attribute).Alias == memberIdentifier)
          {
            if(foundFromAlias == null)
            {
              foundFromAlias = member;
            }
            else
            {
              TalesException ex = new DuplicateMemberException(memberIdentifier, true);
              ex.Data.Add("target type", containingType);
              throw ex;
            }
          }
        }
      }
      
      // If we haven't found a member yet then try picking one using its name
      if(foundFromAlias == null)
      {
        members = containingType.GetMember(memberIdentifier);
      
        if(members.Length == 1)
        {
          foundFromName = members[0];
        }
        else if(members.Length > 1)
        {
          TalesException ex = new DuplicateMemberException(memberIdentifier, false);
          ex.Data.Add("target type", containingType);
          throw ex;
        }
      }
      
      // If we still haven't found a member then if this happens to have an indexer then try that!
      if(foundFromAlias == null && foundFromName == null)
      {
        members = containingType.GetMember(INDEXER_IDENTIFIER);
        
        if(members.Length == 1 && members[0].MemberType == MemberTypes.Property)
        {
          PropertyInfo indexer = members[0] as PropertyInfo;
          MethodInfo indexerGet = indexer.GetGetMethod();
          if(indexerGet.GetParameters().Length == 1)
          {
            foundFromIndexer = indexerGet;
          }
        }
      }
      
      // Decide what we are returning
      if(foundFromAlias != null)
      {
        output = foundFromAlias;
      }
      else if(foundFromName != null)
      {
        output = foundFromName;
      }
      else if(foundFromIndexer != null)
      {
        output = foundFromIndexer;
        isIndexer = true;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Invokes the given method with parameters (if applicable) and returns the value.</para>
    /// </summary>
    /// <param name="method">
    /// A <see cref="MethodInfo"/>
    /// </param>
    /// <param name="targetObject">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <param name="basePosition">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    private object InvokeMethod(MethodInfo method,
                                object targetObject,
                                TalesPath path,
                                ref int basePosition,
                                bool useCurrentPosition)
    {
      object[] parameterValues = new object[method.GetParameters().Length];
      int parameterPosition = useCurrentPosition? basePosition : basePosition +1;
      
      if(method.ReturnType == typeof(void))
      {
        ArgumentOutOfRangeException ex;
        ex = new ArgumentOutOfRangeException("Cannot invoke and traverse a method with a void return type.");
        ex.Data.Add("method name", method.Name);
        ex.Data.Add("target type", targetObject.GetType());
        throw ex;
      }
      
      // Extract all of the parameter information from the path (where applicable)
      for(int i = 0; i < parameterValues.Length; i++)
      {
        if(parameterPosition >= path.Parts.Count)
        {
          IndexOutOfRangeException ex;
          ex = new IndexOutOfRangeException("Parameters to the given method require more path pieces than are " +
                                            "available.");
          ex.Data.Add("parameter count", parameterValues.Length);
          ex.Data.Add("method name", method.Name);
          ex.Data.Add("target type", targetObject.GetType());
          throw ex;
        }
        else
        {
          parameterValues[i] = path.Parts[parameterPosition];
          parameterPosition ++;
        }
      }
      
      return method.Invoke(targetObject, parameterValues);
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
