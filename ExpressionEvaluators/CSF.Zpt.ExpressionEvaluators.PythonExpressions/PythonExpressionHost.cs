using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Runtime;

namespace CSF.Zpt.ExpressionEvaluators.PythonExpressions
{
  /// <summary>
  /// Proof of concept class for executing and evaluating Python expressions with an arbitrary set of variables.
  /// </summary>
  public class PythonExpressionHost
  {
    #region constants

    private const string
      CLASS_NAME = "PythonExpression",
      PYTHON_SCRIPT_TEMPLATE = @"
class PythonExpression:
  def evaluate(self, vals):
    {0}
    return {1}";

    #endregion

    #region fields

    private ScriptEngine _engine;
    private ObjectOperations _operations;

    #endregion

    #region methods

    /// <summary>
    /// Evaluates the given expression, for the given collection of variable names/values.
    /// </summary>
    /// <param name="expression">The Python expression to evaluate.</param>
    /// <param name="variableValues">The variable names &amp; values.</param>
    public object Evaluate(string expression, IDictionary<string,object> variableValues)
    {
      var scope = _engine.CreateScope();

      var vals = SetupScript(expression, variableValues, scope);

      var classDef = scope.GetVariable(CLASS_NAME);
      var instance = _operations.CreateInstance(classDef);

      return (object) instance.evaluate(vals);
    }

    /// <summary>
    /// Sets up the script within a Python scope.
    /// </summary>
    /// <returns>A python tuple representing the (ordered) values of all of the variables.</returns>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="variableValues">The variable names &amp; values.</param>
    /// <param name="scope">The script scope.</param>
    private PythonTuple SetupScript(string expression,
                                    IDictionary<string,object> variableValues,
                                    ScriptScope scope)
    {
      var allNamesAndValues = variableValues
        .Select(x => new { Name = x.Key, Value = x.Value })
        .OrderBy(x => x.Name)
        .ToArray();

      var script = CreateScript(expression, allNamesAndValues.Select(x => x.Name));

      _engine.Execute(script, scope);

      return new PythonTuple(allNamesAndValues.Select(x => x.Value));
    }

    /// <summary>
    /// Creates the <c>System.String</c> python script contents.
    /// </summary>
    /// <returns>The script content.</returns>
    /// <param name="expression">Expression.</param>
    /// <param name="variableNames">Variable names.</param>
    private string CreateScript(string expression, IEnumerable<string> variableNames)
    {
      string variables;

      if(variableNames.Count() == 0)
      {
        variables = String.Empty;
      }
      else if(variableNames.Count() == 1)
      {
        variables = String.Format("{0} = vals[0]", variableNames.Single());
      }
      else
      {
        variables = String.Format("{0} = vals", String.Join(", ", variableNames));
      }

      return String.Format(PYTHON_SCRIPT_TEMPLATE, variables, expression);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.PythonExpressions.PythonExpressionHost"/> class.
    /// </summary>
    public PythonExpressionHost()
    {
      _engine = Python.CreateEngine();
      _operations = _engine.Operations;
    }

    #endregion
  }
}

