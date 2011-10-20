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
using CraigFowler.Web.ZPT.Tales.Exceptions;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace CraigFowler.Web.ZPT.Tales.Expressions
{
  /// <summary>
  /// <para>Represents a <see cref="TalesExpression"/> that represents a 'path' type expression.</para>
  /// </summary>
  public class PathExpression : TalesExpression
  {
    #region constants
    
    private const char PATH_SEPARATOR   = '|';
    
    /* That regex pattern looks pretty uninteligible.  See:
     * 
     * Test.CraigFowler.Web.ZPT.Tales.Expressions.TestPathExpression
     * 
     * for some examples of what it is meant to match.
     */
    private const string
      INDEXER_IDENTIFIER                = "Item",
      VALID_PATH_EXPRESSION_PATTERN     = @"^(((local|global):)?([-\w .,~]+)(/(\??[-\w .,~]+))*)?(\|(((local|global):)?([-\w .,~]+)(/(\??[-\w .,~]+))*)?)*$";
    
    private static readonly Regex
      ValidPathExpression               = new Regex(VALID_PATH_EXPRESSION_PATTERN, RegexOptions.Compiled);
    
    /// <summary>
    /// <para>The prefix used to indicate that the current expression is a path expression.</para>
    /// </summary>
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
    
    /// <summary>
    /// <para>Overridden.  Gets the value of this expression.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    public override object GetValue()
    {
      bool success = false;
      object output = null;
      TraversalException potentialException;
      
      /* We construct this exception, but we don't know if we are going to throw it yet.  We only throw it if the
       * whole traversal process is a failure.
       */
      potentialException = new TraversalException();
      
      for(int i = 0;
          success == false && i < this.Paths.Count;
          i++)
      {
        try
        {
          if(this.Paths[i].Parts.Count == 0)
          {
            output = null;
          }
          else
          {
            output = EvaluatePath(this.Paths[i]);
          }
          
          success = true;
        }
        catch(TalesException ex)
        {
          potentialException.Attempts.Add(this.Paths[i], ex);
        }
      }
      
      // If we never succeded in evaluating any of the paths we were given then throw the traversal exception.
      if(!success)
      {
        throw potentialException;
      }
      
      return output;
    }
    
    #endregion
    
    #region private methods
    
    /// <summary>
    /// <para>Extracts a collection of <see cref="TalesPath"/> from the given string.</para>
    /// </summary>
    /// <param name="expression">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="TalesPath"/> instances.
    /// </returns>
    private List<TalesPath> ExtractPaths(string expression)
    {
      List<TalesPath> output = new List<TalesPath>();
      
      // Detect an invalid path here and throw an exception straight away
      if(!IsValid(expression))
      {
        throw new PathInvalidException(expression);
      }
      
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
    /// If the count of the parts in the <paramref name="path"/> is zero.
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
        throw new PathInvalidException(path, "The path must not be null.");
      }
      else if(path.Parts.Count == 0)
      {
        throw new PathInvalidException(path, "The path has no parts.");
      }
      
      // Make an attempt to get a reference to the root of the path expression from the current context
      try
      {
        rootReference = this.Context.GetRootObject(path.Parts[0]);
      }
      catch(ArgumentException ex)
      {
        throw new PathInvalidException(path, ex);
      }
      
      // Now traverse the rest of the parts of the path
      output = EvaluatePath(path, 1, rootReference);
      
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
      bool suppressPathAdvancement;
      string currentPathPart, memberName;
      
      if(position < path.Parts.Count)
      {
        currentPathPart = path.Parts[position];
        
        // This handles the case for a variable-substitution within a path
        if(currentPathPart.StartsWith("?"))
        {
          memberName = EvaluateSingleVariable(currentPathPart);
        }
				else
				{
					memberName = currentPathPart;
				}
        
        if(parentObject == null)
        {
          throw new PathException(path, "Encountered a null reference part-way through traversing a path.");
        }
        
        try
        {
          currentMember = SelectMember(parentObject,
                                       memberName,
                                       out suppressPathAdvancement);
        }
        catch(ArgumentException ex)
        {
          throw new PathException(path, ex);
        }
        
        if(currentMember == null)
        {
          throw new PathInvalidException(path, "No member could be selected using the current path.");
        }
        
        switch(currentMember.MemberType)
        {
        case MemberTypes.Property:
          PropertyInfo property = currentMember as PropertyInfo;
          if(property.CanRead)
          {
            thisObject = InvokeMethod(property.GetGetMethod(),
                                      parentObject,
                                      path,
                                      ref position,
                                      suppressPathAdvancement);
          }
          else
          {
            TalesException ex = new PathInvalidException(path, "Cannot traverse a non-readable property.");
            ex.Data.Add("member", currentMember);
            ex.Data.Add("target type", parentObject.GetType());
            throw ex;
          }
          break;
        case MemberTypes.Method:
          MethodInfo method = currentMember as MethodInfo;
          thisObject = InvokeMethod(method,
                                    parentObject,
                                    path,
                                    ref position,
                                    suppressPathAdvancement);
          break;
        case MemberTypes.Field:
          FieldInfo field = currentMember as FieldInfo;
          if(field.IsPublic)
          {
            thisObject = field.GetValue(parentObject);
          }
          else
          {
            TalesException ex = new PathInvalidException(path, "Cannot traverse a non-readable field.");
            ex.Data.Add("member", currentMember);
            ex.Data.Add("target type", parentObject.GetType());
            throw ex;
          }
          break;
        default:
          TalesException ex = new PathInvalidException(path,
                                                       "Encountered an unsupported member type whilst " +
                                                       "traversing the path.");
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
    /// <param name="suppressPathAdvancement">
    /// A <see cref="System.Boolean"/>
    /// </param>
    /// <returns>
    /// A <see cref="MemberInfo"/>
    /// </returns>
    private MemberInfo SelectMember(object containingObject, string memberIdentifier, out bool suppressPathAdvancement)
    {
      MemberInfo output = null;
      bool success = false;
      Type containingType;
      
      // Quick sanity-check for some impossible situations
      if(containingObject == null)
      {
        throw new ArgumentNullException("containingObject");
      }
      else if(String.IsNullOrEmpty(memberIdentifier))
      {
        throw new ArgumentException("Member identifier may not be null or empty.", "memberIdentifier");
      }
      
      suppressPathAdvancement = false;
      
      // Get the type of the object that we were passed
      containingType = containingObject.GetType();
      
      // Perform a linear search through the type's members to find one with the alias we are looking for.
      foreach(MemberInfo member in containingType.GetMembers())
      {
        object[] attributes = member.GetCustomAttributes(typeof(TalesMemberAttribute), true);
        
        if(attributes.Length == 1)
        {
          TalesMemberAttribute attribute = (TalesMemberAttribute) attributes[0];
          
          if(attribute.Alias == memberIdentifier && success)
          {
            DuplicateMemberException ex = new DuplicateMemberException(memberIdentifier, true);
            ex.Data.Add("Target type", containingType);
            throw ex;
          }
          else if(attribute.Alias == memberIdentifier)
          {
            output = member;
            success = true;
          }
        }
      }
      
      // If we haven't found a member yet then try picking one using its name
      if(!success)
      {
        MemberInfo[] members = containingType.GetMember(memberIdentifier);
      
        if(members.Length > 1)
        {
          TalesException ex = new DuplicateMemberException(memberIdentifier, false);
          ex.Data.Add("target type", containingType);
          throw ex;
        }
        else if(members.Length == 1)
        {
          object[] attributes = members[0].GetCustomAttributes(typeof(TalesMemberAttribute), true);
          
          if(attributes.Length == 0
             || (attributes.Length == 1
                 && ((TalesMemberAttribute) attributes[0]).Ignore == false))
          {
            output = members[0];
            success = true;
          }
        }
      }
      
      /* If we still haven't found a member then see if the type we are examining has an indexer.
       * If it does then use that!
       */
      if(!success)
      {
        MemberInfo[] members = containingType.GetMember(INDEXER_IDENTIFIER);
        
        if(members.Length > 1)
        {
          TalesException ex = new DuplicateMemberException(memberIdentifier, false);
          ex.Data.Add("target type", containingType);
          throw ex;
        }
        else if(members.Length == 1 && members[0].MemberType == MemberTypes.Property)
        {
          object[] attributes = members[0].GetCustomAttributes(typeof(TalesMemberAttribute), true);
          
          if(attributes.Length == 0
             || (attributes.Length == 1
                 && ((TalesMemberAttribute) attributes[0]).Ignore == false))
          {
            PropertyInfo indexer = members[0] as PropertyInfo;
            MethodInfo getterMethod = indexer.GetGetMethod();
            
            if(getterMethod.GetParameters().Length == 1)
            {
              output = getterMethod;
              suppressPathAdvancement = true;
              success = true;
            }
          }
        }
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Evaluates a small 'inner' TALES path expression (in fact this may only contain a single variable).</para>
    /// </summary>
    /// <param name="variable">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    private string EvaluateSingleVariable(string variable)
    {
			string output;
      PathExpression innerExpression = new PathExpression(variable.Substring(1), this.Context);
			
			output = innerExpression.GetValue().ToString();
			
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
    /// <param name="suppressPositionAdvancement">
    /// A <see cref="System.Boolean"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    private object InvokeMethod(MethodInfo method,
                                object targetObject,
                                TalesPath path,
                                ref int basePosition,
                                bool suppressPositionAdvancement)
    {
      object[] parameterValues = new object[method.GetParameters().Length];
      int offset = suppressPositionAdvancement? 0 : 1;
      object output = null;
      
      if(targetObject == null)
      {
        throw new ArgumentNullException("targetObject");
      }
      
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
        if(basePosition >= path.Parts.Count)
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
					string parameterValue = path.Parts[basePosition + offset];
					
					if(parameterValue.StartsWith("?"))
					{
						parameterValue = EvaluateSingleVariable(parameterValue);
					}
					
          parameterValues[i] = parameterValue;
          basePosition += offset;
        }
      }
      
      try
      {
        output = method.Invoke(targetObject, parameterValues);
      }
      catch(Exception ex)
      {
        Exception interestingException = ex;
        
        while((interestingException is TargetInvocationException)
              && interestingException.InnerException != null)
        {
          interestingException = interestingException.InnerException;
        }
        
        if(interestingException is ZptDocumentParsingException)
        {
          throw interestingException;
        }
        
        string exMessage = String.Format("Encountered an error whilst invoking method '{0}', whilst traversing a " +
                                         "path expression.",
                                         method.Name);
        Exception inner = new PathException(path, exMessage, ex);
        
        ex.Data.Add("parameter count", parameterValues.Length);
        ex.Data.Add("method name", method.Name);
        ex.Data.Add("target type", targetObject.GetType());
        
        throw inner;
      }
      
      return output;
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance and constructs the <see cref="Paths"/> property.</para>
    /// </summary>
    /// <param name="expression">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="context">
    /// A <see cref="TalesContext"/>
    /// </param>
    internal PathExpression(string expression, TalesContext context) : base(expression, context)
    {
      this.Paths = ExtractPaths(ExpressionBody);
    }
    
    #endregion

    #region static methods
    
    /// <summary>
    /// <para>
    /// Validates the given <paramref name="expression"/> as a <see cref="PathExpression"/> and returns a boolean
    /// indicating its validity.
    /// </para>
    /// </summary>
    /// <param name="expression">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool IsValid(string expression)
    {
      return (expression != null)? ValidPathExpression.Match(expression).Success : false;
    }
    
    #endregion
  }
}
