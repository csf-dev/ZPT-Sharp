using System;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;

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
      var hostCreator = GetExpressionHostCreator(assembly);

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

      var type = assembly.GetType(model.GetClassName());
      if(type == null)
      {
        // TODO: Improve this exception and put a message in a res file
        throw new InvalidOperationException();
      }

      return () => (IExpressionHost) Activator.CreateInstance(type);
    }

    private Assembly CompileHostAssembly(ExpressionModel model)
    {
      CSharpCodeProvider provider = new CSharpCodeProvider();
      CompilerParameters parameters = new CompilerParameters();

      var frameworkAssemblyName = String.Format("{0}.dll", typeof(IExpressionHost).Namespace);
      parameters.ReferencedAssemblies.Add(frameworkAssemblyName);

      parameters.GenerateInMemory = true;
      parameters.GenerateExecutable = false;

      var code = GetExpressionHostCode(model);

      var result = provider.CompileAssemblyFromSource(parameters, code);

      if(result.Errors.Count > 0)
      {
        // TODO: Improve this exception and put a message in a res file
        throw new InvalidOperationException();
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

