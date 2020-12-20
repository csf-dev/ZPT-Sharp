using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Metal;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Provides the built-in contexts which are inherent to the TALES specification.
    /// These are generally accessible as if they were root objects within a path expression.
    /// If, for example, their names have been overwritten they may alternatively be referenced
    /// explicitly with the <c>CONTEXTS</c> reserved variable name.
    /// </summary>
    public class BuiltinContextsProvider : IGetsDictionaryOfNamedTalesValues
    {
        readonly ExpressionContext context;
        readonly RenderingConfig config;
        readonly IGetsMetalDocumentAdapter metalDocumentAdapterFactory;

        /// <summary>
        /// An identifier for the keyword-options presented to the rendering process.
        /// </summary>
        public static readonly string Options = "options";

        /// <summary>
        /// An identifier for the collection of named repetition-information objects available in the
        /// expression context.
        /// </summary>
        public static readonly string Repeat = "repeat";

        /// <summary>
        /// An identifier/alias for the model object contained within the expression context.
        /// </summary>
        public static readonly string Here = "here";

        /// <summary>
        /// An identifier/alias for a non-object.  This translates to <c>null</c> in C# applications.
        /// </summary>
        public static readonly string Nothing = "nothing";

        /// <summary>
        /// An identifier/alias for an object which indicates that the current action should be cancelled.
        /// </summary>
        public static readonly string Default = "default";

        /// <summary>
        /// An identifier/alias for getting the attributes from the current <see cref="Dom.INode"/>.
        /// </summary>
        public static readonly string Attributes = "attrs";

        /// <summary>
        /// An identifier/alias for getting the <see cref="Dom.IDocument"/> 
        /// </summary>
        public static readonly string Template = "template";

        /// <summary>
        /// An identifier/alias for getting the container of the current <see cref="Dom.IDocument"/>.
        /// </summary>
        public static readonly string Container = "container";

        /// <summary>
        /// An identifier/alias for getting an error object which was encountered whilst processing a context.
        /// </summary>
        public static readonly string Error = "error";

        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, CancellationToken cancellationToken = default)
        {
            if (BuiltinContextsAndValues.TryGetValue(name, out var valueFunc))
                return Task.FromResult(GetValueResult.For(valueFunc()));

            return Task.FromResult(GetValueResult.Failure);
        }

        /// <summary>
        /// Gets a dictionary of every available named TALES value, exposed by the current instance.
        /// </summary>
        /// <returns>The named values.</returns>
        public Task<IDictionary<string, object>> GetAllNamedValues()
        {
            IDictionary<string, object> values = BuiltinContextsAndValues.ToDictionary(k => k.Key, v => v.Value());
            return Task.FromResult(values);
        }

        Dictionary<string,Func<object>> BuiltinContextsAndValues
        {
            get
            {
                var output = new Dictionary<string, Func<object>>
                {
                    { Here, () => context.Model },
                    { Repeat, () => context.Repetitions },
                    { Options, () => config.KeywordOptions},
                    { Nothing, () => null },
                    { Default, () => AbortZptActionToken.Instance },
                    { Attributes, GetAttributesDictionary },
                    { Template, GetMetalDocumentAdapter },
                    { Container, GetTemplateContainer },
                };

                if (context.Error != null)
                    output[Error] = () => context.Error;

                return output;
            }
        }

        IDictionary<string, Dom.IAttribute> GetAttributesDictionary()
            => context.CurrentNode.Attributes.ToDictionary(k => k.Name, v => v);

        MetalDocumentAdapter GetMetalDocumentAdapter()
            => metalDocumentAdapterFactory.GetMetalDocumentAdapter(context.TemplateDocument);

        object GetTemplateContainer()
        {
            return (context.TemplateDocument.SourceInfo is Rendering.IHasContainer containerProvider)
                ? containerProvider.GetContainer()
                : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuiltinContextsProvider"/> class.
        /// </summary>
        /// <param name="context">The rendering context.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="metalDocumentAdapterFactory">A METAL document adapter factory.</param>
        public BuiltinContextsProvider(ExpressionContext context,
                                       RenderingConfig config,
                                       IGetsMetalDocumentAdapter metalDocumentAdapterFactory)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            this.config = config ?? throw new System.ArgumentNullException(nameof(config));
            this.metalDocumentAdapterFactory = metalDocumentAdapterFactory ?? throw new ArgumentNullException(nameof(metalDocumentAdapterFactory));
        }
    }
}
