using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

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
    private static object _staticSyncRoot;

    private IZptDocumentProvider _defaultHtml, _defaultXml;
    private Dictionary<Type,IZptDocumentProvider> _allProviders;
    private ReaderWriterLockSlim _syncRoot;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the default HTML ZPT document provider.
    /// </summary>
    /// <value>The default HTML document provider.</value>
    public IZptDocumentProvider DefaultHtmlProvider
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
    public IZptDocumentProvider DefaultXmlProvider
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
    /// Gets an <see cref="IZptDocumentProvider"/> by its <c>System.Type</c>.
    /// </summary>
    /// <param name="type">The provider type.</param>
    public IZptDocumentProvider GetProvider(Type type)
    {
      IZptDocumentProvider output;

      try
      {
        _syncRoot.EnterReadLock();


        if(!_allProviders.TryGetValue(type, out output))
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
    /// Adds a provider to the current instance, based upon its <c>System.Type</c>.
    /// </summary>
    /// <param name="provider">The provider instance to add.</param>
    public void AddProvider(IZptDocumentProvider provider)
    {
      if(provider == null)
      {
        throw new ArgumentNullException(nameof(provider));
      }

      var type = provider.GetType();

      try
      {
        _syncRoot.EnterWriteLock();

        _allProviders.Add(type, provider);
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
    private static IZptDocumentProviderRegistry CreateDefaultRegistry()
    {
      var output = new ZptDocumentProviderRegistry();

      var providerMetadataService = new ConfigurationDocumentImplementationProvider();

      var providers = (from md in providerMetadataService.GetAllProviderMetadata()
                       select new {
          Metadata = md,
          Provider = (IZptDocumentProvider) Activator.CreateInstance(md.ProviderType)
        })
        .ToArray();

      output.DefaultHtmlProvider = providers.Single(x => x.Metadata.IsDefaultHtmlProvider).Provider;
      output.DefaultXmlProvider = providers.Single(x => x.Metadata.IsDefaultXmlProvider).Provider;

      foreach(var provider in providers)
      {
        output.AddProvider(provider.Provider);
      }

      return output;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocumentProviderRegistry"/> class.
    /// </summary>
    public ZptDocumentProviderRegistry()
    {
      _syncRoot = new ReaderWriterLockSlim();
      _allProviders = new Dictionary<Type, IZptDocumentProvider>();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.ZptDocumentProviderRegistry"/> class.
    /// </summary>
    static ZptDocumentProviderRegistry()
    {
      _staticSyncRoot = new object();
      _default = CreateDefaultRegistry();
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets or sets a default singleton instance of <see cref="IZptDocumentProviderRegistry"/>.
    /// </summary>
    /// <value>The default registry.</value>
    public static IZptDocumentProviderRegistry Default
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

