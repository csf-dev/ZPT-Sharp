using System;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class CSharpExpressionFactory : ICSharpExpressionFactory
  {
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

    private string GetExpressionHostCode(ExpressionModel model)
    {
      var builder = new ExpressionHostBuilder(model);
      return builder.TransformText();
    }
  }
}

