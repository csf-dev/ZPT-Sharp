using System;

namespace CSF.Zpt.ExpressionEvaluators.PathExpressions
{
  /// <summary>
  /// Represents a part of a TALES path, this type indicates a single traversal from a parent object to a child.
  /// </summary>
  public class PathPart
  {
    #region constants

    private const string INTERPOLATED_SIGNIFIER = "?";

    #endregion

    #region fields

    private string _value;

    #endregion

    #region properties

    /// <summary>
    /// Gets a value indicating whether this instance is interpolated.
    /// </summary>
    /// <value><c>true</c> if this instance is interpolated; otherwise, <c>false</c>.</value>
    public bool IsInterpolated
    {
      get {
        return _value.StartsWith(INTERPOLATED_SIGNIFIER);
      }
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value
    {
      get {
        return this.IsInterpolated? _value.Substring(1) : _value;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.PathExpressions.PathPart"/> class.
    /// </summary>
    /// <param name="value">Value.</param>
    internal PathPart(string value)
    {
      if(value == null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      else if(value.Length == 0)
      {
        throw new FormatException(Resources.ExceptionMessages.TalesPathPartMustNotBeEmpty);
      }

      _value = value;
    }

    #endregion
  }
}

