using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  public interface ITalesModel : IModel
  {
    ExpressionResult Evaluate(Expression talesExpression, RenderingContext context);

    bool TryGetRootObject(string name, RenderingContext context, out object result);

    bool TryGetLocalRootObject(string name, ZptElement element, out object result);
  }
}

