﻿using System;
using System.Collections.Generic;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    public class BuiltinContextsProvider : IGetsNamedTalesValue
    {
        readonly ExpressionContext context;
        readonly RenderingConfig config;

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
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>A boolean indicating whether a value was successfully retrieved or not.</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="value">Exposes the retrieved value if this method returns success.</param>
        public bool TryGetValue(string name, out object value)
        {
            if(BuiltinContextsAndValues.TryGetValue(name, out var valueFunc))
            {
                value = valueFunc();
                return true;
            }

            value = null;
            return false;
        }

        Dictionary<string,Func<object>> BuiltinContextsAndValues
        {
            get
            {
                return new Dictionary<string, Func<object>>
                {
                    { Here, () => context.Model },
                    { Repeat, () => context.Repetitions },
                    { Options, () => config.KeywordOptions},
                    { Nothing, () => null },
                    { Default, () => CancellationToken.Instance },

      //              case ATTRS:
      //  output = true;
      //          result = this.Attrs;
      //          break;

      //case TEMPLATE:
      //          result = _templateFileFactory.CreateTemplateFile(currentContext.Element.OwnerDocument);
      //          output = (result != null);
      //          break;

      //case CONTAINER:
                //var sourceInfo = currentContext.Element.OwnerDocument.GetSourceInfo();
                //result = (sourceInfo != null) ? sourceInfo.GetContainer() : null;
                //output = result != null;
                //break;
                };
            }
        }

        public BuiltinContextsProvider(ExpressionContext context, RenderingConfig config)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            this.config = config ?? throw new System.ArgumentNullException(nameof(config));
        }
    }
}
