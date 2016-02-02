using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Collections;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Encapsulates logic for traversing from one TALES object to another.
  /// </summary>
  public class ObjectTraverser
  {
    #region constants

    private const string INDEXER_NAME = "Item";

    #endregion

    #region fields

    private static ObjectTraverser _default;

    private Dictionary<Tuple<string,Type>,TraversalWrapper> _methodCache;
    private ReaderWriterLockSlim _syncRoot;

    #endregion

    #region methods

    /// <summary>
    /// Traverse from the given source object to a child, along a path defined by a given name.
    /// </summary>
    /// <returns><c>true</c> if the traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="source">The source object from which to perform traversal.</param>
    /// <param name="name">The named path of traversal.</param>
    /// <param name="result">Exposes the result of traversal.</param>
    public bool Traverse(object source, string name, out object result)
    {
      if(name == null)
      {
        throw new ArgumentNullException("name");
      }

      bool output;

      if(source == null)
      {
        result = null;
        output = false;
      }
      else if(source is ITalesPathHandler)
      {
        var pathHandler = (ITalesPathHandler) source;
        result = pathHandler.HandleTalesPath(name);
        output = true;
      }
      else
      {
        output = this.TraverseReflection(source, name, out result);
      }

      return output;
    }

    /// <summary>
    /// Traverses from one object to another via reflection.
    /// </summary>
    /// <returns><c>true</c>, if traversal was successful, <c>false</c> otherwise.</returns>
    /// <param name="source">The source object from which to traverse.</param>
    /// <param name="name">The name of the path to traverse.</param>
    /// <param name="result">Exposes the result of traversal.</param>
    private bool TraverseReflection(object source, string name, out object result)
    {
      bool output;
      var key = new Tuple<string,Type>(name, source.GetType());
      TraversalWrapper traversalWrapper;

      try
      {
        _syncRoot.EnterUpgradeableReadLock();

        if(!_methodCache.ContainsKey(key))
        {
          _syncRoot.EnterWriteLock();

          _methodCache.Add(key, this.CreateTraversalWrapper(key));
        }

        traversalWrapper = _methodCache[key];
      }
      finally
      {
        if(_syncRoot.IsWriteLockHeld)
        {
          _syncRoot.ExitWriteLock();
        }
        if(_syncRoot.IsUpgradeableReadLockHeld)
        {
          _syncRoot.ExitUpgradeableReadLock();
        }
      }

      if(traversalWrapper != null)
      {
        try
        {
          result = traversalWrapper.Traverse(source, name);
          output = true;
        }
        catch(Exception)
        {
          result = null;
          output = false;
        }
      }
      else
      {
        result = null;
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Creates and returns a <see cref="TraversalWrapper"/> instance, which encapsulates optimised logic with which to
    /// execute a call to a member of the given type.
    /// </summary>
    /// <returns>The traversal wrapper.</returns>
    /// <param name="key">The key/value pair of path name and object type.</param>
    private TraversalWrapper CreateTraversalWrapper(Tuple<string,Type> key)
    {
      TraversalWrapper output;

      if(this.TryCreateFromStringIndexer(key.Item2, out output))
      {
        // Intentional no-op, we have the output variable we want
      }
      else if(this.TryCreateFromIntegerIndexer(key.Item2, out output))
      {
        // Intentional no-op, we have the output variable we want
      }
      else if(this.TryCreateFromMethod(key.Item2, key.Item1, out output))
      {
        // Intentional no-op, we have the output variable we want
      }
      else if(this.TryCreateFromProperty(key.Item2, key.Item1, out output))
      {
        // Intentional no-op, we have the output variable we want
      }
      else if(this.TryCreateFromField(key.Item2, key.Item1, out output))
      {
        // Intentional no-op, we have the output variable we want
      }
      else
      {
        output = null;
      }

      return output;
    }

    /// <summary>
    /// Attempts to create a <see cref="TraversalWrapper"/> from a named method.
    /// </summary>
    /// <returns><c>true</c>, if the wrapper could be created, <c>false</c> otherwise.</returns>
    /// <param name="objectType">The object type.</param>
    /// <param name="name">The name of the path to traverse.</param>
    /// <param name="result">Exposes the result.</param>
    private bool TryCreateFromMethod(Type objectType, string name, out TraversalWrapper result)
    {
      bool output;

      var method = objectType.GetMethod(name, Type.EmptyTypes);
      if(method != null)
      {
        var dele = (MethodTraversal) Delegate.CreateDelegate(typeof(MethodTraversal), null, method);
        result = new TraversalWrapper(dele);
        output = true;
      }
      else
      {
        result = null;
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Attempts to create a <see cref="TraversalWrapper"/> from a named property.
    /// </summary>
    /// <returns><c>true</c>, if the wrapper could be created, <c>false</c> otherwise.</returns>
    /// <param name="objectType">The object type.</param>
    /// <param name="name">The name of the path to traverse.</param>
    /// <param name="result">Exposes the result.</param>
    private bool TryCreateFromProperty(Type objectType, string name, out TraversalWrapper result)
    {
      bool output;

      var property = objectType.GetProperty(name, Type.EmptyTypes);
      if(property != null)
      {
        var method = property.GetGetMethod();
        var dele = (MethodTraversal) Delegate.CreateDelegate(typeof(MethodTraversal), null, method);
        result = new TraversalWrapper(dele);
        output = true;
      }
      else
      {
        result = null;
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Attempts to create a <see cref="TraversalWrapper"/> from a string indexer.
    /// </summary>
    /// <returns><c>true</c>, if the wrapper could be created, <c>false</c> otherwise.</returns>
    /// <param name="objectType">The object type.</param>
    /// <param name="result">Exposes the result.</param>
    private bool TryCreateFromStringIndexer(Type objectType, out TraversalWrapper result)
    {
      bool output;

      var property = objectType.GetProperty(INDEXER_NAME, new Type[] { typeof(string) });
      if(property != null)
      {
        var method = property.GetGetMethod();
        var dele = (MethodTraversal) Delegate.CreateDelegate(typeof(MethodTraversal), null, method);
        result = new TraversalWrapper(dele);
        output = true;
      }
      else
      {
        result = null;
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Attempts to create a <see cref="TraversalWrapper"/> from an integer indexer.
    /// </summary>
    /// <returns><c>true</c>, if the wrapper could be created, <c>false</c> otherwise.</returns>
    /// <param name="objectType">The object type.</param>
    /// <param name="result">Exposes the result.</param>
    private bool TryCreateFromIntegerIndexer(Type objectType, out TraversalWrapper result)
    {
      bool output;

      var property = objectType.GetProperty(INDEXER_NAME, new Type[] { typeof(int) });
      if(property != null)
      {
        var method = property.GetGetMethod();
        var dele = (MethodTraversal) Delegate.CreateDelegate(typeof(MethodTraversal), null, method);
        result = new TraversalWrapper(dele);
        output = true;
      }
      else
      {
        result = null;
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Attempts to create a <see cref="TraversalWrapper"/> from a named field.
    /// </summary>
    /// <returns><c>true</c>, if the wrapper could be created, <c>false</c> otherwise.</returns>
    /// <param name="objectType">The object type.</param>
    /// <param name="name">The name of the path to traverse.</param>
    /// <param name="result">Exposes the result.</param>
    private bool TryCreateFromField(Type objectType, string name, out TraversalWrapper result)
    {
      bool output;

      var field = objectType.GetField(name);
      if(field != null)
      {
        var dele = new MethodTraversal(obj => field.GetValue(obj));
        result = new TraversalWrapper(dele);
        output = true;
      }
      else
      {
        result = null;
        output = false;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ObjectTraverser"/> class.
    /// </summary>
    public ObjectTraverser()
    {
      _syncRoot = new ReaderWriterLockSlim();
      _methodCache = new Dictionary<Tuple<string, Type>, TraversalWrapper>();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Tales.ObjectTraverser"/> class.
    /// </summary>
    static ObjectTraverser()
    {
      _default = new ObjectTraverser();
    }

    #endregion

    #region static methods

    /// <summary>
    /// Gets a default (singleton) instance of the <see cref="ObjectTraverser"/>.
    /// </summary>
    /// <value>The default instance.</value>
    public static ObjectTraverser Default
    {
      get {
        return _default;
      }
    }

    #endregion
  }
}

