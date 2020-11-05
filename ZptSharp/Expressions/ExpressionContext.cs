using System;
using System.Collections.Generic;
using ZptSharp.Dom;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which provides contextual values for the rendering of TALES expressions.
    /// </summary>
    public class ExpressionContext
    {
        IElement currentElement;

        /// <summary>
        /// Gets or sets an object representing an error which was encountered whilst rendering or
        /// evaluating an expression.
        /// </summary>
        /// <value>The error.</value>
        public object Error { get; set; }

        /// <summary>
        /// Gets or sets the primary model object from which this context is created.
        /// </summary>
        /// <value>The model.</value>
        public object Model { get; set; }

        /// <summary>
        /// Gets or sets the current DOM element being rendered by this context.
        /// </summary>
        /// <value>The DOM element.</value>
        public IElement CurrentElement
        {
            get => currentElement;
            set => currentElement = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the DOM document being used as a template to render the current rendering request.
        /// </summary>
        /// <value>The template document.</value>
        public IDocument TemplateDocument { get; set; }

        /// <summary>
        /// Gets the local variable definitions for the current context.
        /// </summary>
        /// <value>The local definitions.</value>
        public IDictionary<string, object> LocalDefinitions { get; }

        /// <summary>
        /// Gets the global variable definitions for the current context.
        /// </summary>
        /// <value>The global definitions.</value>
        public IDictionary<string, object> GlobalDefinitions { get; }

        /// <summary>
        /// Gets the repetition variable definitions for the current context.
        /// </summary>
        /// <value>The repetition definitions.</value>
        public IDictionary<string, RepetitionInfo> Repetitions { get; }

        /// <summary>
        /// Gets a clone of the current expression context, but using an alternative specified element.
        /// Additionally, this is intended to be a child context, such that changes to it are not
        /// automatically reflected in the parent context (where it is not applicable to do so).
        /// </summary>
        /// <returns>The cloned expression context.</returns>
        /// <param name="element">The element for the cloned context.</param>
        public ExpressionContext CreateChild(IElement element)
        {
            return new ExpressionContext(element, LocalDefinitions, GlobalDefinitions, Repetitions)
            {
                Error = Error,
                Model = Model,
                TemplateDocument = TemplateDocument,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContext"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Both the <paramref name="localDefinitions"/> and <paramref name="repetitions"/> are shallow-copied by this constructor,
        /// because changes to these two collections which are made inside of a rendering context should not affect any other
        /// rendering context (except children created from this context).
        /// </para>
        /// <para>
        /// On the other hand, <paramref name="globalDefinitions"/> is used directly without copying, because changes to the global
        /// definitions should affect other contexts, including those 'outside' of this one.
        /// </para>
        /// </remarks>
        /// <param name="element">The DOM element for this context.</param>
        /// <param name="localDefinitions">Local definitions.</param>
        /// <param name="globalDefinitions">Global definitions.</param>
        /// <param name="repetitions">Repetitions.</param>
        public ExpressionContext(IElement element,
                                 IDictionary<string, object> localDefinitions = null,
                                 IDictionary<string, object> globalDefinitions = null,
                                 IDictionary<string, RepetitionInfo> repetitions = null)
        {
            CurrentElement = element;
            LocalDefinitions = new Dictionary<string, object>(localDefinitions ?? new Dictionary<string, object>());
            GlobalDefinitions = globalDefinitions ?? new Dictionary<string, object>();
            Repetitions = new Dictionary<string, RepetitionInfo>(repetitions ?? new Dictionary<string, RepetitionInfo>());
        }
    }
}
