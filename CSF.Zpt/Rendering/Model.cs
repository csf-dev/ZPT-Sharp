using System;
using System.Collections.Generic;
using CSF.Zpt.Resources;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents the root of an object model.
  /// </summary>
  public abstract class Model : IModel
  {
    #region fields

    private Model _parent, _root;
    private Dictionary<string,object> _globalDefinitions;
    private RepetitionMetadataCollectionWrapper _cachedRepetitionSummaries;
    private object _error;
    private NamedObjectWrapper _options;

    #endregion

    #region properties

    /// <summary>
    /// Gets the global definitions.
    /// </summary>
    /// <value>The global definitions.</value>
    protected virtual Dictionary<string,object> GlobalDefinitions
    {
      get {
        return _root._globalDefinitions;
      }
    }

    /// <summary>
    /// Gets the local definitions.
    /// </summary>
    /// <value>The local definitions.</value>
    protected virtual Dictionary<string,object> LocalDefinitions
    {
      get;
      private set;
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
    public virtual IModel Parent
    {
      get {
        return _parent;
      }
    }

    /// <summary>
    /// Gets a reference to the root model instance in the current hierarchy.
    /// </summary>
    /// <value>The root.</value>
    protected virtual IModel Root
    {
      get {
        return _root;
      }
    }

    /// <summary>
    /// Gets the repetition info for the current model instance.
    /// </summary>
    /// <value>The repetition info.</value>
    protected virtual IRepetitionInfo RepetitionInfo
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the model object being rendered.
    /// </summary>
    /// <value>The model object.</value>
    public object ModelObject
    {
      get;
      protected set;
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
        throw new ArgumentNullException(nameof(name));
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
        throw new ArgumentNullException(nameof(name));
      }

      GlobalDefinitions[name] = value;
    }

    /// <summary>
    /// Adds information about a repetition to the current instance.
    /// </summary>
    /// <param name="info">The repetition information.</param>
    public virtual void AddRepetitionInfo(IRepetitionInfo info)
    {
      if(info == null)
      {
        throw new ArgumentNullException(nameof(info));
      }

      this.RepetitionInfo = info;
      this.AddLocal(info.Name, info.Value);
    }


    /// <summary>
    /// Adds information about an encountered error to the current model instance.
    /// </summary>
    /// <param name="error">Error.</param>
    public virtual void AddError(object error)
    {
      if(error == null)
      {
        throw new ArgumentNullException(nameof(error));
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
    public abstract IModel CreateChildModel();

    /// <summary>
    /// Creates and returns a sibling <see cref="Model"/> instance.
    /// </summary>
    /// <returns>The sibling model.</returns>
    public virtual IModel CreateSiblingModel()
    {
      var output = (Model) this.CreateTypedSiblingModel();

      output.LocalDefinitions = new Dictionary<string, object>(this.LocalDefinitions);
      output.RepetitionInfo = this.RepetitionInfo;
      output.ModelObject = this.ModelObject;

      return output;
    }

    /// <summary>
    /// Gets all variable definitions for the current model instance.
    /// </summary>
    /// <returns>The variable definitions.</returns>
    public virtual IDictionary<string,object> GetAllDefinitions()
    {
      var localDefinitions = GetAllLocalDefinitions();
      var globalDefinitions = GlobalDefinitions;
      var rootDefinitions = GetBuiltinDefinitions();

      var localAndGlobal = MergeDefinitionsDictionaries(localDefinitions, globalDefinitions);
      return MergeDefinitionsDictionaries(localAndGlobal, rootDefinitions);
    }

    /// <summary>
    /// Gets a collection of the current model's built-in definitions.
    /// </summary>
    /// <returns>The built-in definitions.</returns>
    protected virtual IDictionary<string,object> GetBuiltinDefinitions()
    {
      return new Dictionary<string, object>();
    }

    /// <summary>
    /// Override in implementor classes to create a strongly-typed instance of <see cref="Model"/>, using the same
    /// model-type as the implementor class.
    /// </summary>
    /// <returns>The sibling model.</returns>
    protected abstract Model CreateTypedSiblingModel();

    /// <summary>
    /// Evaluate the specified expression and return the result.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="context">The rendering context for which we are evaluating a result.</param>
    public abstract ExpressionResult Evaluate(string expression, IRenderingContext context);

    /// <summary>
    /// Tries to get a named item from the current instance.
    /// </summary>
    /// <returns><c>true</c>, if an item was found, <c>false</c> otherwise.</returns>
    /// <param name="name">The item name.</param>
    /// <param name="context">The rendering context for which we are evaluating a result.</param>
    /// <param name="result">Exposes the item which was found.</param>
    protected virtual bool TryGetItem(string name, IRenderingContext context, out object result)
    {
      bool output;
      result = null;

      output = this.TryRecursivelyGetLocalItem(name, out result);

      if(!output)
      {
        if(GlobalDefinitions.ContainsKey(name))
        {
          output = true;
          result = GlobalDefinitions[name];
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
        output = _parent.TryRecursivelyGetLocalItem(name, out result);
      }
      else
      {
        output = false;
        result = null;
      }

      return output;
    }

    /// <summary>
    /// Recursively gets all of the <see cref="IRepetitionInfo"/> instances from the current model and all of its
    /// parents.
    /// </summary>
    /// <returns>The repetition information instances.</returns>
    protected virtual IEnumerable<IRepetitionInfo> RecursivelyGetAllRepetitions()
    {
      var repetitions = new List<IRepetitionInfo>();
      RecursivelyGetAllRepetitions(ref repetitions);
      return repetitions;
    }

    /// <summary>
    /// Recursively gets all of the <see cref="IRepetitionInfo"/> instances from the current model and all of its
    /// parents.
    /// </summary>
    /// <param name="repetitions">The repetitions retrieved so far.</param>
    protected virtual void RecursivelyGetAllRepetitions(ref List<IRepetitionInfo> repetitions)
    {
      if(repetitions == null)
      {
        throw new ArgumentNullException(nameof(repetitions));
      }

      if(this.RepetitionInfo != null)
      {
        repetitions.Add(this.RepetitionInfo);
      }

      if(_parent != null)
      {
        _parent.RecursivelyGetAllRepetitions(ref repetitions);
      }
    }

    /// <summary>
    /// Gets the contextualised repetition summaries.
    /// </summary>
    /// <returns>The repetition summaries.</returns>
    protected virtual RepetitionMetadataCollectionWrapper GetRepetitionSummaries()
    {
      if(_cachedRepetitionSummaries == null)
      {
        var allRepetitions = RecursivelyGetAllRepetitions();
        _cachedRepetitionSummaries = new RepetitionMetadataCollectionWrapper(allRepetitions);
      }

      return _cachedRepetitionSummaries;
    }

    /// <summary>
    /// Gets the keyword options specified upon the current instance.
    /// </summary>
    protected virtual NamedObjectWrapper GetKeywordOptions()
    {
      return _root._options;
    }

    /// <summary>
    /// Merges two dictionaries which contain variable definitions together.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <paramref name="first"/> dictionary will always 'win' in any naming conflicts.  Thus if an
    /// identically-named deifinition exists in both the first and <paramref name="second"/> dictionaries, then the
    /// value from the first will be used.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The result of the merge operation.
    /// </returns>
    /// <param name="first">The first dictionary.</param>
    /// <param name="second">The second dictionary.</param>
    protected virtual IDictionary<string,object> MergeDefinitionsDictionaries(IDictionary<string,object> first,
                                                                              IDictionary<string,object> second)
    {
      if(first == null)
      {
        throw new ArgumentNullException(nameof(first));
      }
      if(second == null)
      {
        throw new ArgumentNullException(nameof(second));
      }

      return first
        .Union(second.Where(x => !first.ContainsKey(x.Key)))
        .ToDictionary(k => k.Key, v => v.Value);
    }

    /// <summary>
    /// Gets a dictionary containing all of the local variable definitions.
    /// </summary>
    /// <returns>The all local definitions.</returns>
    protected virtual IDictionary<string,object> GetAllLocalDefinitions()
    {
      IDictionary<string,object> output;

      if(_parent != null)
      {
        output = MergeDefinitionsDictionaries(LocalDefinitions, _parent.GetAllLocalDefinitions());
      }
      else
      {
        output = new Dictionary<string, object>(LocalDefinitions);
      }

      return output;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.Model"/> class.
    /// </summary>
    /// <param name="parent">A reference to the parent model instance, if applicable.</param>
    /// <param name="root">A reference to the root of the model hierarchy.</param>
    /// <param name="modelObject">The model to be rendered.</param>
    public Model(IModel parent, IModel root, object modelObject = null)
    {
      if(root == null)
      {
        throw new ArgumentNullException(nameof(root));
      }

      _parent = (Model) parent;
      _root = (Model) root;

      this.LocalDefinitions = new Dictionary<string, object>();
      _globalDefinitions = (_root == this)? new Dictionary<string,object>() : null;

      this.ModelObject = modelObject;
    }

    /// <summary>
    /// Initializes a new root of the <see cref="CSF.Zpt.Rendering.Model"/> class.
    /// </summary>
    /// <param name="options">Keyword options.</param>
    /// <param name="modelObject">The model to be rendered.</param>
    public Model(NamedObjectWrapper options, object modelObject = null)
    {
      _options = options?? new NamedObjectWrapper();
      _parent = null;
      _root = this;

      this.LocalDefinitions = new Dictionary<string, object>();
      _globalDefinitions = (_root == this)? new Dictionary<string,object>() : null;

      this.ModelObject = modelObject;
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets an empty model/null object.
    /// </summary>
    /// <value>The empty model.</value>
    public static Model Empty
    {
      get {
        return new EmptyModel(null);
      }
    }

    #endregion
  }
}

