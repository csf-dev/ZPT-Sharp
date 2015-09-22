using System;

namespace CSF.Zpt.Tales
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

    /// <summary>
    /// Gets the result.
    /// </summary>
    /// <value>The result.</value>
    public object Result
    {
      get {
        return _result;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the result, typed as requested.
    /// </summary>
    /// <returns>The result.</returns>
    /// <typeparam name="TResult">The expected result type.</typeparam>
    public TResult GetResult<TResult>()
    {
      if(!this.EvaluationSuccess)
      {
        throw new InvalidOperationException("The result may not be retrieved; check EvaluationSuccess first.");
      }

      return (TResult) this.Result;
    }

    #endregion
  }
}

