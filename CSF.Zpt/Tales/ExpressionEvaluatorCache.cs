using System;
using System.Linq;

namespace CSF.Zpt.Tales
{
  public class ExpressionEvaluatorCache : ThreadSafeCache<string,IExpressionEvaluator>
  {
    #region fields

    private IExpressionEvaluator _default;
    private bool _populated;
    private object _populatedSync;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the default expression evaluator.
    /// </summary>
    /// <value>The default expression evaluator.</value>
    public IExpressionEvaluator Default
    {
      get {
        try
        {
          base.SyncRoot.EnterReadLock();
          return _default;
        }
        finally
        {
          if(base.SyncRoot.IsReadLockHeld)
          {
            base.SyncRoot.ExitReadLock();
          }
        }
      }
      set {
        try
        {
          base.SyncRoot.EnterWriteLock();
          _default = value;
        }
        finally
        {
          if(base.SyncRoot.IsWriteLockHeld)
          {
            base.SyncRoot.ExitWriteLock();
          }
        }
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a single expression evaluator based on its <c>System.Type</c>.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="desiredType">Desired type.</param>
    public IExpressionEvaluator TryGetByType(Type desiredType)
    {
      if(desiredType == null)
      {
        throw new ArgumentNullException(nameof(desiredType));
      }

      try
      {
        base.SyncRoot.EnterReadLock();

        return base.Cache.Values.SingleOrDefault(x => x.GetType() == desiredType);
      }
      finally
      {
        if(base.SyncRoot.IsReadLockHeld)
        {
          base.SyncRoot.ExitReadLock();
        }
      }
    }

    /// <summary>
    /// Populates the current instance if required.
    /// </summary>
    /// <param name="populationCallback">Population callback.</param>
    public void PopulateIfRequired(Action<ExpressionEvaluatorCache> populationCallback)
    {
      if(populationCallback == null)
      {
        throw new ArgumentNullException(nameof(populationCallback));
      }

      lock(_populatedSync)
      {
        if(_populated)
        {
          return;
        }

        populationCallback(this);
        _populated = true;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ExpressionEvaluatorCache"/> class.
    /// </summary>
    public ExpressionEvaluatorCache()
    {
      _populatedSync = new object();
    }

    #endregion
  }
}

