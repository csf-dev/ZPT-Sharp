using System;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Rendering
{
  /// <summary>
  /// Dummy implementation of <see cref="Model"/> which cannot evaluate anything.
  /// </summary>
  public class DummyModel : Model
  {
    #region overrides

    /// <summary>
    /// Creates and returns a child <see cref="Model"/> instance.
    /// </summary>
    /// <returns>The child model.</returns>
    public override Model CreateChildModel()
    {
      return new DummyModel(this, this.Root);
    }

    /// <summary>
    /// Evaluate the specified expression and return the result.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation is hard-coded to return an <see cref="ExpressionResult"/> which indicates an evaluation
    /// failure.
    /// </para>
    /// </remarks>
    /// <param name="expression">The expression to evaluate.</param>
    public override ExpressionResult Evaluate(string expression)
    {
      object result;
      ExpressionResult output;

      if(this.TryGetItem(expression, out result))
      {
        output = new ExpressionResult(true, result);
      }
      else
      {
        output = new ExpressionResult(false, null);
      }

      return output;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Test.CSF.Zpt.Rendering.DummyModel"/> class.
    /// </summary>
    protected DummyModel() : this(null, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="Test.CSF.Zpt.Rendering.DummyModel"/> class.
    /// </summary>
    /// <param name="parent">Parent.</param>
    /// <param name="root">Root.</param>
    public DummyModel(Model parent, Model root) : base(parent, root) {}

    #endregion
  }
}

