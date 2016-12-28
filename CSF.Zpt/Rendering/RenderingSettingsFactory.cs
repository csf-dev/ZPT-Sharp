using System;
using CSF.Zpt.Tales;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Default implementation of <see cref="IRenderingSettingsFactory"/>.
  /// </summary>
  public class RenderingSettingsFactory : IRenderingSettingsFactory
  {
    #region constants

    private static readonly Type[] DEFAULT_VISITOR_TYPES = new Type[] {
      typeof(CSF.Zpt.SourceAnnotation.SourceInfoBurnInVisitor),
      typeof(CSF.Zpt.Metal.MetalVisitor),
      typeof(CSF.Zpt.Tal.TalVisitor),
      typeof(CSF.Zpt.Metal.MetalTidyUpVisitor),
      typeof(CSF.Zpt.Tal.TalTidyUpVisitor),
      typeof(CSF.Zpt.SourceAnnotation.SourceInfoTidyUpVisitor),
    };

    /// <summary>
    /// Gets the default value for <see cref="IRenderingSettings.AddSourceFileAnnotation"/>.
    /// </summary>
    private const bool DefaultAddAnnotation = false;

    /// <summary>
    /// Gets the default value for <see cref="IRenderingSettings.OmitXmlDeclaration"/>.
    /// </summary>
    private const bool DefaultOmitXmlDeclaration = false;

    private const char TYPE_NAMES_SEPARATOR = ';';

    #endregion

    #region methods

    /// <summary>
    /// Creates the settings from the given rendering options.
    /// </summary>
    /// <returns>The settings.</returns>
    /// <param name="options">Options.</param>
    public IRenderingSettings CreateSettings(IRenderingOptions options)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      IRenderingContextFactory contextFactory;
      if(!String.IsNullOrEmpty(options.RenderingContextFactoryType))
      {
        contextFactory = GetContextFactory(options.RenderingContextFactoryType);
      }
      else
      {
        contextFactory = GetContextFactory();
      }

      foreach(var keywordOption in options.KeywordOptions)
      {
        contextFactory.AddKeywordOption(keywordOption.Key, keywordOption.Value);
      }

      ITemplateFileFactory templateFactory;
      if(!String.IsNullOrEmpty(options.DocumentFactoryType))
      {
        templateFactory = GetTemplateFactory(options.DocumentFactoryType);
      }
      else
      {
        templateFactory = GetTemplateFactory();
      }

      IContextVisitor[] contextVisitors;
      if(!String.IsNullOrEmpty(options.ContextVisitorTypes))
      {
        contextVisitors = GetContextVisitors(options.ContextVisitorTypes);
      }
      else
      {
        contextVisitors = GetContextVisitors();
      }

      System.Text.Encoding encoding;
      if(!String.IsNullOrEmpty(options.OutputEncodingName))
      {
        encoding = GetEncoding(options.OutputEncodingName);
      }
      else
      {
        encoding = GetEncoding();
      }

      return new RenderingSettings(contextVisitors,
                                   contextFactory,
                                   options.AddSourceFileAnnotation,
                                   encoding,
                                   options.OmitXmlDeclaration,
                                   templateFactory);
    }

    /// <summary>
    /// Creates rendering settings from specified values, using defaults where values are blank.
    /// </summary>
    /// <returns>The settings.</returns>
    /// <param name="contextVisitors">Context visitors.</param>
    /// <param name="contextFactory">Context factory.</param>
    /// <param name="addSourceAnnotation">If set to <c>true</c> add source annotation.</param>
    /// <param name="encoding">Encoding.</param>
    /// <param name="omitXmlDeclaration">If set to <c>true</c> omit xml declaration.</param>
    /// <param name="templateFactory">Template factory.</param>
    public IRenderingSettings CreateSettings(IContextVisitor[] contextVisitors = null,
                                             IRenderingContextFactory contextFactory = null,
                                             bool addSourceAnnotation = DefaultAddAnnotation,
                                             System.Text.Encoding encoding = null,
                                             bool omitXmlDeclaration = DefaultOmitXmlDeclaration,
                                             ITemplateFileFactory templateFactory = null)
    {
      return new RenderingSettings(contextVisitors?? GetContextVisitors(),
                                   contextFactory?? GetContextFactory(),
                                   addSourceAnnotation,
                                   encoding?? GetEncoding(),
                                   omitXmlDeclaration,
                                   templateFactory?? GetTemplateFactory());
    }

    /// <summary>
    /// Gets a <see cref="IRenderingSettings"/> with default values.
    /// </summary>
    /// <returns>The default settings.</returns>
    internal static IRenderingSettings GetDefaultSettings()
    {
      return new RenderingSettings(GetContextVisitors(),
                                   GetContextFactory(),
                                   false,
                                   GetEncoding(),
                                   false,
                                   GetTemplateFactory());
    }

    private static IRenderingContextFactory GetContextFactory()
    {
      return new TalesRenderingContextFactory();
    }

    private IRenderingContextFactory GetContextFactory(string typeName)
    {
      return GetImplementation<IRenderingContextFactory>(typeName);
    }

    private static ITemplateFileFactory GetTemplateFactory()
    {
      return new ZptDocumentFactory();
    }

    private ITemplateFileFactory GetTemplateFactory(string typeName)
    {
      return GetImplementation<ITemplateFileFactory>(typeName);
    }

    private static IContextVisitor[] GetContextVisitors()
    {
      return GetDefaultContextVisitorTypes()
        .Select(x => Activator.CreateInstance(x))
        .Cast<IContextVisitor>()
        .ToArray();
    }

    private IContextVisitor[] GetContextVisitors(string typeNames)
    {
      if(typeNames == null)
      {
        throw new ArgumentNullException(nameof(typeNames));
      }

      return typeNames
        .Split(TYPE_NAMES_SEPARATOR)
        .Select(x => GetImplementation<IContextVisitor>(x))
        .ToArray();
    }

    private static System.Text.Encoding GetEncoding()
    {
      return System.Text.Encoding.UTF8;
    }

    private System.Text.Encoding GetEncoding(string encodingName)
    {
      return System.Text.Encoding.GetEncoding(encodingName);
    }

    private static TImplementation GetImplementation<TImplementation>(string typeName)
    {
      if(typeName == null)
      {
        throw new ArgumentNullException(nameof(typeName));
      }

      var type = Type.GetType(typeName);
      if(type == null)
      {
        // TODO: Move this message to a resource file and improve it.
        string message = String.Format("The implementation type `{1}', for `{0}' was not found.",
                                       typeof(TImplementation).Name,
                                       typeName);
        throw new InvalidOperationException(message);
      }

      return (TImplementation) Activator.CreateInstance(type);
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a collection of <c>System.Type</c> representing the default <see cref="IContextVisitor"/> implementations
    /// which visit rendering contexts.
    /// </summary>
    /// <returns>The default context visitor types.</returns>
    public static Type[] GetDefaultContextVisitorTypes()
    {
      var output = new Type[DEFAULT_VISITOR_TYPES.Length];
      Array.Copy(DEFAULT_VISITOR_TYPES, output, DEFAULT_VISITOR_TYPES.Length);
      return output;
    }

    #endregion
  }
}

