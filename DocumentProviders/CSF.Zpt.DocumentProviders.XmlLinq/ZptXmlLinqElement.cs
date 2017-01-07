using System;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using CSF.Zpt.Resources;
using System.Xml.XPath;
using System.Collections;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.DocumentProviders
{
  /// <summary>
  /// Implementation of <see cref="ZptElement"/> based on documents parsed using <c>System.Xml.Linq</c>.
  /// </summary>
  public class ZptXmlLinqElement : ZptElement
  {
    #region constants

    private const char NEWLINE = '\n';

    #endregion

    #region fields

    private XElement _node;
    private int? _cachedHashCode, _filePosition;

    #endregion

    #region properties

    /// <summary>
    /// Gets the XML node object wrapped by the current instance.
    /// </summary>
    /// <value>The node.</value>
    public virtual XElement Node
    {
      get {
        return _node;
      }
    }

    /// <summary>
    /// Gets the element name.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get {
        return this.Node.Name.LocalName;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance has a parent element or not.
    /// </summary>
    /// <value><c>true</c> if this instance has a parent element; otherwise, <c>false</c>.</value>
    public override bool HasParent
    {
      get {
        return this.Node.Parent != null;
      }
    }

    /// <summary>
    /// Gets a <c>System.Type</c> indicating the type of <see cref="IZptDocument"/> to which the current instance
    /// belongs.
    /// </summary>
    /// <value>The type of ZPT document implementation.</value>
    public override Type ZptDocumentType
    {
      get {
        return typeof(ZptXmlLinqDocument);
      }
    }

    /// <summary>
    /// Gets a value indicating whether or not this instance can write a comment node to a node that does not have
    /// a parent.
    /// </summary>
    /// <value><c>true</c> if this instance can write a comment node if it does not have a parent; otherwise, <c>false</c>.</value>
    public override bool CanWriteCommentWithoutParent { get { return true; } }

    #endregion

    #region methods

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="ZptXmlLinqElement"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="ZptXmlLinqElement"/>.</returns>
    public override string ToString()
    {
      string output;

      using(var stream = new MemoryStream())
      {
        var encoding = new UTF8Encoding(false);

        using(var xmlWriter = new XmlTextWriter(stream, encoding))
        {
          xmlWriter.Formatting = Formatting.Indented;
          this.Node.WriteTo(xmlWriter);
        }

        output = encoding.GetString(stream.ToArray());
      }

      return output;
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.ZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.DocumentProviders.ZptXmlLinqElement"/>.
    /// </summary>
    /// <param name="other">
    /// The <see cref="CSF.Zpt.Rendering.ZptElement"/> to compare with the current
    /// <see cref="CSF.Zpt.DocumentProviders.ZptXmlLinqElement"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.ZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.DocumentProviders.ZptXmlLinqElement"/>; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(ZptElement other)
    {
      bool output;

      if(Object.ReferenceEquals(this, other))
      {
        output = true;
      }
      else
      {
        var ele = other as ZptXmlLinqElement;
        output = (ele != null && ele.Node == this.Node);
      }

      return output;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.DocumentProviders.ZptXmlLinqElement"/> object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
    /// hash table.
    /// </returns>
    public override int GetHashCode()
    {
      if(!_cachedHashCode.HasValue)
      {
        _cachedHashCode = this.Node.GetHashCode();
      }

      return _cachedHashCode.Value;
    }

    /// <summary>
    /// Replaces the current element in its respective DOM with the given replacement.
    /// </summary>
    /// <returns>A reference to the replacement element, in its new DOM.</returns>
    /// <param name="replacement">Replacement.</param>
    public override IZptElement ReplaceWith(IZptElement replacement)
    {
      var repl = ConvertTo<ZptXmlLinqElement>(replacement);

      var cloned = ((ZptXmlLinqElement) repl.Clone()).Node;

      if(this.Node.Parent != null)
      {
        this.Node.ReplaceWith(cloned);
      }
      else
      {
        this.Node.Document.Root.ReplaceWith(cloned);
      }

      return new ZptXmlLinqElement(cloned,
                                   repl.GetSourceInfo(),
                                   this.OwnerDocument,
                                   isImported: true);
    }

    /// <summary>
    /// Replaces the current element instance with the given content.
    /// </summary>
    /// <returns>A collection of <see cref="IZptElement"/>, indicating the element(s) which replaced the current instance.</returns>
    /// <param name="content">The content with which to replace the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    public override IZptElement[] ReplaceWith(string content, bool interpretContentAsStructure)
    {
      var newNode = this.Import(content, interpretContentAsStructure);

      this.Node.ReplaceWith(newNode);

      return interpretContentAsStructure? new[] { new ZptXmlLinqElement(newNode, UnknownSourceFileInfo.Instance, this.OwnerDocument) } : new IZptElement[0];
    }

    /// <summary>
    /// Removes all children of the current element instance and replaces them with the given content.
    /// </summary>
    /// <param name="content">The content with which to replace the children of the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    public override void ReplaceChildrenWith(string content, bool interpretContentAsStructure)
    {
      var newNode = this.Import(content, interpretContentAsStructure);
      this.RemoveAllChildren();
      this.Node.Add(newNode);
    }

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the previous
    /// sibling before a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, before which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    public override IZptElement InsertBefore(IZptElement existing, IZptElement newChild)
    {
      ZptXmlLinqElement
        existingElement = ConvertTo<ZptXmlLinqElement>(existing),
        newChildElement = ConvertTo<ZptXmlLinqElement>(newChild);

      existingElement.Node.AddBeforeSelf(newChildElement.Node);
      return new ZptXmlLinqElement(newChildElement.Node, newChild.GetSourceInfo(), this.OwnerDocument, isImported: true);
    }

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the next
    /// sibling after a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, after which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    public override IZptElement InsertAfter(IZptElement existing, IZptElement newChild)
    {
      ZptXmlLinqElement
        existingElement = ConvertTo<ZptXmlLinqElement>(existing),
        newChildElement = ConvertTo<ZptXmlLinqElement>(newChild);

      existingElement.Node.AddAfterSelf(newChildElement.Node);
      return new ZptXmlLinqElement(newChildElement.Node, newChild.GetSourceInfo(), this.OwnerDocument, isImported: true);
    }

    /// <summary>
    /// Gets the element which is the parent of the current instance.
    /// </summary>
    /// <returns>The parent element.</returns>
    public override IZptElement GetParentElement()
    {
      var parent = this.Node.Parent;
      return (parent != null && parent.NodeType == XmlNodeType.Element)? new ZptXmlLinqElement(parent, this.GetSourceInfo(), this.OwnerDocument) : null;
    }

    /// <summary>
    /// Gets a collection of the child elements from the current source element.
    /// </summary>
    /// <returns>The children.</returns>
    public override IZptElement[] GetChildElements()
    {
      return this.Node.Elements()
        .Cast<XElement>()
        .Where(x => x.NodeType == XmlNodeType.Element)
        .Select(x => new ZptXmlLinqElement(x, this.GetSourceInfo(), this.OwnerDocument))
        .ToArray();
    }

    /// <summary>
    /// Gets a collection of the attributes present upon the current element.
    /// </summary>
    /// <returns>The attributes.</returns>
    public override IZptAttribute[] GetAttributes()
    {
      return this.Node.Attributes()
        .Cast<XAttribute>()
        .Select(x => new ZptXmlLinqAttribute(x))
        .ToArray();
    }

    /// <summary>
    /// Gets an attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public override IZptAttribute GetAttribute(ZptNamespace attributeNamespace, string name)
    {
      if(attributeNamespace == null)
      {
        throw new ArgumentNullException(nameof(attributeNamespace));
      }
      EnforceNameNotEmpty(name);

      string query;
      var nsManager = new XmlNamespaceManager(new NameTable());

      if(String.IsNullOrEmpty(attributeNamespace.Uri))
      {
        query = String.Concat("@", name);
      }
      else if(this.IsInNamespace(attributeNamespace))
      {
        nsManager.AddNamespace("search", attributeNamespace.Uri);
        query = String.Concat("@search:", name, "|", name);
      }
      else
      {
        nsManager.AddNamespace("search", attributeNamespace.Uri);
        query = String.Concat("@search:", name);
      }

      var xmlAttribute = ((IEnumerable) this.Node.XPathEvaluate(query, nsManager))
        .Cast<XAttribute>()
        .FirstOrDefault();

      return (xmlAttribute != null)? new ZptXmlLinqAttribute(xmlAttribute) : null;
    }

    /// <summary>
    /// Sets the value of an attribute.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    public override void SetAttribute(ZptNamespace attributeNamespace, string name, string value)
    {
      if(attributeNamespace == null)
      {
        throw new ArgumentNullException(nameof(attributeNamespace));
      }
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      this.Node.SetAttributeValue(XName.Get(name, attributeNamespace.Uri?? XNamespace.None.NamespaceName), value);
    }

    /// <summary>
    /// Removes a named attribute.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public override void RemoveAttribute(ZptNamespace attributeNamespace, string name)
    {
      if(attributeNamespace == null)
      {
        throw new ArgumentNullException(nameof(attributeNamespace));
      }
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      var attribs = this.Node.Attributes()
        .Cast<XAttribute>()
        .Where(x => x.Name.LocalName == name
               && (attributeNamespace.Uri == null
                   || x.Name.Namespace.NamespaceName == attributeNamespace.Uri))
        .ToArray();

      foreach(var attrib in attribs)
      {
        attrib.Remove();
      }
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public override IZptElement[] SearchChildrenByAttribute(ZptNamespace attributeNamespace, string name)
    {
      if(attributeNamespace == null)
      {
        throw new ArgumentNullException(nameof(attributeNamespace));
      }
      EnforceNameNotEmpty(name);

      string query;
      var nsManager = new XmlNamespaceManager(new NameTable());

      if(String.IsNullOrEmpty(attributeNamespace.Uri))
      {
        query = String.Concat(".//*[@", name, "]");
      }
      else
      {
        nsManager.AddNamespace("search", attributeNamespace.Uri);
        query = String.Concat(".//*[@search:", name, "]");
      }

      return ((IEnumerable) this.Node.XPathEvaluate(query, nsManager))
        .Cast<XElement>()
        .Select(x => new ZptXmlLinqElement(x, this.GetSourceInfo(), this.OwnerDocument))
        .ToArray();
    }

    /// <summary>
    /// Recursively searches for attributes with a given namespace or prefix and removes them from their parent
    /// element.
    /// </summary>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    public override void PurgeAttributes(ZptNamespace attributeNamespace)
    {
      if(attributeNamespace == null)
      {
        throw new ArgumentNullException(nameof(attributeNamespace));
      }

      var elements = ((IEnumerable) this.Node.XPathEvaluate(".//*"))
        .Cast<XElement>()
        .Union(new [] { (XElement) this.Node })
        .ToArray();

      var toRemove = (from ele in elements
                      from attrib in ele.Attributes().Cast<XAttribute>()
                      where
                        attrib.Name.Namespace.NamespaceName == attributeNamespace.Uri
                        || (attrib.Name.Namespace.NamespaceName == "http://www.w3.org/2000/xmlns/"
                            && attrib.Value == attributeNamespace.Uri)
                      select new { Element = ele, Attribute = attrib })
        .ToArray();

      foreach(var item in toRemove)
      {
        item.Attribute.Remove();
      }
    }

    /// <summary>
    /// Recursively searches for elements with a given namespace or prefix and removes them using the
    /// <see cref="Omit"/> behaviour.
    /// </summary>
    /// <param name="elementNamespace">The element namespace.</param>
    public override void PurgeElements(ZptNamespace elementNamespace)
    {
      var toRemove = ((IEnumerable) this.Node.XPathEvaluate(".//*"))
        .Cast<XElement>()
        .Union(new [] { this.Node })
        .Where(x => IsInNamespace(elementNamespace, x))
        .ToArray();

      foreach(var item in toRemove)
      {
        new ZptXmlLinqElement(item, this.GetSourceInfo(), this.OwnerDocument).Omit();
      }
    }

    /// <summary>
    /// Adds a new comment to the DOM immediately before the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public override void AddCommentBefore(string comment)
    {
      if(comment == null)
      {
        throw new ArgumentNullException(nameof(comment));
      }

      this.Node.AddBeforeSelf(new XComment(comment));
    }

    /// <summary>
    /// Adds a new comment to the DOM inside the current element as its first child.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public override void AddCommentInside(string comment)
    {
      if(comment == null)
      {
        throw new ArgumentNullException(nameof(comment));
      }

      this.Node.AddFirst(new XComment(comment));
    }

    /// <summary>
    /// Adds a new comment to the DOM immediately after the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public override void AddCommentAfter(string comment)
    {
      this.Node.AddAfterSelf(new XComment(comment));
    }

    /// <summary>
    /// Gets an element or attribute name based upon its prefix and name.
    /// </summary>
    /// <returns>The assembled name.</returns>
    /// <param name="prefix">The name prefix.</param>
    /// <param name="name">The name.</param>
    private string GetName(string prefix, string name)
    {
      EnforceNameNotEmpty(name);

      string output;

      if(String.IsNullOrEmpty(prefix))
      {
        output = name;
      }
      else
      {
        output = String.Concat(prefix, ":", name);
      }

      return output;
    }

    /// <summary>
    /// Clone this instance into a new Element instance, which may be manipulated without affecting the original.
    /// </summary>
    public override IZptElement Clone()
    {
      var clone = new XElement(this.Node);

      return new ZptXmlLinqElement(clone, this.GetSourceInfo(), this.OwnerDocument, this.IsRoot, true) {
        _filePosition = this.GetLineNumber()
      };
    }

    /// <summary>
    /// Gets the file location (typically a line number) for the current instance.
    /// </summary>
    /// <returns>The file location.</returns>
    protected override string GetNativeFileLocation()
    {
      var lineNumber = this.GetLineNumber();
      return lineNumber.HasValue? lineNumber.Value.ToString() : String.Empty;
    }

    /// <summary>
    /// Gets the current instance's line number.
    /// </summary>
    /// <returns>The line number.</returns>
    protected virtual int? GetLineNumber()
    {
      if(_filePosition.HasValue)
      {
        return _filePosition;
      }
      else
      {
        var lineInfo = ((IXmlLineInfo) this.Node);
        return lineInfo.HasLineInfo()? lineInfo.LineNumber : (int?) null;
      }
    }

    /// <summary>
    /// Gets the file location (typically a line number) for the end tag matched with the current instance.
    /// </summary>
    /// <returns>The end tag file location.</returns>
    protected override string GetNativeEndTagFileLocation()
    {
      string output = String.Empty;

      int? startTagLineNumber = GetLineNumber();

      if(startTagLineNumber.HasValue)
      {
        var outerXml = this.Node.ToString();
        output = (startTagLineNumber.Value + outerXml.Count(x => x == NEWLINE)).ToString();
      }

      return output;
    }

    /// <summary>
    /// Omits the current element, replacing it with its children.
    /// </summary>
    /// <returns>
    /// A collection of the <see cref="IZptElement"/> instances which were children of the element traversed
    /// </returns>
    public override IZptElement[] Omit()
    {
      var children = this.Node.Nodes().ToArray();

      foreach(var child in children)
      {
        this.Node.AddBeforeSelf(child);
      }
      this.Node.Remove();

      return children
        .Where(x => x.NodeType == XmlNodeType.Element)
        .Select(x => new ZptXmlLinqElement(x, this.GetSourceInfo(), this.OwnerDocument))
        .ToArray();
    }

    /// <summary>
    /// Removes the current element from the DOM.
    /// </summary>
    public override void Remove()
    {
      this.Node.Remove();
    }

    /// <summary>
    /// Removes all child elements from the current element.
    /// </summary>
    public override void RemoveAllChildren()
    {
      this.Node.RemoveNodes();
    }

    /// <summary>
    /// Determines whether or not the current instance is in the specified namespace.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is in the specified namespace; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="nSpace">The namespace for which to test.</param>
    public override bool IsInNamespace(ZptNamespace nSpace)
    {
      return this.IsInNamespace(nSpace, this.Node);
    }

    /// <summary>
    /// Determines whether this instance is from same document as the specified element.
    /// </summary>
    /// <returns><c>true</c> if this instance is from same document as the specified element; otherwise, <c>false</c>.</returns>
    /// <param name="other">The element to test.</param>
    public override bool IsFromSameDocumentAs(IZptElement other)
    {
      if(other == null)
      {
        throw new ArgumentNullException(nameof(other));
      }

      var typedOther = ConvertTo<ZptXmlLinqElement>(other);
      return this.Node.Document == typedOther.Node.Document;
    }

    private bool IsInNamespace(ZptNamespace nSpace, XElement node)
    {
      if(nSpace == null)
      {
        throw new ArgumentNullException(nameof(nSpace));
      }

      bool output;

      if(nSpace.Uri != null)
      {
        output = node.Name.Namespace.NamespaceName == nSpace.Uri;
      }
      else
      {
        output = String.IsNullOrEmpty(node.Name.Namespace.NamespaceName);
      }

      return output;
    }

    /// <summary>
    /// Gets the parent of the current <see cref="Node"/>.
    /// </summary>
    /// <returns>The parent node.</returns>
    private XElement GetParent()
    {
      var output = this.Node.Parent;

      EnforceParentNodeNotNull(output);

      return output;
    }

    /// <summary>
    /// Imports the given text as a new <c>XElement</c>.
    /// </summary>
    /// <returns>A collection of the imported nodes.</returns>
    /// <param name="text">The text to import.</param>
    /// <param name="treatAsXml">If set to <c>true</c> then the text is treated as XML.</param>
    private XNode Import(string text, bool treatAsXml)
    {
      string toImport = text?? String.Empty;

      XNode output;

      if(treatAsXml)
      {
        output = XElement.Parse(toImport);
      }
      else
      {
        output = new XText(text);
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.DocumentProviders.ZptXmlLinqElement"/> class.
    /// </summary>
    /// <param name="node">The source XML Node.</param>
    /// <param name="sourceFile">Information about the element's source file.</param>
    /// <param name="isRoot">Whether or not this is the root element.</param>
    /// <param name="isImported">Whether or not this element is imported.</param>
    /// <param name="ownerDocument">The ZPT document which owns the element.</param>
    public ZptXmlLinqElement(XNode node,
                             ISourceInfo sourceFile,
                             IZptDocument ownerDocument,
                             bool isRoot = false,
                             bool isImported = false) : base(sourceFile, isRoot, isImported, ownerDocument)
    {
      if(node == null)
      {
        throw new ArgumentNullException(nameof(node));
      }

      _node = node as XElement;

      if(_node.NodeType == XmlNodeType.Document)
      {
        _node = node.Document.Root;
      }

      EnforceNodeType("XML", XmlNodeType.Element, _node.NodeType);
    }

    #endregion
  }
}

