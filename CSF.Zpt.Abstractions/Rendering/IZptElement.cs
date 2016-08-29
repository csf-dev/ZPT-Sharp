using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which wraps a DOM element.
  /// </summary>
  public interface IZptElement : IEquatable<IZptElement>
  {
    #region properties

    /// <summary>
    /// Gets the element name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    /// <summary>
    /// Gets information about the source file for the current element.
    /// </summary>
    /// <value>The source file.</value>
    ISourceInfo SourceFile { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is the root of its parent document.
    /// </summary>
    /// <value><c>true</c> if this instance is the root element; otherwise, <c>false</c>.</value>
    bool IsRoot { get; }

    /// <summary>
    /// Gets a value indicating whether this instance has a parent element or not.
    /// </summary>
    /// <value><c>true</c> if this instance has a parent element; otherwise, <c>false</c>.</value>
    bool HasParent { get; }

    /// <summary>
    /// Gets a value indicating whether this instance represents an element imported from another document tree.
    /// </summary>
    /// <value><c>true</c> if this instance is imported; otherwise, <c>false</c>.</value>
    bool IsImported { get; }

    /// <summary>
    /// Gets a <c>System.Type</c> indicating the type of <see cref="IZptDocument"/> to which the current instance
    /// belongs.
    /// </summary>
    /// <value>The type of ZPT document implementation.</value>
    Type ZptDocumentType { get; }

    /// <summary>
    /// Gets a value indicating whether or not this instance can write a comment node to a node that does not have
    /// a parent.
    /// </summary>
    /// <value><c>true</c> if this instance can write a comment node if it does not have a parent; otherwise, <c>false</c>.</value>
    bool CanWriteCommentWithoutParent { get; }

    /// <summary>
    /// Gets the <see cref="IZptDocument"/> instance to which the current instance belongs.
    /// </summary>
    IZptDocument OwnerDocument { get; }

    #endregion

    #region methods

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="IZptElement"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="IZptElement"/>.
    /// </returns>
    string ToString();

    /// <summary>
    /// Replaces the current element in its respective DOM with the given replacement.
    /// </summary>
    /// <returns>A reference to the replacement element, in its new DOM.</returns>
    /// <param name="replacement">Replacement.</param>
    IZptElement ReplaceWith(IZptElement replacement);

    /// <summary>
    /// Replaces the current element instance with the given content.
    /// </summary>
    /// <returns>A collection of <see cref="IZptElement"/>, indicating the element(s) which replaced the current instance.</returns>
    /// <param name="content">The content with which to replace the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    IZptElement[] ReplaceWith(string content, bool interpretContentAsStructure);

    /// <summary>
    /// Removes all children of the current element instance and replaces them with the given content.
    /// </summary>
    /// <param name="content">The content with which to replace the children of the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    void ReplaceChildrenWith(string content, bool interpretContentAsStructure);

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the previous
    /// sibling before a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, before which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    IZptElement InsertBefore(IZptElement existing, IZptElement newChild);

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the next
    /// sibling after a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, after which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    IZptElement InsertAfter(IZptElement existing, IZptElement newChild);

    /// <summary>
    /// Gets the element which is the parent of the current instance.
    /// </summary>
    /// <returns>The parent element.</returns>
    IZptElement GetParentElement();

    /// <summary>
    /// Gets an ordered element chain, starting with the current element and including all of its parent elements from
    /// closest to furthest ancestors.
    /// </summary>
    /// <returns>The element chain.</returns>
    IZptElement[] GetElementChain();

    /// <summary>
    /// Gets a collection of the child elements from the current source element.
    /// </summary>
    /// <returns>The children.</returns>
    IZptElement[] GetChildElements();

    /// <summary>
    /// Gets a collection of the attributes present upon the current element.
    /// </summary>
    /// <returns>The attributes.</returns>
    IZptAttribute[] GetAttributes();

    /// <summary>
    /// Gets an attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    IZptAttribute GetAttribute(ZptNamespace attributeNamespace, string name);

    /// <summary>
    /// Sets the value of an attribute.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    void SetAttribute(string name, string value);

    /// <summary>
    /// Sets the value of an attribute.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    void SetAttribute(ZptNamespace attributeNamespace, string name, string value);

    /// <summary>
    /// Removes a named attribute.
    /// </summary>
    /// <param name="name">The attribtue name.</param>
    void RemoveAttribute(string name);

    /// <summary>
    /// Removes a named attribute.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    void RemoveAttribute(ZptNamespace attributeNamespace, string name);

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    IZptElement[] SearchChildrenByAttribute(ZptNamespace attributeNamespace, string name);

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    IZptElement SearchAncestorsByAttribute(ZptNamespace attributeNamespace, string name);

    /// <summary>
    /// Recursively searches for attributes with a given namespace or prefix and removes them from their parent
    /// element.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    void PurgeAttributes(ZptNamespace attributeNamespace);

    /// <summary>
    /// Recursively searches for elements with a given namespace or prefix and removes them using the
    /// <see cref="Omit"/> behaviour.
    /// </summary>
    /// <param name="elementNamespace">The element namespace.</param>
    void PurgeElements(ZptNamespace elementNamespace);

    /// <summary>
    /// Adds a new comment to the DOM immediately before the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    void AddCommentBefore(string comment);

    /// <summary>
    /// Adds a new comment to the DOM inside the current element as its first child.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    void AddCommentInside(string comment);

    /// <summary>
    /// Adds a new comment to the DOM immediately after the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    void AddCommentAfter(string comment);

    /// <summary>
    /// Clone this instance into a new Element instance, which may be manipulated without affecting the original.
    /// </summary>
    IZptElement Clone();

    /// <summary>
    /// Gets the file location (typically a line number) for the current instance.
    /// </summary>
    /// <returns>The file location.</returns>
    string GetFileLocation();

    /// <summary>
    /// Gets the file location (typically a line number) for the end tag matched with the current instance.
    /// </summary>
    /// <returns>The end tag file location.</returns>
    string GetEndTagFileLocation();

    /// <summary>
    /// Omits the current element, replacing it with its children.
    /// </summary>
    /// <returns>
    /// A collection of the <see cref="IZptElement"/> instances which were children of the element traversed
    /// </returns>
    IZptElement[] Omit();

    /// <summary>
    /// Removes the current element from the DOM.
    /// </summary>
    void Remove();

    /// <summary>
    /// Removes all child elements from the current element.
    /// </summary>
    void RemoveAllChildren();

    /// <summary>
    /// Determines whether or not the current instance is in the specified namespace.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is in the specified namespace; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="nSpace">The namespace for which to test.</param>
    bool IsInNamespace(ZptNamespace nSpace);

    /// <summary>
    /// Determines whether this instance is from same document as the specified element.
    /// </summary>
    /// <returns><c>true</c> if this instance is from same document as the specified element; otherwise, <c>false</c>.</returns>
    /// <param name="other">The element to test.</param>
    bool IsFromSameDocumentAs(IZptElement other);

    /// <summary>
    /// Gets the full file path and location for the current element.
    /// </summary>
    /// <returns>The full file path and location.</returns>
    string GetFullFilePathAndLocation();

    #endregion
  }
}

