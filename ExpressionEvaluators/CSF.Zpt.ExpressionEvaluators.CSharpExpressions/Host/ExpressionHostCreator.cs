using System;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host
{
  /// <summary>
  /// Default concrete implementation of <see cref="IExpressionHostCreator"/>, which contains generated code.
  /// </summary>
  public class ExpressionHostCreator : IExpressionHostCreator
  {
    #region fields

    private Func<IExpressionHost> _hostCreator;

    #endregion

    #region properties

    /// <summary>
    /// Gets the assembly.
    /// </summary>
    /// <value>The assembly.</value>
    public Assembly Assembly
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Creates and returns the host instance.
    /// </summary>
    /// <returns>The host instance.</returns>
    public IExpressionHost CreateHostInstance()
    {
      return _hostCreator();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host.ExpressionHostCreator"/> class.
    /// </summary>
    /// <param name="assembly">Assembly.</param>
    /// <param name="hostCreator">A function which creates an expression host instance.</param>
    public ExpressionHostCreator(Assembly assembly, Func<IExpressionHost> hostCreator)
    {
      if(assembly == null)
      {
        throw new ArgumentNullException(nameof(assembly));
      }
      if(hostCreator == null)
      {
        throw new ArgumentNullException(nameof(hostCreator));
      }

      Assembly = assembly;
      _hostCreator = hostCreator;
    }

    #endregion
  }
}

