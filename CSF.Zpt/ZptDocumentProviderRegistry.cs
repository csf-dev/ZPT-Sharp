using System;
using System.Collections.Generic;
using System.Threading;

namespace CSF.Zpt
{
  /// <summary>
  /// The default implementation of <see cref="IZptDocumentProviderRegistry"/>, which provdes a singleton fall-back
  /// instance.
  /// </summary>
  public class ZptDocumentProviderRegistry : IZptDocumentProviderRegistry
  {
    #region fields

    private static IZptDocumentProviderRegistry _default;

    private IZptDocumentProvider _defaultHtml, _defaultXml;
    private Dictionary<string,IZptDocumentProvider> _allProviders;
    private ReaderWriterLockSlim _syncRoot;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the default HTML ZPT document provider.
    /// </summary>
    /// <value>The default HTML document provider.</value>
    public IZptDocumentProvider DefaultHtml
    {
      get {
        try
        {
          _syncRoot.EnterReadLock();
          return _defaultHtml;
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
          _defaultHtml = value;
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

    /// <summary>
    /// Gets or sets the default XML ZPT document provider.
    /// </summary>
    /// <value>The default XML document provider.</value>
    public IZptDocumentProvider DefaultXml
    {
      get {
        try
        {
          _syncRoot.EnterReadLock();
          return _defaultXml;
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
          _defaultXml = value;
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
    /// Gets an <see cref="IZptDocumentProvider"/> by its assembly-qualified type name.
    /// </summary>
    /// <param name="typeName">The assembly-qualified name.</param>
    public IZptDocumentProvider Get(string typeName)
    {
      IZptDocumentProvider output;

      try
      {
        _syncRoot.EnterReadLock();


        if(!_allProviders.TryGetValue(typeName, out output))
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
    /// Adds a provider to the current instance, based upon its assembly-qualified type name.
    /// </summary>
    /// <param name="provider">The provider instance to add.</param>
    public void AddProvider(IZptDocumentProvider provider)
    {
      if(provider == null)
      {
        throw new ArgumentNullException(nameof(provider));
      }

      var typeName = provider.GetType().AssemblyQualifiedName;

      try
      {
        _syncRoot.EnterWriteLock();

        _allProviders.Add(typeName, provider);
      }
      finally
      {
        if(_syncRoot.IsWriteLockHeld)
        {
          _syncRoot.ExitWriteLock();
        }
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocumentProviderRegistry"/> class.
    /// </summary>
    public ZptDocumentProviderRegistry()
    {
      _syncRoot = new ReaderWriterLockSlim();
      _allProviders = new Dictionary<string, IZptDocumentProvider>();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.ZptDocumentProviderRegistry"/> class.
    /// </summary>
    static ZptDocumentProviderRegistry()
    {
      _default = new ZptDocumentProviderRegistry();
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets a default singleton instance of <see cref="IZptDocumentProviderRegistry"/>.
    /// </summary>
    /// <value>The default.</value>
    public static IZptDocumentProviderRegistry Default
    {
      get { return _default; }
    }

    #endregion
  }
}

