using System;
using System.Collections.Generic;
using System.Threading;

namespace CSF.Zpt
{
  public class ZptDocumentProviderRegistry : IZptDocumentProviderRegistry
  {
    #region fields

    private static IZptDocumentProviderRegistry _default;

    private IZptDocumentProvider _defaultHtml, _defaultXml;
    private Dictionary<string,IZptDocumentProvider> _allProviders;
    private ReaderWriterLockSlim _syncRoot;

    #endregion

    #region properties

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
    }

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
    }

    #endregion

    #region methods

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

    #endregion

    #region constructors

    public ZptDocumentProviderRegistry()
    {
      _syncRoot = new ReaderWriterLockSlim();
      _allProviders = new Dictionary<string, IZptDocumentProvider>();
    }

    static ZptDocumentProviderRegistry()
    {
      _default = new ZptDocumentProviderRegistry();
    }

    #endregion

    #region static properties

    public static IZptDocumentProviderRegistry Default
    {
      get { return _default; }
    }

    #endregion
  }
}

