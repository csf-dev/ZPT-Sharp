using System;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Default implementation of <see cref="ICSharpExpressionFactory"/>.
  /// </summary>
  public class CSharpExpressionFactory : ICSharpExpressionFactory
  {
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

      var assembly = CompileHostAssembly(model);
      var hostCreator = GetExpressionHostCreator(assembly, model);

      return new CSharpExpression(hostCreator,
                                  model.ExpressionId,
                                  model.ExpressionText,
                                  model.PropertyNames,
                                  assembly);
    }

    /// <summary>
    /// Gets a creation method which instantiates the <see cref="IExpressionHost"/> instance for a given model.
    /// </summary>
    /// <returns>The expression host creator.</returns>
    /// <param name="assembly">Assembly.</param>
    /// <param name="model">Model.</param>
    private Func<IExpressionHost> GetExpressionHostCreator(Assembly assembly, ExpressionModel model)
    {
      if(assembly == null)
      {
        throw new ArgumentNullException(nameof(assembly));
      }

      var type = assembly.GetType(String.Concat(model.Namespace, ".", model.GetClassName()));
      if(type == null)
      {
        throw new CSharpExpressionExceptionException(Resources.ExceptionMessages.ExpressionTypeMustExist) {
          ExpressionText = model.ExpressionText
        };
      }

      return () => (IExpressionHost) Activator.CreateInstance(type);
    }

    /// <summary>
    /// Generates and compiles an <c>System.Reflection.Assembly</c> containing an implementation of
    /// <see cref="IExpressionHost"/>, from the given expression information.
    /// </summary>
    /// <returns>The generated host assembly.</returns>
    /// <param name="model">Model.</param>
    private Assembly CompileHostAssembly(ExpressionModel model)
    {
      CSharpCodeProvider provider = new CSharpCodeProvider();
      CompilerParameters parameters = new CompilerParameters();

      var frameworkAssemblyName = String.Format("{0}.dll", typeof(IExpressionHost).Namespace);
      var csAssemblyName = String.Format("{0}.dll", typeof (Microsoft.CSharp.CSharpCodeProvider).Namespace);
      var dynAssemblyName = String.Format("System.Core.dll");
      parameters.ReferencedAssemblies.Add(frameworkAssemblyName);
      parameters.ReferencedAssemblies.Add(csAssemblyName);
      parameters.ReferencedAssemblies.Add(dynAssemblyName);

      parameters.GenerateInMemory = true;
      parameters.GenerateExecutable = false;

      var code = GetExpressionHostCode(model);

      var result = provider.CompileAssemblyFromSource(parameters, code);
      var compilerErrors = result.Errors.Cast<CompilerError>();

      if(compilerErrors.Any(x => !x.IsWarning))
      {
        var allErrorTexts = compilerErrors
          .Select(x => String.Format("{0} - {2}:{1}", x.Line, x.ErrorText, x.ErrorNumber));
        
        Console.Error.Write(String.Join(System.Environment.NewLine, allErrorTexts));

        throw new CSharpExpressionExceptionException(Resources.ExceptionMessages.MustNotBeCompileErrors) {
          ExpressionText = model.ExpressionText
        };
      }

      return result.CompiledAssembly;
    }

    /// <summary>
    /// Gets the code to compile for the generated expression host type.
    /// </summary>
    /// <returns>The expression host code.</returns>
    /// <param name="model">Model.</param>
    private string GetExpressionHostCode(ExpressionModel model)
    {
      var builder = new ExpressionHostBuilder(model);
      return builder.TransformText();
    }
  }
}

