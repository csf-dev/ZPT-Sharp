﻿using System;
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

        /// <summary>
        /// Gets the document modifier suitable for use with the specified request.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In addition, the <see cref="SourceAnnotationDocumentModifierDecorator"/> is only included
        /// if <see cref="Config.RenderingConfig.IncludeSourceAnnotation"/> is <see langword="true"/>.
        /// </para>
        /// </remarks>
        /// <returns>The document modifier.</returns>
        /// <param name="request">A rendering request.</param>
        public IModifiesDocument GetDocumentModifier(RenderZptDocumentRequest request)
        {
            var useSourceAnnotation = request.Config.IncludeSourceAnnotation;

            var service = GetBaseService();
            // These are commented-out because they are not yet implemented.  When they are,
            // these two lines should be restored.
            //if (useSourceAnnotation) service = WrapWithSourceAnnotationDecorator(service);
            //service = WrapWithTalDecorator(service);
            service = WrapWithMetalDecorator(service);

            return service;
        }

        IModifiesDocument GetBaseService() => new NoOpDocumentModifier();

        IModifiesDocument WrapWithMetalDecorator(IModifiesDocument wrapped)
            => new MetalDocumentModifierDecorator(metalProcessorFactory, iterativeModifier, wrapped);

        IModifiesDocument WrapWithTalDecorator(IModifiesDocument wrapped)
            => new TalDocumentModifierDecorator(talProcessorFactory, iterativeModifier, wrapped);

        IModifiesDocument WrapWithSourceAnnotationDecorator(IModifiesDocument wrapped)
            => new SourceAnnotationDocumentModifierDecorator(sourceAnnotationProcessorFactory, iterativeModifier, wrapped);

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptDocumentModifierFactory"/> class.
        /// </summary>
        /// <param name="metalProcessorFactory">Metal processor factory.</param>
        /// <param name="talProcessorFactory">Tal processor factory.</param>
        /// <param name="sourceAnnotationProcessorFactory">Source annotation processor factory.</param>
        /// <param name="iterativeModifier">Iterative modifier.</param>
        public ZptDocumentModifierFactory(IGetsMetalContextProcessor metalProcessorFactory,
                                          IGetsTalContextProcessor talProcessorFactory,
                                          IGetsSourceAnnotationContextProcessor sourceAnnotationProcessorFactory,
                                          IIterativelyModifiesDocument iterativeModifier)
        {
            this.metalProcessorFactory = metalProcessorFactory ?? throw new ArgumentNullException(nameof(metalProcessorFactory));
            this.talProcessorFactory = talProcessorFactory ?? throw new ArgumentNullException(nameof(talProcessorFactory));
            this.sourceAnnotationProcessorFactory = sourceAnnotationProcessorFactory ?? throw new ArgumentNullException(nameof(sourceAnnotationProcessorFactory));
            this.iterativeModifier = iterativeModifier ?? throw new ArgumentNullException(nameof(iterativeModifier));
        }
    }
}
