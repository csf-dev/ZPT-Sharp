using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="Model"/> which always contains nothing.
  /// </summary>
  internal sealed class EmptyModel : Model
  {
    public override IModel CreateChildModel()
    {
      return new EmptyModel(this, this.Root);
    }

    protected override Model CreateTypedSiblingModel()
    {
      return new EmptyModel(this.Parent, this.Root);
    }

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
        string message = String.Format(Resources.ExceptionMessages.ModelDoesNotContainItem, expression);
        throw new ModelEvaluationException(message) {
          ElementName = context.Element.Name,
          ExpressionText = expression.ToString(),
        };
      }

      return output;
    }

    internal EmptyModel(NamedObjectWrapper opts) : base(opts) {}

    internal EmptyModel(IModel parent, IModel root) : base(parent, root) {}
  }
}

