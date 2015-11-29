using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents the result of evaluating a TALES expression.
  /// </summary>
  public class ExpressionResult
  {
    #region fields

    private bool _success;
    private object _result;

    #endregion

    #region properties

    /// <summary>
    /// Gets a value indicating whether the expression evaluation was a success.
    /// </summary>
    /// <value><c>true</c> if the evaluation was a success; otherwise, <c>false</c>.</value>
    public bool EvaluationSuccess
    {
      get {
        return _success;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the result.
    /// </summary>
    /// <returns>The result.</returns>
    public object GetResult()
    {
      if(!this.EvaluationSuccess)
      {
        throw new InvalidOperationException("The result may not be retrieved; check EvaluationSuccess first.");
      }

      return _result;
    }

    /// <summary>
    /// Gets the result, as a typed object instance.
    /// </summary>
    /// <returns>The result.</returns>
    /// <typeparam name="TResult">The expected result type.</typeparam>
    public TResult GetResult<TResult>()
    {
      return (TResult) this.GetResult();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ExpressionResult"/> class.
    /// </summary>
    /// <param name="success">If set to <c>true</c> then the current instance represents a successful evaluation.</param>
    /// <param name="result">The result of the expression evaluation (if the evaluation was a success).</param>
    public ExpressionResult(bool success, object result)
    {
      _success = success;
      _result = result;
    }

    #endregion
  }
}

