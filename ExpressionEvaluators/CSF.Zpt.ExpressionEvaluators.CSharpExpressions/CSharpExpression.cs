using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class CSharpExpression : IEquatable<CSharpExpression>
  {
    #region fields

    private Func<IExpressionHost> _hostCreator;
    private string[] _variableNames;
    private Assembly _hostAssembly;

    #endregion

    #region properties

    public virtual int Id
    {
      get;
      private set;
    }

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

    public virtual Assembly HostAssembly
    {
      get {
        return _hostAssembly;
      }
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

      return obj.Id.Equals(this.Id);
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    #endregion

    #region constructor

    public CSharpExpression(Func<IExpressionHost> hostCreator,
                            int id,
                            string expressionText,
                            string[] variableNames,
                            Assembly hostAssembly)
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
      if(hostAssembly == null)
      {
        throw new ArgumentNullException(nameof(hostAssembly));
      }

      Text = expressionText;
      _variableNames = variableNames;
      _hostCreator = hostCreator;
      _hostAssembly = hostAssembly;
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

