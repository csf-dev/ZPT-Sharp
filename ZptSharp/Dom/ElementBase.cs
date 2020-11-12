using System;
using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstract base class for an <see cref="IElement"/>, containing functionality
    /// which is neutral to the specific implementation.
    /// </summary>
    public abstract class ElementBase : IElement
    {
        /// <summary>
        /// A field for the <see cref="IDocument"/> to which this element belongs.
        /// </summary>
        protected readonly IDocument Doc;

        /// <summary>
        /// A field for the source information relating to this element.
        /// </summary>
        protected readonly IElementSourceInfo Source;

        /// <summary>
        /// A field for the parent DOM element (which might be <see langword="null"/>).
        /// </summary>
        protected IElement Parent;

        /// <summary>
        /// Gets the parent document for the current element.
        /// </summary>
        /// <value>The document.</value>
        public virtual IDocument Document => Doc;

        /// <summary>
        /// Gets information which indicates the original source of the element (for example, a file path and line number).
        /// </summary>
        /// <value>The source info.</value>
        public virtual IElementSourceInfo SourceInfo => Source;

        /// <summary>
        /// Gets or sets the parent for the current element.  This will be a <see langword="null"/>
        /// reference if the current instance is the root element of the document or if the element
        /// is not attached to a DOM.
        /// </summary>
        /// <value>The parent element.</value>
        public virtual IElement ParentElement
        {
            get => Parent;
            set => Parent = value;
        }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public abstract IList<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        public abstract IList<IElement> ChildElements { get; }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public abstract IElement GetCopy();

        /// <summary>
        /// Replaces the specified child element (the <paramref name="toReplace"/> parameter)
        /// using the specified <paramref name="replacement"/> element.
        /// Note that this means that the current element will be detached/removed from its parent as a side-effect.
        /// Further DOM manipulation should occur using the replacement element and not the replaced element.
        /// </summary>
        /// <param name="toReplace">The child element to replace.</param>
        /// <param name="replacement">The replacement element.</param>
        public virtual void ReplaceChild(IElement toReplace, IElement replacement)
        {
            if (toReplace == null)
                throw new ArgumentNullException(nameof(toReplace));
            if (replacement == null)
                throw new ArgumentNullException(nameof(replacement));
            if (!ChildElements.Contains(toReplace))
                throw new ArgumentException(Resources.CoreExceptionMessage.ElementMustBeAChildOfThisParent, nameof(toReplace));

            var index = ChildElements.IndexOf(toReplace);
            ChildElements.Insert(index, replacement);
            ChildElements.Remove(toReplace);
        }

        IDocumentSourceInfo IHasDocumentSourceInfo.SourceInfo => SourceInfo.Document;

        IEnumerable<IElement> IHasElements.GetChildElements() => ChildElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementBase"/> class.
        /// </summary>
        /// <param name="document">The element's document.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="sourceInfo">The element source info.</param>
        protected ElementBase(IDocument document,
                              IElement parent = null,
                              IElementSourceInfo sourceInfo = null)
        {
            this.Doc = document ?? throw new ArgumentNullException(nameof(document));
            this.Parent = parent;
            this.Source = sourceInfo ?? new ElementSourceInfo(new UnknownSourceInfo());
        }
    }
}
