using System;
using System.Reflection;
using CSF.Zpt.Tales;
using CSF.Zpt.Resources;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Type which creates instances of <see cref="IRenderingContextFactory"/> via reflection.
  /// </summary>
  public class RenderingContextFactoryFactory : IRenderingContextFactoryFactory
  {
    /// <summary>
    /// Create the factory from a fully-qualified type name.
    /// </summary>
    /// <param name="className">The class name for the desired factory instance.</param>
    public IRenderingContextFactory Create(string className)
    {
      IRenderingContextFactory output;
      Type outputType;

      if(String.IsNullOrEmpty(className))
      {
        output = new TalesRenderingContextFactory();
      }
      else if((outputType = Type.GetType(className)) != null
              && typeof(IRenderingContextFactory).IsAssignableFrom(outputType))
      {
        output = (IRenderingContextFactory) Activator.CreateInstance(outputType);
      }
      else
      {
        string message = String.Format(ExceptionMessages.CouldNotInstantiateTypeFormat,
                                       typeof(IRenderingContextFactory).Name,
                                       className);
        throw new CouldNotCreateRenderingContextFactoryException(message) {
          InvalidClassname = className
        };
      }

      return output;
    }
  }
}

