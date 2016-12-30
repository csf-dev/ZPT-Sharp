using System;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host
{
  /// <summary>
  /// Default implementation of <see cref="IExpressionHostCompiler"/>.
  /// </summary>
  public class ExpressionHostCompiler : IExpressionHostCompiler
  {
    #region constants

    private static readonly string
      SYSTEM_CORE_ASSEMBLY_NAME                   = "System.Core",
      CSHARP_EXPRESSION_FRAMEWORK_ASSEMBLY_NAME   = typeof(IExpressionHost).Namespace,
      CODEDOM_ASSEMBLY_NAME                       = typeof(CSharpCodeProvider).Namespace,
      DLL_SUFFIX                                  = ".dll";

    /// <summary>
    /// A collection of mandatory assemblies which must always be referenced.
    /// </summary>
    private static readonly string[] MandatoryAssemblyReferences = new [] {
      String.Concat(SYSTEM_CORE_ASSEMBLY_NAME,                    DLL_SUFFIX),
      String.Concat(CSHARP_EXPRESSION_FRAMEWORK_ASSEMBLY_NAME,    DLL_SUFFIX),
      String.Concat(CODEDOM_ASSEMBLY_NAME,                        DLL_SUFFIX),
    };

    #endregion

    #region methods

    /// <summary>
    /// Creates an expression from the given information
    /// </summary>
    /// <param name="model">Information representing the creation of a CSharp expression.</param>
    public IExpressionHostCreator GetHostCreator(ExpressionModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var assembly = CompileHostAssembly(model);
      return GetExpressionHostCreator(assembly, model);
    }

    /// <summary>
    /// Gets a creation method which instantiates the <see cref="IExpressionHost"/> instance for a given model.
    /// </summary>
    /// <returns>The expression host creator.</returns>
    /// <param name="assembly">Assembly.</param>
    /// <param name="model">Model.</param>
    private IExpressionHostCreator GetExpressionHostCreator(Assembly assembly, ExpressionModel model)
    {
      if(assembly == null)
      {
        throw new ArgumentNullException(nameof(assembly));
      }

      var type = assembly.GetType(model.GetClassFullName());
      if(type == null)
      {
        throw new CSharpExpressionException(Resources.ExceptionMessages.ExpressionTypeMustExist) {
          ExpressionText = model.Specification.Text
        };
      }

      return new ExpressionHostCreator(assembly, () => (IExpressionHost) Activator.CreateInstance(type));
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

      var references = GetAssemblyReferences(model);
      AddReferencedAssemblies(references, parameters);

      parameters.GenerateInMemory = true;
      parameters.GenerateExecutable = false;

      var code = GetExpressionHostCode(model);

      var result = provider.CompileAssemblyFromSource(parameters, code);
      CheckNoCompilerErrors(result, model, code);

      return result.CompiledAssembly;
    }

    /// <summary>
    /// Gets a collection of the names of referenced assemblies.
    /// </summary>
    /// <returns>The assembly references.</returns>
    /// <param name="model">Model.</param>
    private IEnumerable<string> GetAssemblyReferences(ExpressionModel model)
    {
      return model.Specification.Assemblies
        .Select(x => x.Name)
        .Union(MandatoryAssemblyReferences)
        .Distinct()
        .ToArray();
    }

    /// <summary>
    /// Adds each of the referenced assembly names into the compiler parameters.
    /// </summary>
    /// <param name="assemblyNames">Assembly names.</param>
    /// <param name="parameters">Parameters.</param>
    private void AddReferencedAssemblies(IEnumerable<string> assemblyNames, CompilerParameters parameters)
    {
      foreach(var name in assemblyNames)
      {
        parameters.ReferencedAssemblies.Add(name);
      }
    }

    /// <summary>
    /// Gets the code to compile for the generated expression host type.
    /// </summary>
    /// <returns>The expression host code.</returns>
    /// <param name="model">Model.</param>
    private string GetExpressionHostCode(ExpressionModel model)
    {
      var builder = new ExpressionHostTemplate(model);
      return builder.TransformText();
    }

    /// <summary>
    /// Checks that the compilation result does not contain any errors, and raises an exception if they do.
    /// </summary>
    /// <param name="result">Result.</param>
    /// <param name="model">Model.</param>
    /// <param name="code">The code which was to be compiled.</param>
    private void CheckNoCompilerErrors(CompilerResults result, ExpressionModel model, string code)
    {
      var compilerErrors = result.Errors.Cast<CompilerError>();

      if(compilerErrors.Any(x => !x.IsWarning))
      {
        foreach(var error in compilerErrors.Where(x => !x.IsWarning))
        {
          ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Warning,
                                              5,
                                              Resources.LogFormats.CompilerErrorFormat,
                                              error,
                                              model.Specification.Text);
        }

        ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Information,
                                            5,
                                            Resources.LogFormats.CodeWhichCausedCompileErrorFormat,
                                            code);

        throw new CSharpExpressionException(Resources.ExceptionMessages.MustNotBeCompileErrors) {
          ExpressionText = model.Specification.Text
        };
      }
      else
      {
        ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Verbose,
                                            5,
                                            Resources.LogFormats.CodeToBeCompiledFormat,
                                            code);
      }
    }

    #endregion
  }
}

