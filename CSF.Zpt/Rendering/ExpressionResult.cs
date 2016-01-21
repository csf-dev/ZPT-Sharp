using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents the result of evaluating a TALES expression.
  /// </summary>
  public class ExpressionResult
  {
    #region fields

    private object _result;

    #endregion

    #region properties

    /// <summary>
    /// Gets the result of this expression evaluation.
    /// </summary>
    /// <value>The result.</value>
    public virtual object Result
    {
      get {
        return _result;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Zpt.Rendering.ExpressionResult"/> cancels the action.
    /// </summary>
    /// <value><c>true</c> if the result cancels the action; otherwise, <c>false</c>.</value>
    public virtual bool CancelsAction
    {
      get {
        return this.Result == Model.CancelAction;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Invokes <see cref="M:GetResult()"/> and returns the result as a <c>System.Boolean</c> equivalent.
    /// </summary>
    /// <returns><c>true</c>, if the result indicates truth, <c>false</c> otherwise.</returns>
    public virtual bool GetResultAsBoolean()
    {
      bool output;

      if(this.Result == null)
      {
        output = false;
      }
      else
      {
        var type = this.Result.GetType();
        output = (!type.IsValueType || !this.Result.Equals(Activator.CreateInstance(type)));
      }

      return output;
    }

    /// <summary>
    /// Gets the result, as a typed object instance.
    /// </summary>
    /// <returns>The result.</returns>
    /// <typeparam name="TResult">The expected result type.</typeparam>
    public virtual TResult GetResult<TResult>()
    {
      return (TResult) this.Result;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ExpressionResult"/> class.
    /// </summary>
    /// <param name="result">The result of the expression evaluation.</param>
    public ExpressionResult(object result)
    {
      _result = result;
    }

    #endregion
  }
}

