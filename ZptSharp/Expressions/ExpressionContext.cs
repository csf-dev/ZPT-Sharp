using System;
using System.Collections.Generic;
using System.Linq;
using ZptSharp.Dom;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which provides contextual values for the rendering of TALES expressions.
    /// </summary>
    public class ExpressionContext
    {
        INode currentElement;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExpressionContext"/> is the root context.
        /// </summary>
        /// <value><c>true</c> if this is the root context; otherwise, <c>false</c>.</value>
        public bool IsRootContext { get; set; }

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
        public INode CurrentElement
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
        /// Gets a collection of the contexts &amp; handlers which might be able to deal
        /// with errors encountered whilst processing this context.
        /// </summary>
        /// <value>The error handlers.</value>
        public Stack<ErrorHandlingContext> ErrorHandlers { get; }

        /// <summary>
        /// Gets a clone of the current expression context, but using an alternative specified element.
        /// Additionally, this is intended to be a child context, such that changes to it are not
        /// automatically reflected in the parent context (where it is not applicable to do so).
        /// </summary>
        /// <returns>The cloned expression context.</returns>
        /// <param name="element">The element for the cloned context.</param>
        public ExpressionContext CreateChild(INode element)
        {
            return new ExpressionContext(element, LocalDefinitions, GlobalDefinitions, Repetitions, ErrorHandlers)
            {
                Error = Error,
                Model = Model,
                TemplateDocument = TemplateDocument,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContext"/> class.
        /// </summary>
        /// <param name="element">The DOM element for this context.</param>
        public ExpressionContext(INode element) : this(element, null, null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContext"/> class; this is (to a degree)
        /// a copy-constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <paramref name="localDefinitions"/>, <paramref name="repetitions"/> &amp; <paramref name="errorHandlers"/>
        /// are shallow-copied by this constructor.  This is  because changes to these collections which are made
        /// inside of a context should not affect any other context
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
        /// <param name="errorHandlers">Error handlers</param>
        public ExpressionContext(INode element,
                                 IDictionary<string, object> localDefinitions,
                                 IDictionary<string, object> globalDefinitions,
                                 IDictionary<string, RepetitionInfo> repetitions,
                                 Stack<ErrorHandlingContext> errorHandlers)
        {
            CurrentElement = element;
            LocalDefinitions = new Dictionary<string, object>(localDefinitions ?? new Dictionary<string, object>());
            GlobalDefinitions = globalDefinitions ?? new Dictionary<string, object>();
            Repetitions = new Dictionary<string, RepetitionInfo>(repetitions ?? new Dictionary<string, RepetitionInfo>());
            ErrorHandlers = new Stack<ErrorHandlingContext>(errorHandlers ?? Enumerable.Empty<ErrorHandlingContext>());
        }
    }
}
