using System;
using CSF.Caches;

namespace CSF.Zpt
{
  /// <summary>
  /// Cache implementation for <see cref="IZptDocumentProvider"/> instances.
  /// </summary>
  internal class ZptDocumentProviderCache : ThreadSafeCache<Type,IZptDocumentProvider>
  {
    #region fields

    private IZptDocumentProvider _defaultHtml, _defaultXml;
    private bool _populated;
    private object _populatedSync;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the default html provider.
    /// </summary>
    /// <value>The default html provider.</value>
    public IZptDocumentProvider DefaultHtmlProvider
    {
      get {
        try
        {
          base.SyncRoot.EnterReadLock();
          return _defaultHtml;
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
          _defaultHtml = value;
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

    /// <summary>
    /// Gets or sets the default xml provider.
    /// </summary>
    /// <value>The default xml provider.</value>
    public IZptDocumentProvider DefaultXmlProvider
    {
      get {
        try
        {
          base.SyncRoot.EnterReadLock();
          return _defaultXml;
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
          _defaultXml = value;
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
    /// Populates the current instance if required.
    /// </summary>
    /// <param name="populationCallback">Population callback.</param>
    public void PopulateIfRequired(Action<ZptDocumentProviderCache> populationCallback)
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
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocumentProviderCache"/> class.
    /// </summary>
    public ZptDocumentProviderCache()
    {
      _populatedSync = new object();
    }

    #endregion
  }
}

