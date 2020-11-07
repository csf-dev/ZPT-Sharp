using System;
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

        /// <summary>
        /// Gets the METAL context processor.
        /// </summary>
        /// <returns>The METAL context processor.</returns>
        public IProcessesExpressionContext GetMetalContextProcessor()
        {
            var service = GetMacroUsageContextProcessor();
            service = GetAddDefinedMacroToGlobalScopeProcessorDecorator(service);
            return service;
        }

        IProcessesExpressionContext GetMacroUsageContextProcessor()
            => new MacroUsageContextProcessor(specProvider, macroProvider, macroExpander);

        IProcessesExpressionContext GetAddDefinedMacroToGlobalScopeProcessorDecorator(IProcessesExpressionContext wrapped)
            => new AddDefinedMacroToGlobalScopeProcessorDecorator(specProvider, wrapped);

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalContextProcessorFactory"/> class.
        /// </summary>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="macroProvider">Macro provider.</param>
        /// <param name="macroExpander">Macro expander.</param>
        public MetalContextProcessorFactory(IGetsMetalAttributeSpecs specProvider,
                                            IGetsMacro macroProvider,
                                            IExpandsMacro macroExpander)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.macroProvider = macroProvider ?? throw new ArgumentNullException(nameof(macroProvider));
            this.macroExpander = macroExpander ?? throw new ArgumentNullException(nameof(macroExpander));
        }
    }
}
