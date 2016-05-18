using System;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Util
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
    public override IModel CreateChildModel()
    {
      return new DummyModel(this, this.Root);
    }

    /// <summary>
    /// Creates an instance of <see cref="DummyModel"/>.
    /// </summary>
    /// <returns>The sibling model.</returns>
    protected override Model CreateTypedSiblingModel()
    {
      return new DummyModel(this.Parent, this.Root);
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
    /// <param name="element">The element for which we are evaluating a result.</param>
    public override ExpressionResult Evaluate(string expression, RenderingContext context)
    {
      object result;
      ExpressionResult output;

      if(this.TryGetItem(expression, context, out result))
      {
        output = new ExpressionResult(result);
      }
      else
      {
        string message = String.Format("The item '{0}' was not found in the model.",
                                       expression);
        throw new InvalidOperationException(message);
      }

      return output;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Test.CSF.Zpt.Util.DummyModel"/> class.
    /// </summary>
    protected DummyModel() : this((TemplateKeywordOptions) null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="Test.CSF.Zpt.Rendering.DummyModel"/> class.
    /// </summary>
    /// <param name="parent">Parent.</param>
    /// <param name="root">Root.</param>
    public DummyModel(IModel parent, IModel root) : base(parent, root) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="Test.CSF.Zpt.Rendering.DummyModel"/> class.
    /// </summary>
    /// <param name="options">Keyword options.</param>
    public DummyModel(TemplateKeywordOptions options) : base(options) {}

    #endregion
  }
}

