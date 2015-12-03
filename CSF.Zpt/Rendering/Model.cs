using System;
using System.Collections.Generic;
using CSF.Zpt.Resources;

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
    private Dictionary<ZptElement,RepetitionInfo> _repetitionInfo;
    private object _error;

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
    /// Gets information about an error, or a <c>null</c> reference if no error was encountered.
    /// </summary>
    /// <value>The error.</value>
    public virtual object Error
    {
      get {
        return _error;
      }
    }

    /// <summary>
    /// Gets a reference to the parent model (if applicable).
    /// </summary>
    /// <value>The parent.</value>
    public virtual Model Parent
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
    /// Adds a collection of repetition information to the current instance.
    /// </summary>
    /// <param name="info">The repetition information.</param>
    public virtual void AddRepetitionInfo(RepetitionInfo[] info)
    {
      if(info == null)
      {
        throw new ArgumentNullException("info");
      }

      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Adds information about an encountered error to the current model instance.
    /// </summary>
    /// <param name="error">Error.</param>
    public virtual void AddError(object error)
    {
      if(error == null)
      {
        throw new ArgumentNullException("error");
      }
      else if(_error != null)
      {
        throw new InvalidOperationException(ExceptionMessages.ModelMustNotAlreadyContainAnError);
      }

      _error = error;
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
    /// <param name="element">The element for which we are evaluating a result.</param>
    public abstract ExpressionResult Evaluate(string expression, ZptElement element);

    /// <summary>
    /// Tries to get a named item from the current instance.
    /// </summary>
    /// <returns><c>true</c>, if an item was found, <c>false</c> otherwise.</returns>
    /// <param name="name">The item name.</param>
    /// <param name="element">The element for which we are evaluating a result.</param>
    /// <param name="result">Exposes the item which was found.</param>
    protected virtual bool TryGetItem(string name, ZptElement element, out object result)
    {
      bool output = false;
      result = null;

      if(_repetitionInfo != null
         && _repetitionInfo.ContainsKey(element))
      {
        var candidate = _repetitionInfo[element];
        if(candidate.Name == name)
        {
          output = true;
          result = candidate.Value;
        }
      }

      if(!output)
      {
        output = this.TryRecursivelyGetLocalItem(name, out result);
      }

      if(!output)
      {
        if(this.Root.GlobalDefinitions.ContainsKey(name))
        {
          output = true;
          result = this.Root.GlobalDefinitions[name];
        }
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
        output = true;
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

