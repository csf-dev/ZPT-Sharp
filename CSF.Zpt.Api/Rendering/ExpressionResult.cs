using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents the result of evaluating a TALES expression.
  /// </summary>
  public class ExpressionResult
  {
    #region fields

    private object _value;

    #endregion

    #region properties

    /// <summary>
    /// Gets the result value of this expression evaluation.
    /// </summary>
    /// <value>The value.</value>
    public virtual object Value
    {
      get {
        return _value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Zpt.Rendering.ExpressionResult"/> cancels the action.
    /// </summary>
    /// <value><c>true</c> if the result cancels the action; otherwise, <c>false</c>.</value>
    public virtual bool CancelsAction
    {
      get {
        var canceller = this.Value as IActionCanceller;

        return canceller != null && canceller.ShouldCancelAction();
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Invokes <see cref="M:GetValue()"/> and returns the value as a <c>System.Boolean</c> equivalent.
    /// </summary>
    /// <returns><c>true</c>, if the value indicates truth, <c>false</c> otherwise.</returns>
    public virtual bool GetValueAsBoolean()
    {
      bool output;

      if(this.Value == null)
      {
        output = false;
      }
      else
      {
        var type = this.Value.GetType();
        output = (!type.IsValueType || !this.Value.Equals(Activator.CreateInstance(type)));
      }

      return output;
    }

    /// <summary>
    /// Gets the result value, as a typed object instance.
    /// </summary>
    /// <returns>The value.</returns>
    /// <typeparam name="TResult">The expected value type.</typeparam>
    public virtual TResult GetValue<TResult>()
    {
      return (TResult) this.Value;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ExpressionResult"/> class.
    /// </summary>
    /// <param name="value">The result value of the expression evaluation.</param>
    public ExpressionResult(object value)
    {
      _value = value;
    }

    #endregion
  }
}

