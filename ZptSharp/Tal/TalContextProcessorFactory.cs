using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IGetsTalContextProcessor"/> which returns
    /// a context processor suitable for processing the TAL model-binding syntax.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This factory assembles a service which uses the decorator pattern.  Each individual attribute
    /// is handled by a separate decorator class (which handles just that attribute).  At the centre
    /// of the decorator stack is a no-op service.
    /// </para>
    /// <para>
    /// The <see cref="OnErrorAttributeDecorator"/> is special, in that it uses a <c>try/catch</c>
    /// construct to intercept any errors which occur in other decorators which are 'deeper' in the stack.
    /// </para>
    /// </remarks>
    public class TalContextProcessorFactory : IGetsTalContextProcessor
    {
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IEvaluatesExpression evaluator;
        readonly IInterpretsExpressionResult resultInterpreter;
        readonly IGetsVariableDefinitionsFromAttributeValue definitionProvider;
        readonly IEvaluatesDomValueExpression domEvaluator;
        readonly Microsoft.Extensions.Logging.ILogger<OnErrorAttributeDecorator> onErrorlogger;
        readonly IReplacesNode replacer;
        readonly IOmitsNode omitter;
        readonly IGetsRepetitionContexts repetitionContextProvider;
        readonly IGetsAttributeDefinitions attributeDefinitionsProvider;
        readonly Metal.IGetsMetalAttributeSpecs metalSpecProvider;
        private readonly Microsoft.Extensions.Logging.ILogger<ContentOrReplaceAttributeDecorator> contentReplaceLogger;

        /// <summary>
        /// Gets the TAL context processor.
        /// </summary>
        /// <returns>The TAL context processor.</returns>
        public IHandlesProcessingError GetTalContextProcessor()
        {
            var service = GetNoOpService();
            service = GetOmitTagDecorator(service);
            service = GetAttributesDecorator(service);
            service = GetContentOrReplaceDecorator(service);
            service = GetRepeatDecorator(service);
            service = GetConditionDecorator(service);
            service = GetDefineDecorator(service);
            service = GetOnErrorDecorator(service);
            service = GetMacroNameDecorator(service);

            return service;
        }

        IHandlesProcessingError GetNoOpService()
            => new NoOpTalContextProcessor();

        IHandlesProcessingError GetOnErrorDecorator(IHandlesProcessingError service)
            => new OnErrorAttributeDecorator(service, specProvider, domEvaluator, onErrorlogger);

        IHandlesProcessingError GetOmitTagDecorator(IHandlesProcessingError service)
            => new OmitTagAttributeDecorator(service, specProvider, omitter, evaluator, resultInterpreter);

        IHandlesProcessingError GetAttributesDecorator(IHandlesProcessingError service)
            => new AttributesAttributeDecorator(service, specProvider, evaluator, resultInterpreter, attributeDefinitionsProvider);

        IHandlesProcessingError GetContentOrReplaceDecorator(IHandlesProcessingError service)
            => new ContentOrReplaceAttributeDecorator(service, specProvider, domEvaluator, replacer, omitter, contentReplaceLogger);

        IHandlesProcessingError GetRepeatDecorator(IHandlesProcessingError service)
            => new RepeatAttributeDecorator(service, specProvider, evaluator, resultInterpreter, repetitionContextProvider);

        IHandlesProcessingError GetConditionDecorator(IHandlesProcessingError service)
            => new ConditionAttributeDecorator(service, specProvider, evaluator, resultInterpreter);

        IHandlesProcessingError GetDefineDecorator(IHandlesProcessingError service)
            => new DefineAttributeDecorator(service, specProvider, evaluator, resultInterpreter, definitionProvider);

        IHandlesProcessingError GetMacroNameDecorator(IHandlesProcessingError service)
            => new RecordMetalMacroNameDecorator(service, metalSpecProvider);

        /// <summary>
        /// Initializes a new instance of the <see cref="TalContextProcessorFactory"/> class.
        /// </summary>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="resultInterpreter">Result interpreter.</param>
        /// <param name="definitionProvider">Definition provider.</param>
        /// <param name="domEvaluator">DOM evaluator.</param>
        /// <param name="onErrorlogger">On errorlogger.</param>
        /// <param name="replacer">Replacer.</param>
        /// <param name="omitter">Omitter.</param>
        /// <param name="repetitionContextProvider">Repetition context provider.</param>
        /// <param name="attributeDefinitionsProvider">The 'attributes' attribute definitions provider.</param>
        /// <param name="metalSpecProvider">METAL attribute spec provider.</param>
        /// <param name="contentReplaceLogger">Content or replace logger.</param>
        public TalContextProcessorFactory(IGetsTalAttributeSpecs specProvider,
                                          IEvaluatesExpression evaluator,
                                          IInterpretsExpressionResult resultInterpreter,
                                          IGetsVariableDefinitionsFromAttributeValue definitionProvider,
                                          IEvaluatesDomValueExpression domEvaluator,
                                          Microsoft.Extensions.Logging.ILogger<OnErrorAttributeDecorator> onErrorlogger,
                                          IReplacesNode replacer,
                                          IOmitsNode omitter,
                                          IGetsRepetitionContexts repetitionContextProvider,
                                          IGetsAttributeDefinitions attributeDefinitionsProvider,
                                          Metal.IGetsMetalAttributeSpecs metalSpecProvider,
                                          Microsoft.Extensions.Logging.ILogger<ContentOrReplaceAttributeDecorator> contentReplaceLogger)
        {
            this.specProvider = specProvider ?? throw new System.ArgumentNullException(nameof(specProvider));
            this.evaluator = evaluator ?? throw new System.ArgumentNullException(nameof(evaluator));
            this.resultInterpreter = resultInterpreter ?? throw new System.ArgumentNullException(nameof(resultInterpreter));
            this.definitionProvider = definitionProvider ?? throw new System.ArgumentNullException(nameof(definitionProvider));
            this.domEvaluator = domEvaluator ?? throw new System.ArgumentNullException(nameof(domEvaluator));
            this.onErrorlogger = onErrorlogger ?? throw new System.ArgumentNullException(nameof(onErrorlogger));
            this.replacer = replacer ?? throw new System.ArgumentNullException(nameof(replacer));
            this.omitter = omitter ?? throw new System.ArgumentNullException(nameof(omitter));
            this.repetitionContextProvider = repetitionContextProvider ?? throw new System.ArgumentNullException(nameof(repetitionContextProvider));
            this.attributeDefinitionsProvider = attributeDefinitionsProvider ?? throw new System.ArgumentNullException(nameof(attributeDefinitionsProvider));
            this.metalSpecProvider = metalSpecProvider ?? throw new System.ArgumentNullException(nameof(metalSpecProvider));
            this.contentReplaceLogger = contentReplaceLogger ?? throw new System.ArgumentNullException(nameof(contentReplaceLogger));
        }
    }
}
