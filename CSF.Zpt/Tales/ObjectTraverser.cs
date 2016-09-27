using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Collections;
using CSF.Reflection;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Encapsulates logic for traversing from one TALES object to another.
  /// </summary>
  /// <remarks>
  /// <para>There's reflection here, a LOT of reflection.</para>
  /// <para>
  /// Let's just make the intention of this class clear though, the intention is to enable traversal from a source
  /// object to a child object.  This type currently supports traversing by:
  /// </para>
  /// <list type="bullets">
  /// <item>
  /// By casting the source object to <see cref="ITalesPathHandler"/> and executing the
  /// <see cref="ITalesPathHandler.HandleTalesPath"/> method.
  /// </item>
  /// <item>A string indexer (like a dictionary of string &amp; object, for example)</item>
  /// <item>A method which takes no parameters (using the method name)</item>
  /// <item>A property (using the property name)</item>
  /// <item>A field (using the field name)</item>
  /// <item>A integer indexer (like an IList)</item>
  /// </list>
  /// <para>
  /// The above is also the order of precedence in which those different mechanisms of traversal are attempted.
  /// </para>
  /// <para>
  /// The rest of this class is about creating delegates to avoid using <c>MethodInfo</c> and <c>PropertyInfo</c>
  /// instances for those traversals.  Using the raw reflection classes is slow, and we care a lot about performance
  /// here.
  /// </para>
  /// <para>
  /// What we are doing is (for the traversals which require reflection into a member), constructing a delegate and
  /// caching it within this object instance, indexed by its type, and the name of the path of traversal.  This way any
  /// future calls to traverse that same path will (in theory) be quicker.
  /// </para>
  /// <para>
  /// Traversal via fields is one exception to this at present, and the delegate created for this still (internally)
  /// makes use of the raw reflection type.  Perhaps this could be improved?
  /// </para>
  /// </remarks>
  public class ObjectTraverser
  {
    #region constants

    private const string INDEXER_NAME = "Item";
    private static readonly MethodInfo
      OpenGenericCreateMethod,  
      OpenGenericCreateDelegateReferenceTypeMethod,
      OpenGenericCreateDelegateValueTypeMethod;

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
    /// <param name="currentContext">Gets the current rendering context.</param>
    public bool Traverse(object source, string name, out object result, Rendering.IRenderingContext currentContext)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
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
        output = pathHandler.HandleTalesPath(name, out result, currentContext);
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

          var createMethod = OpenGenericCreateMethod.MakeGenericMethod(key.Item2);
          _methodCache.Add(key, (TraversalWrapper) createMethod.Invoke(this, new Object[] { name }));
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
    /// <param name="name">The name of the path to traverse.</param>
    /// <typeparam name="TSource">The source object type.</typeparam>
    private TraversalWrapper CreateTraversalWrapper<TSource>(string name)
    {
      TraversalWrapper output;

      bool success = (this.TryCreateFromStringIndexer<TSource>(out output)
                      || this.TryCreateFromMethod<TSource>(name, out output)
                      || this.TryCreateFromProperty<TSource>(name, out output)
                      || this.TryCreateFromField<TSource>(name, out output)
                      || this.TryCreateFromIntegerIndexer<TSource>(out output));

      if(!success)
      {
        output = null;
      }

      return output;
    }

    /// <summary>
    /// Attempts to create a <see cref="TraversalWrapper"/> from a named method.
    /// </summary>
    /// <returns><c>true</c>, if the wrapper could be created, <c>false</c> otherwise.</returns>
    /// <param name="name">The name of the path to traverse.</param>
    /// <param name="result">Exposes the result.</param>
    /// <typeparam name="TSource">The source object type.</typeparam>
    private bool TryCreateFromMethod<TSource>(string name, out TraversalWrapper result)
    {
      bool output;

      var method = typeof(TSource).GetMethod(name,
                                             BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
                                             null,
                                             Type.EmptyTypes,
                                             null);
      if(method != null)
      {
        var createMethod = this.SelectCreateDelegateMethod<TSource>();
        var createDelegateMethod = createMethod.MakeGenericMethod(method.DeclaringType, method.ReturnType);
        Func<TSource,object> dele = (Func<TSource,object>) createDelegateMethod.Invoke(this, new object[] { method });
        result = new TraversalWrapper<TSource>(dele);
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
    /// <param name="name">The name of the path to traverse.</param>
    /// <param name="result">Exposes the result.</param>
    /// <typeparam name="TSource">The source object type.</typeparam>
    private bool TryCreateFromProperty<TSource>(string name, out TraversalWrapper result)
    {
      bool output;

      var property = typeof(TSource).GetProperty(name, Type.EmptyTypes);
      if(property != null)
      {
        var method = property.GetGetMethod();
        var createMethod = this.SelectCreateDelegateMethod<TSource>();
        var createDelegateMethod = createMethod.MakeGenericMethod(method.DeclaringType, method.ReturnType);
        Func<TSource,object> dele = (Func<TSource,object>) createDelegateMethod.Invoke(this, new object[] { method });
        result = new TraversalWrapper<TSource>(dele);
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
    /// <param name="result">Exposes the result.</param>
    /// <typeparam name="TSource">The source object type.</typeparam>
    private bool TryCreateFromStringIndexer<TSource>(out TraversalWrapper result)
    {
      bool output;

      var property = typeof(TSource).GetProperty(INDEXER_NAME, new Type[] { typeof(string) });
      if(property != null)
      {
        var method = property.GetGetMethod();
        var dele = (Func<TSource,string,object>) Delegate.CreateDelegate(typeof(Func<TSource,string,object>), null, method);
        result = new TraversalWrapper<TSource>(dele);
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
    /// <param name="result">Exposes the result.</param>
    /// <typeparam name="TSource">The source object type.</typeparam>
    private bool TryCreateFromIntegerIndexer<TSource>(out TraversalWrapper result)
    {
      bool output;

      var property = typeof(TSource).GetProperty(INDEXER_NAME, new Type[] { typeof(int) });
      if(property != null)
      {
        var method = property.GetGetMethod();
        var dele = (Func<TSource,int,object>) Delegate.CreateDelegate(typeof(Func<TSource,int,object>), null, method);
        result = new TraversalWrapper<TSource>(dele);
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
    /// <param name="name">The name of the path to traverse.</param>
    /// <param name="result">Exposes the result.</param>
    /// <typeparam name="TSource">The source object type.</typeparam>
    private bool TryCreateFromField<TSource>(string name, out TraversalWrapper result)
    {
      bool output;

      var field = typeof(TSource).GetField(name);
      if(field != null)
      {
        var dele = new Func<TSource,object>(obj => field.GetValue(obj));
        result = new TraversalWrapper<TSource>(dele);
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
    /// Selects an appropriate <c>System.Reflection.MethodInfo</c> with which to create a
    /// <c>Func&lt;TSource,object&gt;</c>.  This method selects a version suitable either for reference types or value
    /// types.
    /// </summary>
    /// <returns>A method with which to create a delegate.</returns>
    /// <typeparam name="TSource">The source object type.</typeparam>
    private MethodInfo SelectCreateDelegateMethod<TSource>()
    {
      MethodInfo output;

      if(typeof(TSource).IsValueType)
      {
        output = OpenGenericCreateDelegateValueTypeMethod;
      }
      else
      {
        output = OpenGenericCreateDelegateReferenceTypeMethod;
      }

      return output;
    }

    /// <summary>
    /// Creates a method delegate from a method which is callable upon a reference type.
    /// </summary>
    /// <returns>The created delegate.</returns>
    /// <param name="method">The method for which to create a delegate.</param>
    /// <typeparam name="TSource">The source object type.</typeparam>
    /// <typeparam name="TResult">The the method return type.</typeparam>
    private Func<TSource,object> CreateMethodDelegateForReferenceType<TSource,TResult>(MethodInfo method)
      where TSource : class
    {
      var interDele = (Func<TSource,TResult>) Delegate.CreateDelegate(typeof(Func<TSource,TResult>), null, method);
      return new Func<TSource,object>(s => interDele(s));
    }

    /// <summary>
    /// Creates a method delegate from a method which is callable upon a value type.
    /// </summary>
    /// <returns>The created delegate.</returns>
    /// <param name="method">The method for which to create a delegate.</param>
    /// <typeparam name="TSource">The source object type.</typeparam>
    /// <typeparam name="TResult">The the method return type.</typeparam>
    private Func<TSource,object> CreateMethodDelegateForValueType<TSource,TResult>(MethodInfo method)
      where TSource : struct
    {
      var interDele = (FuncByRef<TSource,TResult>) Delegate.CreateDelegate(typeof(FuncByRef<TSource,TResult>), null, method);
      return new Func<TSource,object>(s => interDele(ref s));
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
      OpenGenericCreateMethod = Reflect.Method<ObjectTraverser>(x => x.CreateTraversalWrapper<string>(null))
        .GetGenericMethodDefinition();
      OpenGenericCreateDelegateReferenceTypeMethod = Reflect.Method<ObjectTraverser>(x => x.CreateMethodDelegateForReferenceType<string,object>(null))
        .GetGenericMethodDefinition();
      OpenGenericCreateDelegateValueTypeMethod = Reflect.Method<ObjectTraverser>(x => x.CreateMethodDelegateForValueType<int,object>(null))
        .GetGenericMethodDefinition();
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

