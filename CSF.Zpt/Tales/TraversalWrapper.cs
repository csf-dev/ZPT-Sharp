using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Wrapper for a traversal delegate.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The rationale for this type, and for using delegates to traverse properties, methods, indexers and fields, is that
  /// it offers a significant speed/performance boost over just using <c>MethodInfo</c>, <c>PropertyInfo</c> and
  /// <c>FieldInfo</c>.
  /// </para>
  /// <para>
  /// The raw reflection items may be converted into delegate instances and cached within the
  /// <see cref="ObjectTraverser"/>, which will significantly speed up all future calls.
  /// </para>
  /// </remarks>
  public class TraversalWrapper
  {
    #region fields

    private MethodTraversal _method;
    private StringIndexTraversal _stringIndex;
    private IntegerIndexTraversal _integerIndex;

    #endregion

    #region methods

    /// <summary>
    /// Traverse the specified object, optionally using a value, and gets the result.
    /// </summary>
    /// <param name="source">Source.</param>
    /// <param name="value">Value.</param>
    public object Traverse(object source, string value)
    {
      if(_method != null)
      {
        return _method(source);
      }
      else if(_stringIndex != null)
      {
        return _stringIndex(source, value);
      }
      else if(_integerIndex != null)
      {
        return _integerIndex(source, Convert.ToInt32(value));
      }
      else
      {
        // Theoretically impossible
        throw new InvalidOperationException();
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TraversalWrapper"/> class.
    /// </summary>
    /// <param name="dele">A delegate instance.</param>
    public TraversalWrapper(MethodTraversal dele)
    {
      if(dele == null)
      {
        throw new ArgumentNullException("dele");
      }

      _method = dele;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TraversalWrapper"/> class.
    /// </summary>
    /// <param name="dele">A delegate instance.</param>
    public TraversalWrapper(StringIndexTraversal dele)
    {
      if(dele == null)
      {
        throw new ArgumentNullException("dele");
      }

      _stringIndex = dele;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TraversalWrapper"/> class.
    /// </summary>
    /// <param name="dele">A delegate instance.</param>
    public TraversalWrapper(IntegerIndexTraversal dele)
    {
      if(dele == null)
      {
        throw new ArgumentNullException("dele");
      }

      _integerIndex = dele;
    }

    #endregion

  }
}

