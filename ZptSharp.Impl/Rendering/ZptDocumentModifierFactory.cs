using System;
using ZptSharp.Config;
using ZptSharp.Metal;
using ZptSharp.SourceAnnotation;
using ZptSharp.Tal;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// An implementation of <see cref="IGetsDocumentModifier"/> which assembles an instance of
    /// <see cref="IModifiesDocument"/> using the decorator pattern.  Each implementation of the service
    /// wraps another, augmenting its functionality.
    /// </summary>
    public class ZptDocumentModifierFactory : IGetsDocumentModifier
    {
        readonly IGetsMetalContextProcessor metalProcessorFactory;
        readonly IGetsTalContextProcessor talProcessorFactory;
        readonly IGetsSourceAnnotationContextProcessor sourceAnnotationProcessorFactory;
        readonly IIterativelyModifiesDocument iterativeModifier;
        readonly IGetsZptNodeAndAttributeRemovalContextProcessor cleanupProcessorFactory;
        readonly RenderingConfig config;
        readonly IGetsRootExpressionContext rootContextProvider;

        /// <summary>
        /// Gets the document modifier implementation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In addition, the <see cref="SourceAnnotationDocumentModifierDecorator"/> is only included
        /// if <see cref="Config.RenderingConfig.IncludeSourceAnnotation"/> is <see langword="true"/>.
        /// </para>
        /// </remarks>
        /// <returns>The document modifier.</returns>
        public IModifiesDocument GetDocumentModifier()
        {
            var service = GetBaseService();
            service = WrapWithCleanupDecorator(service);
            if (config.IncludeSourceAnnotation)
                service = WrapWithSourceAnnotationDecorator(service);
            service = WrapWithTalDecorator(service);
            service = WrapWithMetalDecorator(service);

            return service;
        }

        /// <summary>
        /// Gets a document modifier implementation which uses an existing rendering context and not a new context created from a document/model pair.
        /// </summary>
        /// <returns>The document modifier.</returns>
        public IModifiesDocumentUsingContext GetDocumentModifierUsingContext()
            => new ContextBasedDocumentModifierAdapter(GetDocumentModifier());


        IModifiesDocument GetBaseService() => new NoOpDocumentModifier();

        IModifiesDocument WrapWithMetalDecorator(IModifiesDocument wrapped)
            => new MetalDocumentModifierDecorator(metalProcessorFactory, iterativeModifier, wrapped, rootContextProvider);

        IModifiesDocument WrapWithTalDecorator(IModifiesDocument wrapped)
            => new TalDocumentModifierDecorator(talProcessorFactory, iterativeModifier, wrapped, rootContextProvider);

        IModifiesDocument WrapWithSourceAnnotationDecorator(IModifiesDocument wrapped)
            => new SourceAnnotationDocumentModifierDecorator(sourceAnnotationProcessorFactory, iterativeModifier, config, wrapped, rootContextProvider);

        IModifiesDocument WrapWithCleanupDecorator(IModifiesDocument wrapped)
            => new RemoveZptAttributesModifierDecorator(cleanupProcessorFactory, iterativeModifier, wrapped, rootContextProvider);

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptDocumentModifierFactory"/> class.
        /// </summary>
        /// <param name="metalProcessorFactory">Metal processor factory.</param>
        /// <param name="talProcessorFactory">Tal processor factory.</param>
        /// <param name="sourceAnnotationProcessorFactory">Source annotation processor factory.</param>
        /// <param name="iterativeModifier">Iterative modifier.</param>
        /// <param name="cleanupProcessorFactory">Cleanup processor factory.</param>
        /// <param name="config">Rendering config.</param>
        /// <param name="rootContextProvider">A provider for the root expression context.</param>
        public ZptDocumentModifierFactory(IGetsMetalContextProcessor metalProcessorFactory,
                                          IGetsTalContextProcessor talProcessorFactory,
                                          IGetsSourceAnnotationContextProcessor sourceAnnotationProcessorFactory,
                                          IIterativelyModifiesDocument iterativeModifier,
                                          IGetsZptNodeAndAttributeRemovalContextProcessor cleanupProcessorFactory,
                                          RenderingConfig config,
                                          IGetsRootExpressionContext rootContextProvider)
        {
            this.metalProcessorFactory = metalProcessorFactory ?? throw new ArgumentNullException(nameof(metalProcessorFactory));
            this.talProcessorFactory = talProcessorFactory ?? throw new ArgumentNullException(nameof(talProcessorFactory));
            this.sourceAnnotationProcessorFactory = sourceAnnotationProcessorFactory ?? throw new ArgumentNullException(nameof(sourceAnnotationProcessorFactory));
            this.iterativeModifier = iterativeModifier ?? throw new ArgumentNullException(nameof(iterativeModifier));
            this.cleanupProcessorFactory = cleanupProcessorFactory ?? throw new ArgumentNullException(nameof(cleanupProcessorFactory));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.rootContextProvider = rootContextProvider ?? throw new ArgumentNullException(nameof(rootContextProvider));
        }
    }
}
