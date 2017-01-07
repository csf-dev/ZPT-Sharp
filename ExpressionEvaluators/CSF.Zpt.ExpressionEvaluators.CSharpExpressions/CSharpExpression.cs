using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;
using CSF.Collections;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Represents a compiled CSharp expression, providing a gateway API/wrapper to the generated type.
  /// </summary>
  public class CSharpExpression : IEquatable<CSharpExpression>
  {
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
    /// Gets the expression specification from which the current instance was created.
    /// </summary>
    /// <value>The specification.</value>
    public ExpressionSpecification Specification
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the object which is responsible for creating instances of <see cref="IExpressionHost"/>.
    /// </summary>
    /// <value>The host creator.</value>
    protected IExpressionHostCreator HostCreator
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Evaluate the expression for the given variables.
    /// </summary>
    /// <param name="model">The TALES model.</param>
    public virtual object Evaluate(ITalesModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var variableDefinitions = model.GetAllDefinitions();

      var variablesProvided = VariableSpecification.GetVariableSpecifications(variableDefinitions);
      var variablesExpected = Specification.Variables;

      if(!variablesProvided.AreContentsSameAs(variablesExpected))
      {
        throw new CSharpExpressionException(Resources.ExceptionMessages.DefinedVariablesMustMatch) {
          ExpressionText = this.Specification.Text
        };
      }

      var variablesToSet = (from availableVariable in variableDefinitions
                            from usefulVariable in variablesProvided
                            where availableVariable.Key == usefulVariable.Name
                            select availableVariable)
        .ToArray();

      var host = HostCreator.CreateHostInstance();
      foreach(var kvp in variablesToSet)
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

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpression"/> class.
    /// </summary>
    /// <param name="hostCreator">The expression host creator for the current expression.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="spec">Expression specification.</param>
    public CSharpExpression(int id,
                            ExpressionSpecification spec,
                            IExpressionHostCreator hostCreator)
    {
      if(hostCreator == null)
      {
        throw new ArgumentNullException(nameof(hostCreator));
      }
      if(spec == null)
      {
        throw new ArgumentNullException(nameof(spec));
      }

      Specification = spec;
      HostCreator = hostCreator;
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

