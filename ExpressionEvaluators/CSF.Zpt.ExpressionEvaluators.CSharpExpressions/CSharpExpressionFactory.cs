using System;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;
using System.Linq;
using CSF.Configuration;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Default implementation of <see cref="ICSharpExpressionFactory"/>.
  /// </summary>
  public class CSharpExpressionFactory : ICSharpExpressionFactory
  {
    #region fields

    private IExpressionHostCompiler _hostCreatorFactory;

    #endregion

    #region methods

    /// <summary>
    /// Creates an expression from the given information
    /// </summary>
    /// <param name="model">Information representing the creation of a CSharp expression.</param>
    public CSharpExpression Create(ExpressionModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var hostCreator = _hostCreatorFactory.GetHostCreator(model);

      return new CSharpExpression(model.ExpressionId,
                                  model.Specification,
                                  hostCreator);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpressionFactory"/> class.
    /// </summary>
    /// <param name="hostCreatorFactory">Host creator factory.</param>
    public CSharpExpressionFactory(IExpressionHostCompiler hostCreatorFactory = null)
    {
      _hostCreatorFactory = hostCreatorFactory?? new ExpressionHostCompiler();
    }

    #endregion
  }
}

