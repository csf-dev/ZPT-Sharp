using System;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents the root of an object model.
  /// </summary>
  public abstract class Model
  {
    #region fields

    private static object _cancelAction = new Object();

    private Model _parent, _root;
    private Dictionary<string,object> _localDefinitions, _globalDefinitions;

    #endregion

    #region properties

    /// <summary>
    /// Gets the global definitions.
    /// </summary>
    /// <value>The global definitions.</value>
    protected virtual Dictionary<string,object> GlobalDefinitions
    {
      get {
        return _globalDefinitions;
      }
    }

    /// <summary>
    /// Gets the local definitions.
    /// </summary>
    /// <value>The local definitions.</value>
    protected virtual Dictionary<string,object> LocalDefinitions
    {
      get {
        return _localDefinitions;
      }
    }

    /// <summary>
    /// Gets a reference to the parent model (if applicable).
    /// </summary>
    /// <value>The parent.</value>
    protected virtual Model Parent
    {
      get {
        return _parent;
      }
    }

    /// <summary>
    /// Gets a reference to the root model instance in the current hierarchy.
    /// </summary>
    /// <value>The root.</value>
    protected virtual Model Root
    {
      get {
        return _root;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Adds a new item to the current local model, identified by a given name and containing a given value.
    /// </summary>
    /// <param name="name">The new item name.</param>
    /// <param name="value">The item value.</param>
    public virtual void AddLocal(string name, object value)
    {
      if(name == null)
      {
        throw new ArgumentNullException("name");
      }

      this.LocalDefinitions[name] = value;
    }

    /// <summary>
    /// Adds a new item to the current local model, identified by a given name and containing a given value.
    /// </summary>
    /// <param name="name">The new item name.</param>
    /// <param name="value">The item value.</param>
    public virtual void AddGlobal(string name, object value)
    {
      if(name == null)
      {
        throw new ArgumentNullException("name");
      }

      this.Root.GlobalDefinitions[name] = value;
    }

    /// <summary>
    /// Creates and returns a child <see cref="Model"/> instance.
    /// </summary>
    /// <returns>The child model.</returns>
    public abstract Model CreateChildModel();

    /// <summary>
    /// Evaluate the specified expression and return the result.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    public abstract ExpressionResult Evaluate(string expression);

    /// <summary>
    /// Tries to get a named item from the current instance.
    /// </summary>
    /// <returns><c>true</c>, if an item was found, <c>false</c> otherwise.</returns>
    /// <param name="name">The item name.</param>
    /// <param name="result">Exposes the item which was found.</param>
    protected virtual bool TryGetItem(string name, out object result)
    {
      bool output = this.TryRecursivelyGetLocalItem(name, out result);

      if(!output)
      {
        output = this.Root.GlobalDefinitions.ContainsKey(name);

        if(output)
        {
          result = this.Root.GlobalDefinitions[name];
        }
      }

      if(!output)
      {
        result = null;
      }

      return output;
    }

    /// <summary>
    /// Tries recursively to get an item from the local scope exposed by the current instance and its parents.
    /// </summary>
    /// <returns><c>true</c>, if an item was found, <c>false</c> otherwise.</returns>
    /// <param name="name">The item name.</param>
    /// <param name="result">Exposes the item which was found.</param>
    protected virtual bool TryRecursivelyGetLocalItem(string name, out object result)
    {
      bool output = false;

      if(this.LocalDefinitions.ContainsKey(name))
      {
        result = this.LocalDefinitions[name];
      }
      else if(this.Parent != null)
      {
        output = this.Parent.TryRecursivelyGetLocalItem(name, out result);
      }
      else
      {
        output = false;
        result = null;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Constructor exists for framework/testing use only - its use is not advised.
    /// </summary>
    protected Model() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.Model"/> class.
    /// </summary>
    /// <param name="parent">A reference to the parent model instance, if applicable.</param>
    /// <param name="root">A reference to the root of the model hierarchy, if applicable.</param>
    public Model(Model parent, Model root)
    {
      _parent = parent;
      _root = root?? this;

      _localDefinitions = new Dictionary<string, object>();
      _globalDefinitions = (_root == this)? new Dictionary<string,object>() : null;
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets a value indicating that the action is to be cancelled.
    /// </summary>
    /// <value><c>true</c> if the action is to be cancelleds; otherwise, <c>false</c>.</value>
    public static object CancelAction
    {
      get {
        return _cancelAction;
      }
    }

    #endregion
  }
}

