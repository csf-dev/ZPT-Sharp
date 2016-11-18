using System;

namespace CSF.Zpt.ExpressionEvaluators.PathExpressions
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
  public class TraversalWrapper<TSource> : TraversalWrapper
  {
    #region fields

    private Func<TSource,object> _method;
    private Func<TSource,string,object> _stringIndex;
    private Func<TSource,int,object> _integerIndex;

    #endregion

    #region methods

    /// <summary>
    /// Traverse the specified object, optionally using a value, and gets the result.
    /// </summary>
    /// <returns>The result of the traversal.</returns>
    /// <param name="source">Source.</param>
    /// <param name="value">Value.</param>
    public object Traverse(TSource source, string value)
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

    /// <summary>
    /// Traverse the specified object, optionally using a value, and gets the result.
    /// </summary>
    /// <returns>The result of the traversal.</returns>
    /// <param name="source">Source.</param>
    /// <param name="value">Value.</param>
    public override object Traverse(object source, string value)
    {
      return this.Traverse((TSource) source, value);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.PathExpressions.TraversalWrapper"/> class.
    /// </summary>
    /// <param name="dele">A delegate instance.</param>
    public TraversalWrapper(Func<TSource,object> dele)
    {
      if(dele == null)
      {
        throw new ArgumentNullException(nameof(dele));
      }

      _method = dele;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.PathExpressions.TraversalWrapper"/> class.
    /// </summary>
    /// <param name="dele">A delegate instance.</param>
    public TraversalWrapper(Func<TSource,string,object> dele)
    {
      if(dele == null)
      {
        throw new ArgumentNullException(nameof(dele));
      }

      _stringIndex = dele;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.PathExpressions.TraversalWrapper"/> class.
    /// </summary>
    /// <param name="dele">A delegate instance.</param>
    public TraversalWrapper(Func<TSource,int,object> dele)
    {
      if(dele == null)
      {
        throw new ArgumentNullException(nameof(dele));
      }

      _integerIndex = dele;
    }

    #endregion

  }
}

