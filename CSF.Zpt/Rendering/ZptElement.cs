using System;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents an element node in a ZPT document.
  /// </summary>
  public abstract class ZptElement : IZptElement, IEquatable<ZptElement>
  {
    #region fields

    private ISourceInfo _sourceFile;
    private bool _isRoot, _isImported;
    private IZptDocument _ownerDocument;
    private ISourceInfoFactory _sourceInfoCreator;

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
    public virtual ISourceInfo SourceFile
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

    /// <summary>
    /// Gets a value indicating whether or not this instance can write a comment node to a node that does not have
    /// a parent.
    /// </summary>
    /// <value><c>true</c> if this instance can write a comment node if it does not have a parent; otherwise, <c>false</c>.</value>
    public abstract bool CanWriteCommentWithoutParent { get; }

    /// <summary>
    /// Gets the <see cref="IZptDocument"/> instance to which the current instance belongs.
    /// </summary>
    public virtual IZptDocument OwnerDocument
    {
        get { return _ownerDocument; }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets information about the source of the current element.
    /// </summary>
    public ISourceInfo GetSourceInfo()
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }


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
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.IZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptElement"/>.
    /// </summary>
    /// <param name="obj">
    /// The <see cref="CSF.Zpt.Rendering.IZptElement"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.ZptElement"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.IZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptElement"/>; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool Equals(IZptElement obj)
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
    public abstract IZptElement ReplaceWith(IZptElement replacement);

    /// <summary>
    /// Replaces the current element instance with the given content.
    /// </summary>
    /// <returns>A collection of <see cref="IZptElement"/>, indicating the element(s) which replaced the current instance.</returns>
    /// <param name="content">The content with which to replace the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    public abstract IZptElement[] ReplaceWith(string content, bool interpretContentAsStructure);

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
    public abstract IZptElement InsertBefore(IZptElement existing, IZptElement newChild);

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the next
    /// sibling after a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, after which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    public abstract IZptElement InsertAfter(IZptElement existing, IZptElement newChild);

    /// <summary>
    /// Gets the element which is the parent of the current instance.
    /// </summary>
    /// <returns>The parent element.</returns>
    public abstract IZptElement GetParentElement();

    /// <summary>
    /// Gets an ordered element chain, starting with the current element and including all of its parent elements from
    /// closest to furthest ancestors.
    /// </summary>
    /// <returns>The element chain.</returns>
    public virtual IZptElement[] GetElementChain()
    {
      List<IZptElement> output = new List<IZptElement>();
      IZptElement current = this;

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
    public abstract IZptElement[] GetChildElements();

    /// <summary>
    /// Gets a collection of the attributes present upon the current element.
    /// </summary>
    /// <returns>The attributes.</returns>
    public abstract IZptAttribute[] GetAttributes();

    /// <summary>
    /// Gets an attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public abstract IZptAttribute GetAttribute(ZptNamespace attributeNamespace, string name);

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
    public abstract IZptElement[] SearchChildrenByAttribute(ZptNamespace attributeNamespace, string name);

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public virtual IZptElement SearchAncestorsByAttribute(ZptNamespace attributeNamespace, string name)
    {
      IZptElement output = null, currentElement = this;

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
    /// Adds a new comment to the DOM inside the current element as its first child.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public abstract void AddCommentInside(string comment);

    /// <summary>
    /// Adds a new comment to the DOM immediately after the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public abstract void AddCommentAfter(string comment);

    /// <summary>
    /// Clone this instance into a new Element instance, which may be manipulated without affecting the original.
    /// </summary>
    public abstract IZptElement Clone();

    /// <summary>
    /// Gets the file location (typically a line number) for the current instance.
    /// </summary>
    /// <returns>The file location.</returns>
    public abstract string GetFileLocation();

    /// <summary>
    /// Gets the file location (typically a line number) for the end tag matched with the current instance.
    /// </summary>
    /// <returns>The end tag file location.</returns>
    public abstract string GetEndTagFileLocation();

    /// <summary>
    /// Omits the current element, replacing it with its children.
    /// </summary>
    /// <returns>
    /// A collection of the <see cref="IZptElement"/> instances which were children of the element traversed
    /// </returns>
    public abstract IZptElement[] Omit();

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

    /// <summary>
    /// Determines whether this instance is from same document as the specified element.
    /// </summary>
    /// <returns><c>true</c> if this instance is from same document as the specified element; otherwise, <c>false</c>.</returns>
    /// <param name="other">The element to test.</param>
    public abstract bool IsFromSameDocumentAs(IZptElement other);

    /// <summary>
    /// Gets the full file path and location for the current element.
    /// </summary>
    /// <returns>The full file path and location.</returns>
    public virtual string GetFullFilePathAndLocation()
    {
      if(SourceFile == null)
      {
        return "<unknown>";
      }
      else
      {
        string location = GetFileLocation();
        if(location != null)
        {
          return String.Format("{0} (line {1})", SourceFile.FullName, location);
        }
        else
        {
          return SourceFile.FullName;
        }
      }
    }

    /// <summary>
    /// Converts the given <see cref="IZptElement"/> to an implementation-specific subclass, or raises an exception
    /// if the conversion is not valid.
    /// </summary>
    /// <returns>The converted element instance.</returns>
    /// <param name="element">The element for conversion.</param>
    /// <typeparam name="TElement">The desired element type.</typeparam>
    protected virtual TElement ConvertTo<TElement>(IZptElement element) where TElement : class,IZptElement
    {
      return ConvertElement<TElement>(element);
    }

    /// <summary>
    /// Enforces that an <c>System.Object</c> (representing a parent node) is not null, raising an exception if it
    /// is not.
    /// </summary>
    /// <param name="parentNode">The parent node.</param>
    protected virtual void EnforceParentNodeNotNull(object parentNode)
    {
      if(parentNode == null)
      {
        throw new InvalidOperationException(Resources.ExceptionMessages.CannotGetParentFromRootNode);
      }
    }

    /// <summary>
    /// Enforces that a name must not be <c>null</c> or empty, raising an exception if it is.
    /// </summary>
    /// <param name="name">Name.</param>
    protected static void EnforceNameNotEmpty(string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }
      else if(name.Length == 0)
      {
        throw new ArgumentException(Resources.ExceptionMessages.NameMustNotBeEmptyString, nameof(name));
      }
    }

    /// <summary>
    /// Enforces that two objects (expected and actual), representing node types, are equal.  Raises an exception if
    /// they are not.
    /// </summary>
    /// <param name="expectedNodeLanguage">Expected node language (EG: "XML").</param>
    /// <param name="expectedNodeType">Expected node type.</param>
    /// <param name="actualNodeType">Actual node type.</param>
    /// <typeparam name="TNodeType">The type of objects being compared.</typeparam>
    protected virtual void EnforceNodeType<TNodeType>(string expectedNodeLanguage,
                                                    TNodeType expectedNodeType,
                                                    TNodeType actualNodeType)
    {
      if(!Object.Equals(actualNodeType, expectedNodeType))
      {
        string message = String.Format(Resources.ExceptionMessages.IncorrectWrappedNodeType,
                                       expectedNodeLanguage,
                                       expectedNodeType,
                                       actualNodeType);
        throw new ArgumentException(message);
      }
    }
//
//    private void 

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
    /// <param name="ownerDocument">The ZPT document which owns the element.</param>
    /// <param name="sourceInfoCreator">A source-info factory.</param>
    public ZptElement(ISourceInfo sourceFile,
                      bool isRoot,
                      bool isImported,
                      IZptDocument ownerDocument,
                      ISourceInfoFactory sourceInfoCreator = null)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if(ownerDocument == null)
      {
          throw new ArgumentNullException(nameof(ownerDocument));
      }

      _sourceInfoCreator = sourceInfoCreator?? new SourceInfoFactory();

      _sourceFile = sourceFile;
      _isRoot = isRoot;
      _isImported = isImported;
      _ownerDocument = ownerDocument;
    }

    #endregion

    #region static methods

    /// <summary>
    /// Converts the given <see cref="IZptElement"/> to an implementation-specific subclass, or raises an exception
    /// if the conversion is not valid.
    /// </summary>
    /// <returns>The converted element instance.</returns>
    /// <param name="element">The element for conversion.</param>
    /// <typeparam name="TElement">The desired element type.</typeparam>
    internal static TElement ConvertElement<TElement>(IZptElement element) where TElement : class,IZptElement
    {
      var output = element as TElement;

      if(output == null)
      {
        string message = String.Format(Resources.ExceptionMessages.RenderedElementIncorrectType,
                                       typeof(TElement).Name);
        throw new ArgumentException(message, "element");
      }

      return output;
    }

    #endregion
  }
}

