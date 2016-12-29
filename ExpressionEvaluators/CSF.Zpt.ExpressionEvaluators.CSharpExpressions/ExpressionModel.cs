using System;
using System.Collections.Generic;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Represents information about a CSharp expression.
  /// </summary>
  public class ExpressionModel
  {
    #region constants

    private const string DynamicCodeNamespace = "CSF.Zpt.DynamicCSharpExpressions";

    #endregion

    #region properties

    /// <summary>
    /// Gets the expression identifier.
    /// </summary>
    /// <value>The expression identifier.</value>
    public int ExpressionId
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the specification from which the current instance was created.
    /// </summary>
    /// <value>The expression specification.</value>
    public ExpressionSpecification Specification
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the namespace for the generated expression type.
    /// </summary>
    /// <value>The namespace.</value>
    public string Namespace
    {
      get {
        return DynamicCodeNamespace;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the name of the class for the generated type.
    /// </summary>
    /// <returns>The class name.</returns>
    public string GetClassName()
    {
      return String.Format("ExpressionHost_{0}", ExpressionId);
    }

    /// <summary>
    /// Gets the full (namespace-qualified) name for the generated type.
    /// </summary>
    /// <returns>The class full name.</returns>
    public string GetClassFullName()
    {
      return String.Concat(Namespace, ".", GetClassName());
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.ExpressionModel"/> class.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="spec">Expression specification.</param>
    public ExpressionModel(int id, ExpressionSpecification spec)
    {
      if(spec == null)
      {
        throw new ArgumentNullException(nameof(spec));
      }

      ExpressionId = id;
      Specification = spec;
    }

    #endregion
  }
}

