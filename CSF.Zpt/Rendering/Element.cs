using System;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents an element node in a ZPT document.
  /// </summary>
  public abstract class Element
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
    /// Gets a value indicating whether this instance represents an element imported from another document tree.
    /// </summary>
    /// <value><c>true</c> if this instance is imported; otherwise, <c>false</c>.</value>
    public virtual bool IsImported
    {
      get {
        return _isImported;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current
    /// <see cref="CSF.Zpt.Rendering.Element"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Rendering.Element"/>.
    /// </returns>
    public abstract new string ToString();

    /// <summary>
    /// Replaces the current element in its respective DOM with the given replacement.
    /// </summary>
    /// <returns>A reference to the replacement element, in its new DOM.</returns>
    /// <param name="replacement">Replacement.</param>
    public abstract Element ReplaceWith(Element replacement);

    /// <summary>
    /// Gets a collection of the child elements from the current source element.
    /// </summary>
    /// <returns>The children.</returns>
    public abstract Element[] GetChildElements();

    /// <summary>
    /// Gets a collection of the attributes present upon the current element.
    /// </summary>
    /// <returns>The attributes.</returns>
    public abstract Attribute[] GetAttributes();

    /// <summary>
    /// Gets an attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="prefix">The attribute prefix.</param>
    /// <param name="name">The attribute name.</param>
    public abstract Attribute GetAttribute(string attributeNamespace, string prefix, string name);

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="prefix">The attribute prefix.</param>
    /// <param name="name">The attribute name.</param>
    public abstract Element[] SearchChildrenByAttribute(string attributeNamespace, string prefix, string name);

    /// <summary>
    /// Recursively searches for attributes with a given namespace or prefix and removes them from their parent
    /// element.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="prefix">The attribute prefix.</param>
    public abstract void PurgeAttributes(string attributeNamespace, string prefix);

    /// <summary>
    /// Adds a new comment to the DOM immediately before the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public abstract void AddCommentBefore(string comment);

    /// <summary>
    /// Clone this instance into a new Element instance, which may be manipulated without affecting the original.
    /// </summary>
    public abstract Element Clone();

    /// <summary>
    /// Gets the file location (typically a line number) for the current instance.
    /// </summary>
    /// <returns>The file location.</returns>
    public abstract string GetFileLocation();

    #endregion

    #region constructor

    /// <summary>
    /// Fake constructor for Mocking framework usage only.  Do not call.
    /// </summary>
    protected Element() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.Element"/> class.
    /// </summary>
    /// <param name="sourceFile">Information about the element's source file.</param>
    /// <param name="isRoot">Whether or not this is the root element.</param>
    /// <param name="isImported">Whether or not this element is imported.</param>
    public Element(SourceFileInfo sourceFile,
                   bool isRoot,
                   bool isImported)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException("sourceFile");
      }

      _sourceFile = sourceFile;
      _isRoot = isRoot;
      _isImported = isImported;
    }

    #endregion
  }
}

