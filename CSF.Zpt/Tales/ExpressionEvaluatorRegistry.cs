using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// The default implementation of <see cref="IExpressionEvaluatorRegistry"/>, which provdes a singleton fall-back
  /// instance.
  /// </summary>
  public class ExpressionEvaluatorRegistry : IExpressionEvaluatorRegistry
  {
    #region fields

    private static IExpressionEvaluatorRegistry _default;
    private static object _staticSyncRoot;

    private Dictionary<string,IExpressionEvaluator> _evaluatorsByPrefix;
    private Dictionary<Type,IExpressionEvaluator> _evaluatorsByType;
    private IExpressionEvaluator _defaultEvaluator;
    private ReaderWriterLockSlim _syncRoot;

    #endregion

    #region properties

    /// <summary>
    /// Gets the default evaluator.
    /// </summary>
    /// <value>The default evaluator.</value>
    public IExpressionEvaluator DefaultEvaluator
    {
      get {
        try
        {
          _syncRoot.EnterReadLock();
          return _defaultEvaluator;
        }
        finally
        {
          if(_syncRoot.IsReadLockHeld)
          {
            _syncRoot.ExitReadLock();
          }
        }
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        try
        {
          _syncRoot.EnterWriteLock();
          _defaultEvaluator = value;
        }
        finally
        {
          if(_syncRoot.IsWriteLockHeld)
          {
            _syncRoot.ExitWriteLock();
          }
        }
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets an evaluator by its string expression prefix.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="prefix">The associated expression prefix.</param>
    public IExpressionEvaluator GetEvaluator(string prefix)
    {
      if(prefix == null)
      {
        throw new ArgumentNullException(nameof(prefix));
      }

      IExpressionEvaluator output;

      try
      {
        _syncRoot.EnterReadLock();

        if(!_evaluatorsByPrefix.TryGetValue(prefix, out output))
        {
          output = null;
        }
      }
      finally
      {
        if(_syncRoot.IsReadLockHeld)
        {
          _syncRoot.ExitReadLock();
        }
      }

      return output;
    }

    /// <summary>
    /// Gets an evaluator by its <c>System.Type</c>.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="evaluatorType">Evaluator type.</param>
    public IExpressionEvaluator GetEvaluator(Type evaluatorType)
    {
      if(evaluatorType == null)
      {
        throw new ArgumentNullException(nameof(evaluatorType));
      }

      IExpressionEvaluator output;

      try
      {
        _syncRoot.EnterReadLock();

        if(!_evaluatorsByType.TryGetValue(evaluatorType, out output))
        {
          output = null;
        }
      }
      finally
      {
        if(_syncRoot.IsReadLockHeld)
        {
          _syncRoot.ExitReadLock();
        }
      }

      return output;
    }

    /// <summary>
    /// Adds an evaluator instance to the current registry, based upon its <c>System.Type</c> and
    /// <see cref="IExpressionEvaluator.ExpressionPrefix"/>.
    /// </summary>
    /// <param name="evaluator">The evaluator to add.</param>
    public void AddEvaluator(IExpressionEvaluator evaluator)
    {
      if(evaluator == null)
      {
        throw new ArgumentNullException(nameof(evaluator));
      }

      try
      {
        _syncRoot.EnterWriteLock();

        _evaluatorsByPrefix.Add(evaluator.ExpressionPrefix, evaluator);
        _evaluatorsByType.Add(evaluator.GetType(), evaluator);
      }
      finally
      {
        if(_syncRoot.IsWriteLockHeld)
        {
          _syncRoot.ExitWriteLock();
        }
      }
    }

    /// <summary>
    /// Creates the default/singleton provider registry instance.
    /// </summary>
    /// <returns>The default registry.</returns>
    private static IExpressionEvaluatorRegistry CreateDefaultRegistry()
    {
      var output = new ExpressionEvaluatorRegistry();

      var evaluatorMetadataService = new ConfigurationExpressionEvaluatorProvider();

      var evaluators = (from md in evaluatorMetadataService.GetAllEvaluatorMetadata()
                        select new {
          Metadata = md,
          Evaluator = (IExpressionEvaluator) Activator.CreateInstance(md.EvaluatorType)
        })
        .ToArray();

      output.DefaultEvaluator = evaluators.Single(x => x.Metadata.IsDefaultEvaluator).Evaluator;

      foreach(var evaluator in evaluators)
      {
        output.AddEvaluator(evaluator.Evaluator);
      }

      return output;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ExpressionEvaluatorRegistry"/> class.
    /// </summary>
    public ExpressionEvaluatorRegistry()
    {
      _syncRoot = new ReaderWriterLockSlim();
      _evaluatorsByPrefix = new Dictionary<string, IExpressionEvaluator>();
      _evaluatorsByType = new Dictionary<Type, IExpressionEvaluator>();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Tales.ExpressionEvaluatorRegistry"/> class.
    /// </summary>
    static ExpressionEvaluatorRegistry()
    {
      _staticSyncRoot = new object();
      _default = CreateDefaultRegistry();
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets or sets a default singleton instance of <see cref="IExpressionEvaluatorRegistry"/>.
    /// </summary>
    /// <value>The default registry.</value>
    public static IExpressionEvaluatorRegistry Default
    {
      get {
        lock(_staticSyncRoot)
        {
          return _default;
        }
      }
      set {
        lock(_staticSyncRoot)
        {
          if(value == null)
          {
            throw new ArgumentNullException(nameof(value));
          }

          _default = value;
        }
      }
    }

    #endregion
  }
}

