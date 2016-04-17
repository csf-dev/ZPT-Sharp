using System;
using System.Collections.Generic;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Simple wrapper type which provides access to objects by <c>System.String</c> names.
  /// </summary>
  public class NamedObjectWrapper : ITalesPathHandler
  {
    #region fields

    private Dictionary<string,object> _objects;
    private object _syncRoot;

    #endregion

    #region properties

    public object this [string key]
    {
      get {
        object output;

        lock(_syncRoot)
        {
          output = (_objects.ContainsKey(key))? _objects[key] : null;
        }

        return output;
      }
      set {
        lock(_syncRoot)
        {
          _objects[key] = value;
        }
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets an <c>System.Object</c> based upon a TALES path fragment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should return a <c>System.Object</c> which represents the traversal of a single-level TALES path
    /// fragment, from the current instance.  The value of that fragment is passed via the name
    /// <paramref name="pathFragment"/>.
    /// </para>
    /// <para>
    /// The precise meaning of 'traversal' is left to the implementation, but typical semantics will see an object
    /// return an associated object from an object graph.
    /// </para>
    /// <example>
    /// <para>
    /// In this simple example, the <c>Employee</c> class may return data from a related <c>Person</c> object, without
    /// exposing the Person object directly.  This might be because (as shown in this example), the API of that
    /// <c>Person</c> object is more complex than desired, and so TALES should see a simplified version.
    /// </para>
    /// <code>
    /// public class Employee : ITalesPathHandler
    /// {
    ///   private Person _person;
    ///   
    ///   public object HandleTalesPath(string pathFragment)
    ///   {
    ///     switch(pathFragment)
    ///     {
    ///     case: "name";
    ///       return _person.Name;
    ///     case: "address";
    ///       return _person.Address.FullAddress;
    ///     case: "gender":
    ///       return _person.Gender.ToString();
    ///     default:
    ///       return null;
    ///     }
    ///   }
    /// }
    /// </code>
    /// </example>
    /// <para>
    /// Note that the return value does not need to be a primitive type.  It may be a complex object, and the return
    /// value may also implement <see cref="ITalesPathHandler"/> if desired.
    /// </para>
    /// </remarks>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    public bool HandleTalesPath(string pathFragment, out object result)
    {
      if(pathFragment == null)
      {
        throw new ArgumentNullException(nameof(pathFragment));
      }

      bool output;

      lock(_syncRoot)
      {
        output = _objects.ContainsKey(pathFragment);
        result = output? _objects[pathFragment] : null;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.NamedObjectWrapper"/> class.
    /// </summary>
    public NamedObjectWrapper()
    {
      _objects = new Dictionary<string, object>();
      _syncRoot = new Object();
    }

    #endregion
  }
}

