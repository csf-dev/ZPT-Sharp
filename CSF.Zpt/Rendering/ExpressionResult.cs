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

    #region methods

    /// <summary>
    /// Gets the result.
    /// </summary>
    /// <returns>The result.</returns>
    public virtual object GetResult()
    {
      return _result;
    }

    /// <summary>
    /// Gets a value determining whether the result cancels the action.
    /// </summary>
    /// <returns><c>true</c>, if the result cancels the action, <c>false</c> otherwise.</returns>
    public virtual bool CancelsAction()
    {
      var result = this.GetResult();
      return result == Model.CancelAction;
    }

    /// <summary>
    /// Invokes <see cref="M:GetResult()"/> and returns the result as a <c>System.Boolean</c> equivalent.
    /// </summary>
    /// <returns><c>true</c>, if the result indicates truth, <c>false</c> otherwise.</returns>
    public virtual bool GetResultAsBoolean()
    {
      bool output;

      var result = this.GetResult();

      if(result == null)
      {
        output = false;
      }
      else
      {
        var type = result.GetType();
        output = (!type.IsValueType || !result.Equals(Activator.CreateInstance(type)));
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
      return (TResult) this.GetResult();
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

