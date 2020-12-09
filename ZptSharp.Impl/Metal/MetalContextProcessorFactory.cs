using System;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IGetsMetalContextProcessor"/> which gets the
    /// expression-context processor for METAL.
    /// </summary>
    public class MetalContextProcessorFactory : IGetsMetalContextProcessor
    {
        readonly IGetsMetalAttributeSpecs specProvider;
        readonly IGetsMacro macroProvider;
        readonly IExpandsMacro macroExpander;
        readonly ISearchesForAttributes attributeFinder;
        readonly Microsoft.Extensions.Logging.ILogger<MacroUsageContextProcessor> logger;
        readonly IReplacesNode replacer;

        /// <summary>
        /// Gets the METAL context processor.
        /// </summary>
        /// <returns>The METAL context processor.</returns>
        public IProcessesExpressionContext GetMetalContextProcessor()
        {
            var service = GetMacroUsageContextProcessor();
            service = GetMacroAddingDecorator(service);
            return service;
        }

        IProcessesExpressionContext GetMacroUsageContextProcessor()
            => new MacroUsageContextProcessor(specProvider, macroProvider, macroExpander, logger, replacer);

        IProcessesExpressionContext GetMacroAddingDecorator(IProcessesExpressionContext service)
            => new AddDefinedMacroToGlobalScopeProcessorDecorator(specProvider, service, attributeFinder);

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalContextProcessorFactory"/> class.
        /// </summary>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="macroProvider">Macro provider.</param>
        /// <param name="macroExpander">Macro expander.</param>
        /// <param name="attributeFinder">An attribute finder.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="replacer">The node replacer.</param>
        public MetalContextProcessorFactory(IGetsMetalAttributeSpecs specProvider,
                                            IGetsMacro macroProvider,
                                            IExpandsMacro macroExpander,
                                            ISearchesForAttributes attributeFinder,
                                            Microsoft.Extensions.Logging.ILogger<MacroUsageContextProcessor> logger,
                                            IReplacesNode replacer)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.macroProvider = macroProvider ?? throw new ArgumentNullException(nameof(macroProvider));
            this.macroExpander = macroExpander ?? throw new ArgumentNullException(nameof(macroExpander));
            this.attributeFinder = attributeFinder ?? throw new ArgumentNullException(nameof(attributeFinder));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.replacer = replacer ?? throw new ArgumentNullException(nameof(replacer));
        }
    }
}
