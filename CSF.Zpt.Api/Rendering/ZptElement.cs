using System;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents an element node in a ZPT document.
  /// </summary>
  public abstract class ZptElement : IEquatable<ZptElement>
  {
    #region fields

    private SourceFileInfo _sourceFile;
    private bool _isRoot, _isImported;

    #endregion

    #region properties

    /// <summary>
    /// Gets the element name.
    /// </summary>
    /// <value>The name.</value>
    public abstract string Name { get; }

    /// <summary>
    /// Gets information about the source file for the current element.
    /// </summary>
    /// <value>The source file.</value>
    public virtual SourceFileInfo SourceFile
    {
      get {
        return _sourceFile;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is the root of its parent document.
    /// </summary>
    /// <value><c>true</c> if this instance is the root element; otherwise, <c>false</c>.</value>
    public virtual bool IsRoot
    {
      get {
        return _isRoot;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance has a parent element or not.
    /// </summary>
    /// <value><c>true</c> if this instance has a parent element; otherwise, <c>false</c>.</value>
    public abstract bool HasParent { get; }

    /// <summary>
    /// Gets a value indicating whether this instance represents an element imported from another document tree.
    /// </summary>
    /// <value><c>true</c> if this instance is imported; otherwise, <c>false</c>.</value>
    public virtual bool IsImported
    {
      get {
        return _isImported;
      }
    }

    /// <summary>
    /// Gets a <c>System.Type</c> indicating the type of <see cref="IZptDocument"/> to which the current instance
    /// belongs.
    /// </summary>
    /// <value>The type of ZPT document implementation.</value>
    public abstract Type ZptDocumentType { get; }

    #endregion

    #region methods

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="ZptElement"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="ZptElement"/>.
    /// </returns>
    public abstract new string ToString();

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptElement"/>.
    /// </summary>
    /// <param name="obj">
    /// The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.Rendering.ZptElement"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptElement"/>; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      bool output;

      if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else
      {
        var other = obj as ZptElement;
        output = (other != null && this.Equals(other));
      }

      return output;
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.ZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptElement"/>.
    /// </summary>
    /// <param name="other">
    /// The <see cref="CSF.Zpt.Rendering.ZptElement"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.ZptElement"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.ZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptElement"/>; otherwise, <c>false</c>.
    /// </returns>
    public abstract bool Equals(ZptElement other);

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.Rendering.ZptElement"/> object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
    /// hash table.
    /// </returns>
    public abstract override int GetHashCode();

    /// <summary>
    /// Replaces the current element in its respective DOM with the given replacement.
    /// </summary>
    /// <returns>A reference to the replacement element, in its new DOM.</returns>
    /// <param name="replacement">Replacement.</param>
    public abstract ZptElement ReplaceWith(ZptElement replacement);

    /// <summary>
    /// Replaces the current element instance with the given content.
    /// </summary>
    /// <returns>A collection of <see cref="ZptElement"/>, indicating the element(s) which replaced the current instance.</returns>
    /// <param name="content">The content with which to replace the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    public abstract ZptElement[] ReplaceWith(string content, bool interpretContentAsStructure);

    /// <summary>
    /// Removes all children of the current element instance and replaces them with the given content.
    /// </summary>
    /// <param name="content">The content with which to replace the children of the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    public abstract void ReplaceChildrenWith(string content, bool interpretContentAsStructure);

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the previous
    /// sibling before a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, before which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    public abstract ZptElement InsertBefore(ZptElement existing, ZptElement newChild);

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the next
    /// sibling after a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, after which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    public abstract ZptElement InsertAfter(ZptElement existing, ZptElement newChild);

    /// <summary>
    /// Gets the element which is the parent of the current instance.
    /// </summary>
    /// <returns>The parent element.</returns>
    public abstract ZptElement GetParentElement();

    /// <summary>
    /// Gets an ordered element chain, starting with the current element and including all of its parent elements from
    /// closest to furthest ancestors.
    /// </summary>
    /// <returns>The element chain.</returns>
    public virtual ZptElement[] GetElementChain()
    {
      List<ZptElement> output = new List<ZptElement>();
      ZptElement current = this;

      while(current != null)
      {
        output.Add(current);
        current = current.GetParentElement();
      }

      return output.ToArray();
    }

    /// <summary>
    /// Gets a collection of the child elements from the current source element.
    /// </summary>
    /// <returns>The children.</returns>
    public abstract ZptElement[] GetChildElements();

    /// <summary>
    /// Gets a collection of the attributes present upon the current element.
    /// </summary>
    /// <returns>The attributes.</returns>
    public abstract ZptAttribute[] GetAttributes();

    /// <summary>
    /// Gets an attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public abstract ZptAttribute GetAttribute(ZptNamespace attributeNamespace, string name);

    /// <summary>
    /// Sets the value of an attribute.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    public virtual void SetAttribute(string name, string value)
    {
      this.SetAttribute(ZptNamespace.Default, name, value);
    }

    /// <summary>
    /// Sets the value of an attribute.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    public abstract void SetAttribute(ZptNamespace attributeNamespace, string name, string value);

    /// <summary>
    /// Removes a named attribute.
    /// </summary>
    /// <param name="name">The attribtue name.</param>
    public virtual void RemoveAttribute(string name)
    {
      this.RemoveAttribute(ZptNamespace.Default, name);
    }

    /// <summary>
    /// Removes a named attribute.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public abstract void RemoveAttribute(ZptNamespace attributeNamespace, string name);

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public abstract ZptElement[] SearchChildrenByAttribute(ZptNamespace attributeNamespace, string name);

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public virtual ZptElement SearchAncestorsByAttribute(ZptNamespace attributeNamespace, string name)
    {
      ZptElement output = null;

      var currentElement = this;
      while(output == null && currentElement != null)
      {
        if(currentElement.GetAttribute(attributeNamespace, name) != null)
        {
          output = currentElement;
        }
        currentElement = currentElement.GetParentElement();
      }

      return output;
    }

    /// <summary>
    /// Recursively searches for attributes with a given namespace or prefix and removes them from their parent
    /// element.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    public abstract void PurgeAttributes(ZptNamespace attributeNamespace);

    /// <summary>
    /// Recursively searches for elements with a given namespace or prefix and removes them using the
    /// <see cref="Omit"/> behaviour.
    /// </summary>
    /// <param name="elementNamespace">The element namespace.</param>
    public abstract void PurgeElements(ZptNamespace elementNamespace);

    /// <summary>
    /// Adds a new comment to the DOM immediately before the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public abstract void AddCommentBefore(string comment);

    /// <summary>
    /// Adds a new comment to the DOM immediately after the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public abstract void AddCommentAfter(string comment);

    /// <summary>
    /// Clone this instance into a new Element instance, which may be manipulated without affecting the original.
    /// </summary>
    public abstract ZptElement Clone();

    /// <summary>
    /// Gets the file location (typically a line number) for the current instance.
    /// </summary>
    /// <returns>The file location.</returns>
    public abstract string GetFileLocation();

    /// <summary>
    /// Omits the current element, replacing it with its children.
    /// </summary>
    /// <returns>
    /// A collection of the <see cref="ZptElement"/> instances which were children of the element traversed
    /// </returns>
    public abstract ZptElement[] Omit();

    /// <summary>
    /// Removes the current element from the DOM.
    /// </summary>
    public abstract void Remove();

    /// <summary>
    /// Removes all child elements from the current element.
    /// </summary>
    public abstract void RemoveAllChildren();

    /// <summary>
    /// Determines whether or not the current instance is in the specified namespace.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is in the specified namespace; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="nSpace">The namespace for which to test.</param>
    public abstract bool IsInNamespace(ZptNamespace nSpace);

    #endregion

    #region constructor

    /// <summary>
    /// Fake constructor for Mocking framework usage only.  Do not call.
    /// </summary>
    protected ZptElement() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ZptElement"/> class.
    /// </summary>
    /// <param name="sourceFile">Information about the element's source file.</param>
    /// <param name="isRoot">Whether or not this is the root element.</param>
    /// <param name="isImported">Whether or not this element is imported.</param>
    public ZptElement(SourceFileInfo sourceFile,
                      bool isRoot,
                      bool isImported)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }

      _sourceFile = sourceFile;
      _isRoot = isRoot;
      _isImported = isImported;
    }

    #endregion
  }
}

