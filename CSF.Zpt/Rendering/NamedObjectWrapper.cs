﻿using System;
using System.Collections.Generic;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Simple wrapper type which provides access to objects by <c>System.String</c> names.
  /// </summary>
  public class NamedObjectWrapper : ITalesPathHandler
  {
    #region fields

    private IDictionary<string,object> _objects;
    private string _stringRepresentation;
    private bool _hasStringRepresentation;
    private object _syncRoot;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the item with the specified key.
    /// </summary>
    /// <param name="key">Key.</param>
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
    ///   public bool HandleTalesPath(string pathFragment, out object result, RenderingContext currentContext)
    ///   {
    ///     switch(pathFragment)
    ///     {
    ///     case: "name";
    ///       result = _person.Name;
    ///       return true;
    ///     case: "address";
    ///       result = _person.Address.FullAddress;
    ///       return true;
    ///     case: "gender":
    ///       result = _person.Gender.ToString();
    ///       return true;
    ///     default:
    ///       result = null;
    ///       return false;
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
    /// <param name="currentContext">Gets the current rendering context.</param>
    public virtual bool HandleTalesPath(string pathFragment, out object result, Rendering.IRenderingContext currentContext)
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

    /// <summary>
    /// Sets the string representation.
    /// </summary>
    /// <param name="name">Name.</param>
    public virtual void SetStringRepresentation(string name)
    {
      _stringRepresentation = name;
      _hasStringRepresentation = true;
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Rendering.NamedObjectWrapper"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Rendering.NamedObjectWrapper"/>.</returns>
    public override string ToString()
    {
      return _hasStringRepresentation? _stringRepresentation : base.ToString();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.NamedObjectWrapper"/> class.
    /// </summary>
    /// <param name="objects">An optional collection of the initial objects to load into the current instance.</param>
    public NamedObjectWrapper(IDictionary<string,object> objects = null)
    {
      _objects = objects?? new Dictionary<string, object>();
      _syncRoot = new Object();
    }

    #endregion
  }
}
