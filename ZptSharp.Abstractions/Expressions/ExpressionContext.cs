using System;
using System.Collections.Generic;
using System.Linq;
using ZptSharp.Dom;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which provides contextual values for the rendering of TALES expressions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class encapsulates the state for every ZPT operation performed upon a DOM node.
    /// That state is contextual, related to the current position in the DOM document and other operations which have occured previously.
    /// </para>
    /// <para>
    /// Some ZPT operations, such as <c>tal:define</c>, will make alterations to the current context.  Others might
    /// just read it.
    /// </para>
    /// <para>
    /// Apart from the root context (see <see cref="IsRootContext"/>), every context is created by cloning its
    /// parent context and substituting a different 'current' DOM node.
    /// The structure of parent-to-child expression contexts follows the structure of the DOM.
    /// </para>
    /// </remarks>
    public class ExpressionContext
    {
        INode currentNode;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExpressionContext"/> is the root context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The root context is typically synonymous with the root DOM node in the document.
        /// The root context is one which was not created from a parent context.
        /// </para>
        /// </remarks>
        /// <value><c>true</c> if this is the root context; otherwise, <c>false</c>.</value>
        public bool IsRootContext { get; set; }

        /// <summary>
        /// Gets or sets an object representing an error which was encountered whilst rendering or
        /// evaluating an expression.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The error object will almost always derive from <see cref="Exception"/>.
        /// If it is <see langword="null"/> then no error has occurred when rendering; if not then it will describe the error which is currently being handled.
        /// </para>
        /// </remarks>
        /// <value>The error (usually an exception).</value>
        public object Error { get; set; }

        /// <summary>
        /// Gets or sets the model object from which this context was created.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is typically the model which was initially passed to the rendering process.
        /// It may be of any object (and could legitimately be <see langword="null"/>).
        /// Its precise semantics depend upon the application/current usage of ZptSharp.
        /// </para>
        /// </remarks>
        /// <value>The model.</value>
        public object Model { get; set; }

        /// <summary>
        /// Gets or sets the current DOM node being rendered by this context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Every ZPT operation has a current DOM node, because the ZPT operations are declared within the attributes of element nodes.
        /// It would be very unusual for this node object not to be a DOM element node.
        /// </para>
        /// </remarks>
        /// <value>The DOM node (typically an element node).</value>
        public INode CurrentNode
        {
            get => currentNode;
            set => currentNode = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the DOM document being used as a template to render the current rendering request.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the document currently being rendered.
        /// In scenarios where a rendering request touches and draws source from many documents (METAL macro usage),
        /// this document is the primary/initial document which was selected as the template.
        /// </para>
        /// </remarks>
        /// <value>The template document.</value>
        public IDocument TemplateDocument { get; set; }

        /// <summary>
        /// Gets the local variable definitions for the current context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Definitions are the variables which are available to ZPT operations using this context.
        /// There are three types of definitions.
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><c>LocalDefinitions</c> (this property)</description>
        /// </item>
        /// <item>
        /// <description><see cref="GlobalDefinitions"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="Repetitions"/></description>
        /// </item>
        /// </list>
        /// <para>
        /// Local definitions are scoped to the DOM element upon which they are created (via the
        /// <c>tal:define</c> keyword) and that DOM element's descendents.
        /// A locally-defined variable is not visible 'outside' of the element where it was defined.
        /// </para>
        /// <para>
        /// Global definitions are in-scope from the document position at which they were defined and
        /// onwards in the document (in source code order).
        /// Globally-defined variables do not use the DOM structure; global variables are usable outside of
        /// the element &amp; descendents where they were defined, as long as it is after the point of
        /// definition in the document source code.
        /// </para>
        /// <para>
        /// Repetitions are a special type of variable which are created only by the <c>tal:repeat</c>
        /// operation.
        /// They behave identically to locally-defined variables except that they have a standard set of
        /// properties, defined by the <see cref="RepetitionInfo"/> class.
        /// </para>
        /// </remarks>
        /// <value>The local definitions.</value>
        public IDictionary<string, object> LocalDefinitions { get; }

        /// <summary>
        /// Gets the global variable definitions for the current context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Definitions are the variables which are available to ZPT operations using this context.
        /// There are three types of definitions.
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="LocalDefinitions"/></description>
        /// </item>
        /// <item>
        /// <description><c>GlobalDefinitions</c> (this property)</description>
        /// </item>
        /// <item>
        /// <description><see cref="Repetitions"/></description>
        /// </item>
        /// </list>
        /// <para>
        /// Local definitions are scoped to the DOM element upon which they are created (via the
        /// <c>tal:define</c> keyword) and that DOM element's descendents.
        /// A locally-defined variable is not visible 'outside' of the element where it was defined.
        /// </para>
        /// <para>
        /// Global definitions are in-scope from the document position at which they were defined and
        /// onwards in the document (in source code order).
        /// Globally-defined variables do not use the DOM structure; global variables are usable outside of
        /// the element &amp; descendents where they were defined, as long as it is after the point of
        /// definition in the document source code.
        /// </para>
        /// <para>
        /// Repetitions are a special type of variable which are created only by the <c>tal:repeat</c>
        /// operation.
        /// They behave identically to locally-defined variables except that they have a standard set of
        /// properties, defined by the <see cref="RepetitionInfo"/> class.
        /// </para>
        /// </remarks>
        /// <value>The global definitions.</value>
        public IDictionary<string, object> GlobalDefinitions { get; }

        /// <summary>
        /// Gets the repetition variable definitions for the current context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Definitions are the variables which are available to ZPT operations using this context.
        /// There are three types of definitions.
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="LocalDefinitions"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="GlobalDefinitions"/></description>
        /// </item>
        /// <item>
        /// <description><c>Repetitions</c> (this property)</description>
        /// </item>
        /// </list>
        /// <para>
        /// Local definitions are scoped to the DOM element upon which they are created (via the
        /// <c>tal:define</c> keyword) and that DOM element's descendents.
        /// A locally-defined variable is not visible 'outside' of the element where it was defined.
        /// </para>
        /// <para>
        /// Global definitions are in-scope from the document position at which they were defined and
        /// onwards in the document (in source code order).
        /// Globally-defined variables do not use the DOM structure; global variables are usable outside of
        /// the element &amp; descendents where they were defined, as long as it is after the point of
        /// definition in the document source code.
        /// </para>
        /// <para>
        /// Repetitions are a special type of variable which are created only by the <c>tal:repeat</c>
        /// operation.
        /// They behave identically to locally-defined variables except that they have a standard set of
        /// properties, defined by the <see cref="RepetitionInfo"/> class.
        /// </para>
        /// </remarks>
        /// <value>The repetition definitions.</value>
        public IDictionary<string, RepetitionInfo> Repetitions { get; }

        /// <summary>
        /// Gets a collection of the contexts &amp; handlers which might be able to deal
        /// with errors encountered whilst processing this context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Error handlers are added to this property by the parent contexts.
        /// Much like the C# <c>try</c> &amp; <c>catch</c> keywords, ZPT rendering errors 'propagate upwards' through the DOM.
        /// This occurs until either they are handled by a <c>tal:on-error</c> attribute or until they 'escape' the root of the DOM.
        /// In the second case, this will lead to an exception being raised for the overall rendering operation.
        /// A rendering error upon an element might be handled by a <c>tal:on-error</c> attribute defined upon that same
        /// element, its parent element, grandparent or more disatant ancestor.
        /// </para>
        /// </remarks>
        /// <value>The error handlers.</value>
        public Stack<ErrorHandlingContext> ErrorHandlers { get; }

        /// <summary>
        /// Gets a clone of the current expression context, using a specified node as the current node for the created context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is provided for the creation of child expression contexts.  The <paramref name="node"/>
        /// is intended to be a child of the <see cref="CurrentNode"/>.
        /// </para>
        /// <para>
        /// Because this is intended to be a child context, the local definitions, repetitions &amp; error handlers
        /// are cloned by this method, such that modifications to these collections upon the child context do not
        /// affect the parent.
        /// </para>
        /// <para>
        /// The exception to this, is the <see cref="GlobalDefinitions"/>.
        /// Global definitions are intentionally not cloned, because a child's changes to global definitions may indeed affect parent contexts.
        /// A global definition is in-scope from the moment it is defined, for the remainder of the document, regardless of the DOM hierarchy.
        /// </para>
        /// </remarks>
        /// <returns>The cloned expression context.</returns>
        /// <param name="node">The node for the cloned context.</param>
        public ExpressionContext CreateChild(INode node)
        {
            return new ExpressionContext(node, LocalDefinitions, GlobalDefinitions, Repetitions, ErrorHandlers)
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
        /// This constructor is typically used for the root expression context.
        /// All of the other state of the context (apart from the current node) will be initialized with default/empty values.
        /// Even if this is for a root context, <see cref="IsRootContext"/> must still be set manually.
        /// </para>
        /// </remarks>
        /// <param name="node">The DOM node for this context.</param>
        public ExpressionContext(INode node) : this(node, null, null, null, null) { }

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
        /// <param name="node">The DOM node for this context.</param>
        /// <param name="localDefinitions">Local definitions.</param>
        /// <param name="globalDefinitions">Global definitions.</param>
        /// <param name="repetitions">Repetitions.</param>
        /// <param name="errorHandlers">Error handlers</param>
        public ExpressionContext(INode node,
                                 IDictionary<string, object> localDefinitions,
                                 IDictionary<string, object> globalDefinitions,
                                 IDictionary<string, RepetitionInfo> repetitions,
                                 Stack<ErrorHandlingContext> errorHandlers)
        {
            CurrentNode = node;
            LocalDefinitions = new Dictionary<string, object>(localDefinitions ?? new Dictionary<string, object>());
            GlobalDefinitions = globalDefinitions ?? new Dictionary<string, object>();
            Repetitions = new Dictionary<string, RepetitionInfo>(repetitions ?? new Dictionary<string, RepetitionInfo>());
            ErrorHandlers = new Stack<ErrorHandlingContext>(errorHandlers ?? Enumerable.Empty<ErrorHandlingContext>());
        }
    }
}
