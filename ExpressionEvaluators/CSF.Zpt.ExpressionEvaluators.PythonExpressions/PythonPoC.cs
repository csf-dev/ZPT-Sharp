using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSF.Zpt.ExpressionEvaluators.PythonExpressions
{
  public class PythonPoC
  {
    #region constants

    private const string
      CLASS_NAME = "PythonExpression",
      PYTHON_SCRIPT_TEMPLATE = @"
class PythonExpression:
{0}
  def setVariable(self, name, val):
{1}
  def evaluate(self):
    return {2}",
    VARIABLE_DEF_TEMPLATE = "  {0} = None{1}",
    VARIABLE_SET_TEMPLATE = @"    if name == '{0}':
      {0} = val
      return
  {1}";

    #endregion

    #region fields

    private ScriptEngine _engine;

    #endregion

    #region methods

    public object Evaluate(string expression, IDictionary<string,object> variableValues)
    {
      var scope = _engine.CreateScope();
      var ops = _engine.Operations;

      var script = CreateScript(expression, variableValues.Keys.ToArray());

      _engine.Execute(script, scope);

      var pythonClass = scope.GetVariable(CLASS_NAME);
      var instance = ops.CreateInstance(pythonClass);

      foreach(var pairing in variableValues)
      {
        instance.setVariable(pairing.Key, pairing.Value);
      }

      return (object) instance.evaluate();
    }

    private string CreateScript(string expression, IEnumerable<string> variableNames)
    {
      StringBuilder
        variableDefs = new StringBuilder(),
        variableSetters = new StringBuilder();

      foreach(var name in variableNames)
      {
        variableDefs.AppendFormat(VARIABLE_DEF_TEMPLATE, name, Environment.NewLine);
        variableSetters.AppendFormat(VARIABLE_SET_TEMPLATE, name, Environment.NewLine);
      }

      var output = String.Format(PYTHON_SCRIPT_TEMPLATE, variableDefs, variableSetters, expression);

      Console.WriteLine(output);

      return output;
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

