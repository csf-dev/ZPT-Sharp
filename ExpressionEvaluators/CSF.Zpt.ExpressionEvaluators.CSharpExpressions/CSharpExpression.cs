using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Represents a compiled CSharp expression, providing a gateway API/wrapper to the generated type.
  /// </summary>
  public class CSharpExpression : IEquatable<CSharpExpression>
  {
    #region fields

    private Func<IExpressionHost> _hostCreator;
    private string[] _variableNames;
    private Assembly _hostAssembly;

    #endregion

    #region properties

    /// <summary>
    /// Gets the identifier for the current expression instance.
    /// </summary>
    /// <value>The identifier.</value>
    public virtual int Id
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a collection of the variable names registered for the current instance.
    /// </summary>
    /// <value>The variable names.</value>
    public virtual IEnumerable<string> VariableNames
    {
      get {
        return _variableNames;
      }
    }

    /// <summary>
    /// Gets the text of the expression code.
    /// </summary>
    /// <value>The text.</value>
    public virtual string Text
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a reference to the <c>System.Reflection.Assembly</c> containing the generated code.
    /// </summary>
    /// <value>The host assembly.</value>
    public virtual Assembly HostAssembly
    {
      get {
        return _hostAssembly;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Evaluate the expression for the given variables.
    /// </summary>
    /// <param name="variableDefinitions">Variable definitions.</param>
    public virtual object Evaluate(IDictionary<string,object> variableDefinitions)
    {
      if(variableDefinitions == null)
      {
        throw new ArgumentNullException(nameof(variableDefinitions));
      }

      var variablesDefined = variableDefinitions.Keys.ToArray();
      if(!AreVariablesEquivalent(variablesDefined, _variableNames))
      {
        throw new CSharpExpressionExceptionException(Resources.ExceptionMessages.DefinedVariablesMustMatch) {
          ExpressionText = this.Text
        };
      }

      var host = _hostCreator();
      foreach(var kvp in variableDefinitions)
      {
        host.SetVariableValue(kvp.Key, kvp.Value);
      }
      return host.Evaluate();
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      if(Object.ReferenceEquals(this, obj))
      {
        return true;
      }

      CSharpExpression other = obj as CSharpExpression;
      return Equals(other);
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/>
    /// is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/>.
    /// </summary>
    /// <param name="obj">The <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/> to compare with the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/> is
    /// equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/>; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    /// <summary>
    /// Determines whether two collections of variables are equivalent (order-neutral collection equality).
    /// </summary>
    /// <returns><c>true</c>, if the variable collections are equivalent, <c>false</c> otherwise.</returns>
    /// <param name="listOne">List one.</param>
    /// <param name="listTwo">List two.</param>
    private bool AreVariablesEquivalent(string[] listOne, string[] listTwo)
    {
      int listOneCount = listOne.Count(), listTwoCount = listTwo.Count();
      return (listOneCount == listTwoCount && listOne.Intersect(listTwo).Count() == listOneCount);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/> class.
    /// </summary>
    /// <param name="hostCreator">Host creator.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="expressionText">Expression text.</param>
    /// <param name="variableNames">Variable names.</param>
    /// <param name="hostAssembly">Host assembly.</param>
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

    /// <summary>Operator overload for equality between two expressions</summary>
    /// <returns><c>true</c> of the expressions are equal; <c>false</c> otherwise.</returns>
    /// <param name="first">The first expression.</param>
    /// <param name="second">The second expression.</param>
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

    /// <summary>Operator overload for inequality between two expressions</summary>
    /// <returns><c>true</c> of the expressions are not equal; <c>false</c> otherwise.</returns>
    /// <param name="first">The first expression.</param>
    /// <param name="second">The second expression.</param>
    public static bool operator !=(CSharpExpression first, CSharpExpression second)
    {
      return !(first == second);
    }

    #endregion
  }
}

