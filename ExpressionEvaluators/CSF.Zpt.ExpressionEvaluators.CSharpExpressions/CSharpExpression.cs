using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class CSharpExpression : IEquatable<CSharpExpression>
  {
    #region fields

    private Func<IExpressionHost> _hostCreator;
    private string[] _variableNames;

    #endregion

    #region properties

    public virtual IEnumerable<string> VariableNames
    {
      get {
        return _variableNames;
      }
    }

    public virtual string Text
    {
      get;
      private set;
    }

    #endregion

    #region methods

    public virtual object Evaluate(IDictionary<string,object> variableDefinitions)
    {
      if(variableDefinitions == null)
      {
        throw new ArgumentNullException(nameof(variableDefinitions));
      }

      var variablesDefined = variableDefinitions.Keys.OrderBy(x => x).ToArray();
      if(variablesDefined != _variableNames)
      {
        // TODO: Create a better exception type and put a message in a res file.
        throw new InvalidOperationException();
      }

      var host = _hostCreator();
      foreach(var kvp in variableDefinitions)
      {
        host.SetPropertyValue(kvp.Key, kvp.Value);
      }
      return host.Evaluate();
    }

    public override bool Equals(object obj)
    {
      if(Object.ReferenceEquals(this, obj))
      {
        return true;
      }

      CSharpExpression other = obj as CSharpExpression;
      return Equals(other);
    }

    public virtual bool Equals(CSharpExpression obj)
    {
      if(Object.ReferenceEquals(this, obj))
      {
        return true;
      }
      if((object) obj == null)
      {
        return false;
      }

      return (obj.Text.Equals(this.Text)
              && obj.VariableNames.Equals(this.VariableNames));
    }

    public override int GetHashCode()
    {
      return _variableNames.GetHashCode() ^ Text.GetHashCode();
    }

    #endregion

    #region constructor

    public CSharpExpression(Func<IExpressionHost> hostCreator,
                            string expressionText,
                            IEnumerable<string> variableNames)
    {
      if(hostCreator == null)
      {
        throw new ArgumentNullException(nameof(hostCreator));
      }
      if(expressionText == null)
      {
        throw new ArgumentNullException(nameof(expressionText));
      }
      if(variableNames == null)
      {
        throw new ArgumentNullException(nameof(variableNames));
      }

      Text = expressionText;
      _variableNames = variableNames.OrderBy(x => x).ToArray();
      _hostCreator = hostCreator;
    }

    #endregion

    #region operator overloads

    public static bool operator ==(CSharpExpression first, CSharpExpression second)
    {
      if(Object.ReferenceEquals(first, second))
      {
        return true;
      }

      if((object) first == null)
      {
        return false;
      }

      return first.Equals(second);
    }

    public static bool operator !=(CSharpExpression first, CSharpExpression second)
    {
      return !(first == second);
    }

    #endregion
  }
}

