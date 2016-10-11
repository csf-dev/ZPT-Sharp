using System;

namespace CSF.Zpt.Tales
{
  public interface IExpressionEvaluatorService
  {
    IExpressionEvaluator GetEvaluator(Type type);

    IExpressionEvaluator GetEvaluator(string prefix);

    IExpressionEvaluator GetDefaultEvaluator();
  }
}

