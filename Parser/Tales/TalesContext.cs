//  
//  TalesContext.cs
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
  /// <para>Represents a context for which a <see cref="TalesExpression"/> instance is evaluated against.</para>
  /// </summary>
  /// <remarks>
  /// <para>A context is essentially a 'folder'</para>
  /// </remarks>
  public class TalesContext
  {
    #region constants
    
    private const string
      CONTEXT_ROOT_REFERENCE  = "CONTEXTS",
      NOTHING_REFERENCE       = "nothing",
      DEFAULT_REFERENCE       = "default",
      REPEAT_REFERENCE        = "repeat",
      OPTIONS_REFERENCE       = "options",
      ATTRIBUTES_REFERENCE    = "attrs";
    
    #endregion
    
    #region fields
    
    private TalesContext parentContext;
    private Dictionary<string,object> localDefinitions, rootContexts, localRepeatVariables, options;
    private Dictionary<string,string> attributes;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>
    /// Gets or sets the <see cref="TalesContext"/> that is the immediate parent of this context.  May be null.
    /// </para>
    /// </summary>
    public TalesContext ParentContext
    {
      get {
        return parentContext;
      }
      set {
        parentContext = value;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Returns an editable dictionary of local aliases that are made in this context.</para>
    /// </summary>
    public Dictionary<string, object> Aliases
    {
      get {
        return localDefinitions;
      }
      private set {
        localDefinitions = value;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Returns an editable dictionary of local repeat variables that are made in this context.</para>
    /// </summary>
    public Dictionary<string, object> RepeatVariables
    {
      get {
        return localRepeatVariables;
      }
      private set {
        localRepeatVariables = value;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a dictionary of options set from the keywords arguments passed to this template.
    /// This property is rarely used on the web.
    /// </para>
    /// </summary>
    public Dictionary<string, object> Options
    {
      get {
        return options;
      }
      private set {
        options = value;
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a dictionary of default attribute values for the TAL element that this TALES expression is
    /// used on.
    /// </para>
    /// </summary>
    public Dictionary<string, string> Attributes
    {
      get {
        return attributes;
      }
      private set {
        attributes = value;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Returns the special root contexts for this instance.</para>
    /// </summary>
    protected Dictionary<string,object> RootContexts
    {
      get {
        return rootContexts;
      }
      private set {
        rootContexts = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Creates a new <see cref="TalesExpression"/> using this context and the given expression string.</para>
    /// </summary>
    /// <param name="expressionText">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="TalesExpression"/>
    /// </returns>
    /// <seealso cref="TalesExpression.ExpressionFactory"/>
    public TalesExpression CreateExpression(string expressionText)
    {
      return TalesExpression.ExpressionFactory(expressionText, this);
    }
    
    /// <summary>
    /// <para>
    /// Gets a reference to the object that serves as the 'start-point' of a path reference from an identifier string.
    /// The following priority order is used:
    /// </para>
    /// <list type="number">
    /// <item>If it is the special string CONTEXTS then return the special root contexts dictionary.</item>
    /// <item>If the identifier can be found in the definitions list then return the relevant object.</item>
    /// <item>
    /// If the identifier can be found within the root contexts dictionary then return the relevant item from there.
    /// </item>
    /// <item>If the identifier has not been found then set "found" to false and return null.</item>
    /// </list>
    /// </summary>
    /// <param name="identifier">
    /// A <see cref="System.String"/> - the starting object name in a TALES 'path' string.
    /// </param>
    /// <param name="found">
    /// <para>
    /// A <see cref="System.Boolean"/>, if the object reference was found (that is, it exists as a definition) then this
    /// parameter will output true.  If the given object reference does not exist as a definition, nor is it a builtin
    /// root context then this will output false.
    /// </para>
    /// <para>
    /// Note that this will be set to true even if the given object is null.  It will only be false if no such
    /// definition exists.
    /// </para>
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/> - the object reference to the start point of the path.  Note that this may be null
    /// and that the parameter <paramref name="found"/> should be checked in order to determine whether the object
    /// exists but was legitimately set to null, or whether it does not exist.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If the parameter <paramref name="identifier"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the parameter <paramref name="identifier"/> is an empty string OR if no matching root object reference can
    /// be found in this context.
    /// </exception>
    public object GetRootObject(string identifier)
    {
      object output;
      Dictionary<string,object> definitions = this.GetAliases();
      
      if(identifier == null)
      {
        throw new ArgumentNullException("identifier");
      }
      else if(identifier == String.Empty)
      {
        throw new ArgumentOutOfRangeException("identifier", "The identifier must not be an empty string.");
      }
      else if(identifier == CONTEXT_ROOT_REFERENCE)
      {
        output = this.RootContexts;
      }
      else if(definitions.ContainsKey(identifier))
      {
        output = definitions[identifier];
      }
      else if(this.RootContexts.ContainsKey(identifier))
      {
        output = this.RootContexts[identifier];
      }
      else
      {
        throw new ArgumentOutOfRangeException("identifier",
                                              "No root object can be found matching the given identifier.");
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Creates and returns a new <see cref="TalesContext"/> using the current context as the parent.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="TalesContext"/>
    /// </returns>
    public TalesContext CreateChildContext()
    {
      return new TalesContext(this);
    }
    
    #endregion
    
    #region private methods
    
    /// <summary>
    /// <para>Merges two string dictionaries of definitions and object references.</para>
    /// <para>
    /// If the <paramref name="child"/> dictionary contains entries that have the same name as entries in
    /// <paramref name="parent"/> then the child entries will take precedence.
    /// </para>
    /// </summary>
    /// <param name="parent">
    /// A <see cref="Dictionary<System.String, System.Object>"/>
    /// </param>
    /// <param name="child">
    /// A <see cref="Dictionary<System.String, System.Object>"/>
    /// </param>
    /// <returns>
    /// A <see cref="Dictionary<System.String, System.Object>"/>
    /// </returns>
    private Dictionary<string,object> MergeDictionaries(Dictionary<string,object> parent,
                                                        Dictionary<string,object> child)
    {
      Dictionary<string,object> output = new Dictionary<string, object>();
      
      if(child == null)
      {
        throw new ArgumentNullException("child");
      }
      
      if(parent != null)
      {
        foreach(string key in parent.Keys)
        {
          output.Add(key, parent[key]);
        }
      }
      
      foreach(string key in child.Keys)
      {
        if(output.ContainsKey(key))
        {
          output[key] = child[key];
        }
        else
        {
          output.Add(key, child[key]);
        }
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Generates and returns a dictionary of the effective definitions in this context, by recursing
    /// through the <see cref="ParentContext"/>.
    /// </para>
    /// <para>
    /// Definitions closer to the 'local' level will hide/override/shadow definitions made at the parent
    /// level(s).  This generated value is ephemeral and not for editing.
    /// </para>
    /// </summary>
    /// <returns>
    /// A <see cref="Dictionary"/> of objects, indexed by <see cref="System.String"/> keys.
    /// </returns>
    private Dictionary<string,object> GetAliases()
    {
      Dictionary<string,object> output;
      
      if(this.ParentContext != null)
      {
        output = MergeDictionaries(this.ParentContext.GetAliases(), this.Aliases);
      }
      else
      {
        output = MergeDictionaries(null, this.Aliases);
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Generates and returns a dictionary of the effective repeat variables in this context, by recursing
    /// through the <see cref="ParentContext"/>.  Variables closer to the 'local' level will hide/override/shadow
    /// variables made at the parent level(s).  This generated value is ephemeral and not for editing.
    /// </para>
    /// </summary>
    /// <returns>
    /// A <see cref="Dictionary"/> of objects, indexed by <see cref="System.String"/> keys.
    /// </returns>
    private Dictionary<string,object> GetRepeatVariables()
    {
      Dictionary<string,object> output;
      
      if(this.ParentContext != null)
      {
        output = MergeDictionaries(this.ParentContext.GetRepeatVariables(), this.RepeatVariables);
      }
      else
      {
        output = MergeDictionaries(null, this.RepeatVariables);
      }
      
      return output;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Overloaded.  Initialises this instance with default values.</para>
    /// </summary>
    public TalesContext()
    {
      this.ParentContext = null;
      this.Aliases = new Dictionary<string, object>();
      this.RepeatVariables = new Dictionary<string, object>();
      this.Options = new Dictionary<string, object>();
      this.Attributes = new Dictionary<string, string>();
      
      // Initialise the root contexts dictionary
      this.RootContexts = new Dictionary<string, object>();
      this.RootContexts.Add(NOTHING_REFERENCE, null);
      this.RootContexts.Add(DEFAULT_REFERENCE, new DefaultValueMarker());
      this.RootContexts.Add(REPEAT_REFERENCE, this.GetRepeatVariables());
      this.RootContexts.Add(OPTIONS_REFERENCE, Options);
      this.RootContexts.Add(ATTRIBUTES_REFERENCE, Attributes);
    }
    
    /// <summary>
    /// <para>Overloaded.  Initialises this instance a given parent context.</para>
    /// </summary>
    private TalesContext(TalesContext parent) : this()
    {
      this.ParentContext = parent;
    }
    
    #endregion
  }
}
