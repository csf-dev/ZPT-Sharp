using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.CSharp;

namespace CSF.Zpt.ExpressionEvaluators.PythonExpressions
{
  public class PythonPoC
  {
    #region constants

    private const string
      CLASS_NAME = "PythonExpression",
      PYTHON_SCRIPT = @"
class PythonExpression:
  def evaluate(self,a,b,c):
    return a+b+c";

    #endregion

    #region fields

    private ScriptEngine _engine;

    #endregion

    #region methods

    public object Evaluate()
    {
      var scope = _engine.CreateScope();
      var ops = _engine.Operations;

      _engine.Execute(PYTHON_SCRIPT, scope);

      var pythonClass = scope.GetVariable(CLASS_NAME);
      var instance = ops.CreateInstance(pythonClass);

      return (object) instance.evaluate(1, 2, 3);
    }

    #endregion

    #region constructor

    public PythonPoC()
    {
      _engine = Python.CreateEngine();
    }

    #endregion
  }
}

