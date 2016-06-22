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
    private RepetitionInfoCollection _repetitionInfo;
    private Dictionary<ZptElement,ContextualisedRepetitionSummaryWrapper> _cachedRepetitionSummaries;
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
        return _globalDefinitions;
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
    /// Gets or sets the repetition info.
    /// </summary>
    /// <value>The repetition info.</value>
    protected virtual RepetitionInfoCollection RepetitionInfo
    {
      get {
        return _repetitionInfo;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        _repetitionInfo = value;
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

      _root.GlobalDefinitions[name] = value;
    }

    /// <summary>
    /// Adds a collection of repetition information to the current instance.
    /// </summary>
    /// <param name="info">The repetition information.</param>
    public virtual void AddRepetitionInfo(IRepetitionInfo[] info)
    {
      if(info == null)
      {
        throw new ArgumentNullException(nameof(info));
      }

      /* TODO: Rework this - repetition info is now already contextualised to the current element, so there is no
       * need to keep the whole collection.
       */

      this.RepetitionInfo = new RepetitionInfoCollection(this.RepetitionInfo, info);
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

      return output;
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
    public abstract ExpressionResult Evaluate(string expression, RenderingContext context);

    /// <summary>
    /// Tries to get a named item from the current instance.
    /// </summary>
    /// <returns><c>true</c>, if an item was found, <c>false</c> otherwise.</returns>
    /// <param name="name">The item name.</param>
    /// <param name="context">The rendering context for which we are evaluating a result.</param>
    /// <param name="result">Exposes the item which was found.</param>
    protected virtual bool TryGetItem(string name, RenderingContext context, out object result)
    {
      bool output;
      result = null;

      output = this.RepetitionInfo.TryResolveValue(name, context.Element.GetElementChain(), out result);

      if(!output)
      {
        output = this.TryRecursivelyGetLocalItem(name, out result);
      }

      if(!output)
      {
        if(_root.GlobalDefinitions.ContainsKey(name))
        {
          output = true;
          result = _root.GlobalDefinitions[name];
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
    /// Gets the contextualised repetition summaries for the given <see cref="ZptElement"/>.
    /// </summary>
    /// <returns>The repetition summaries.</returns>
    /// <param name="element">Element.</param>
    protected virtual ContextualisedRepetitionSummaryWrapper GetRepetitionSummaries(ZptElement element)
    {
      if(!_cachedRepetitionSummaries.ContainsKey(element))
      {
        var elementChain = element.GetElementChain();
        var summaries = this.RepetitionInfo.GetContextualisedSummaries(elementChain);
        _cachedRepetitionSummaries.Add(element, summaries);
      }

      return _cachedRepetitionSummaries[element];
    }

    /// <summary>
    /// Gets the keyword options specified upon the current instance.
    /// </summary>
    protected virtual NamedObjectWrapper GetKeywordOptions()
    {
      return _root._options;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.Model"/> class.
    /// </summary>
    /// <param name="parent">A reference to the parent model instance, if applicable.</param>
    /// <param name="root">A reference to the root of the model hierarchy.</param>
    public Model(IModel parent, IModel root)
    {
      if(root == null)
      {
        throw new ArgumentNullException(nameof(root));
      }

      _parent = (Model) parent;
      _root = (Model) root;

      this.LocalDefinitions = new Dictionary<string, object>();
      _globalDefinitions = (_root == this)? new Dictionary<string,object>() : null;
      _repetitionInfo = new RepetitionInfoCollection(new RepetitionInfo[0]);
      _cachedRepetitionSummaries = new Dictionary<ZptElement, ContextualisedRepetitionSummaryWrapper>();
    }

    /// <summary>
    /// Initializes a new root of the <see cref="CSF.Zpt.Rendering.Model"/> class.
    /// </summary>
    /// <param name="options">Keyword options.</param>
    public Model(NamedObjectWrapper options)
    {
      _options = options?? new NamedObjectWrapper();
      _parent = null;
      _root = this;

      this.LocalDefinitions = new Dictionary<string, object>();
      _globalDefinitions = (_root == this)? new Dictionary<string,object>() : null;
      _repetitionInfo = new RepetitionInfoCollection(new RepetitionInfo[0]);
      _cachedRepetitionSummaries = new Dictionary<ZptElement, ContextualisedRepetitionSummaryWrapper>();
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
        return ZptConstants.CancellationToken;
      }
    }

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

    #region contained type

    private class EmptyModel : Model
    {
      public override IModel CreateChildModel()
      {
        return new EmptyModel(this, this.Root);
      }

      protected override Model CreateTypedSiblingModel()
      {
        return new EmptyModel(this.Parent, this.Root);
      }

      public override ExpressionResult Evaluate(string expression, RenderingContext context)
      {
        object result;
        ExpressionResult output;

        if(this.TryGetItem(expression, context, out result))
        {
          output = new ExpressionResult(result);
        }
        else
        {
          string message = String.Format(Resources.ExceptionMessages.ModelDoesNotContainItem, expression);
          throw new ModelEvaluationException(message) {
            ElementName = context.Element.Name,
            ExpressionText = expression.ToString(),
          };
        }

        return output;
      }

      internal EmptyModel(NamedObjectWrapper opts) : base(opts) {}

      internal EmptyModel(IModel parent, IModel root) : base(parent, root) {}
    }

    #endregion
  }
}

