using System;
using System.Collections.Generic;

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
    /// Gets the expression text.
    /// </summary>
    /// <value>The expression text.</value>
    public string ExpressionText
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the property (variable) names associated with the expression.
    /// </summary>
    /// <value>The property names.</value>
    public string[] PropertyNames
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

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.ExpressionModel"/> class.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="text">Text.</param>
    /// <param name="properties">Properties.</param>
    public ExpressionModel(int id, string text, string[] properties)
    {
      if(text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if(properties == null)
      {
        throw new ArgumentNullException(nameof(properties));
      }

      ExpressionId = id;
      ExpressionText = text;
      PropertyNames = properties;
    }

    #endregion
  }
}

