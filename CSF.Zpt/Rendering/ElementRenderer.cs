using System;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Default implementation of <see cref="IElementRenderer"/>.
  /// </summary>
  public class ElementRenderer : IElementRenderer
  {
    /// <summary>
    /// Renders the current document, returning an <see cref="IZptElement"/> representing the rendered result.
    /// </summary>
    /// <returns>The result of the rendering process.</returns>
    /// <param name="model">An object to which the ZPT document is to be applied.</param>
    /// <param name="element">The original element to be rendered.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="IModelValueContainer"/>, to configure it.</param>
    public IZptElement RenderElement(object model,
                                     IZptElement element,
                                     IRenderingSettings options,
                                     Action<IModelValueContainer> contextConfigurator)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      var output = element;
      var context = options.CreateRootContext(output, model);

      ZptConstants.TraceSource.TraceInformation(Resources.LogMessageFormats.RenderingDocument,
                                                (output.SourceFile != null)? output.SourceFile.FullName : "<unknown>",
                                                nameof(ZptDocument),
                                                nameof(RenderElement));

      if(contextConfigurator != null)
      {
        contextConfigurator(context);
      }

      try
      {
        foreach(var visitor in options.ContextVisitors)
        {
          var contexts = visitor.VisitContext(context);

          if(contexts.Count() != 1)
          {
            string message = String.Format(Resources.ExceptionMessages.WrongCountOfReturnedContexts,
                                           typeof(IContextVisitor).Name,
                                           typeof(RenderingContext).Name);
            throw new RenderingException(message);
          }

          context = contexts.Single();
        }
      }
      catch(Exception ex)
      {
        ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Error,
                                            3,
                                            Resources.LogMessageFormats.UnexpectedRenderingException,
                                            nameof(ZptDocument),
                                            nameof(RenderElement),
                                            ex.ToString());
        throw;
      }

      return context.Element;
    }
  }
}

