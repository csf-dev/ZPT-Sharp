using System;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using CSF.Zpt.Resources;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="ZptElement"/> based on documents parsed using <c>System.Xml</c>.
  /// </summary>
  public class ZptXmlElement : ZptElement
  {
    #region constants

    private const string
      INDENT_PATTERN        = @"([ \t]+)$",
      XML_COMMENT_START    = "<!-- ",
      XML_COMMENT_END      = " -->\n";

    private static readonly Regex Indent = new Regex(INDENT_PATTERN, RegexOptions.Compiled);

    #endregion

    #region fields

    private XmlNode _node;
    private int? _cachedHashCode;

    #endregion

    #region properties

    /// <summary>
    /// Gets the XML node object wrapped by the current instance.
    /// </summary>
    /// <value>The node.</value>
    public virtual XmlNode Node
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
        return this.Node.Name;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="ZptXmlElement"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="ZptXmlElement"/>.</returns>
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
    /// <see cref="CSF.Zpt.Rendering.ZptXmlElement"/>.
    /// </summary>
    /// <param name="other">
    /// The <see cref="CSF.Zpt.Rendering.ZptElement"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.ZptXmlElement"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.ZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptXmlElement"/>; otherwise, <c>false</c>.
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
        var ele = other as ZptXmlElement;
        output = (ele != null && ele.Node == this.Node);
      }

      return output;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.Rendering.ZptXmlElement"/> object.
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
    public override ZptElement ReplaceWith(ZptElement replacement)
    {
      var repl = replacement as ZptXmlElement;
      if(repl == null)
      {
        string message = String.Format(ExceptionMessages.ElementMustBeCorrectType,
                                       typeof(ZptXmlElement).Name);
        throw new ArgumentException(message, "replacement");
      }

      XmlNode importedNode;

      if(this.Node.ParentNode != null)
      {
        importedNode = this.Node.OwnerDocument.ImportNode(repl.Node, true);
        this.GetParent().ReplaceChild(importedNode, this.Node);
      }
      else
      {
        var newDocument = new XmlDocument();
        importedNode = newDocument.ImportNode(repl.Node, true);
        newDocument.AppendChild(importedNode);
      }

      return new ZptXmlElement(importedNode,
                            repl.SourceFile,
                            isImported: true);
    }

    /// <summary>
    /// Replaces the current element instance with the given content.
    /// </summary>
    /// <returns>A collection of <see cref="ZptElement"/>, indicating the element(s) which replaced the current instance.</returns>
    /// <param name="content">The content with which to replace the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    public override ZptElement[] ReplaceWith(string content, bool interpretContentAsStructure)
    {
      var newNode = this.Import(content, interpretContentAsStructure);

      var output = this.GetParent().InsertBefore(newNode, this.Node);
      this.Remove();

      return interpretContentAsStructure? new[] { new ZptXmlElement(output, this.SourceFile) } : new ZptElement[0];
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
      this.Node.AppendChild(newNode);
    }

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the previous
    /// sibling before a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, before which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    public override ZptElement InsertBefore(ZptElement existing, ZptElement newChild)
    {
      ZptXmlElement
        existingElement = existing as ZptXmlElement,
        newChildElement = newChild as ZptXmlElement;
      if(existingElement == null)
      {
        string message = String.Format(ExceptionMessages.ElementMustBeCorrectType,
                                       typeof(ZptXmlElement).Name);
        throw new ArgumentException(message, "existing");
      }
      if(newChildElement == null)
      {
        string message = String.Format(ExceptionMessages.ElementMustBeCorrectType,
                                       typeof(ZptXmlElement).Name);
        throw new ArgumentException(message, "newChild");
      }

      var importedNode = this.Node.OwnerDocument.ImportNode(newChildElement.Node, true);

      var output = this.Node.InsertBefore(importedNode, existingElement.Node);
      return new ZptXmlElement(output, newChild.SourceFile, isImported: true);
    }

    /// <summary>
    /// Inserts a new child element into the current element's child elements.  The new child will be the next
    /// sibling after a given existing child.
    /// </summary>
    /// <returns>The newly-added element.</returns>
    /// <param name="existing">An existing child element, after which the child will be inserted.</param>
    /// <param name="newChild">The new child element to insert.</param>
    public override ZptElement InsertAfter(ZptElement existing, ZptElement newChild)
    {
      ZptXmlElement
        existingElement = existing as ZptXmlElement,
        newChildElement = newChild as ZptXmlElement;
      if(existingElement == null)
      {
        string message = String.Format(ExceptionMessages.ElementMustBeCorrectType,
                                       typeof(ZptXmlElement).Name);
        throw new ArgumentException(message, "existing");
      }
      if(newChildElement == null)
      {
        string message = String.Format(ExceptionMessages.ElementMustBeCorrectType,
                                       typeof(ZptXmlElement).Name);
        throw new ArgumentException(message, "newChild");
      }

      var importedNode = this.Node.OwnerDocument.ImportNode(newChildElement.Node, true);

      var output = this.Node.InsertAfter(importedNode, existingElement.Node);
      return new ZptXmlElement(output, newChild.SourceFile, isImported: true);
    }

    /// <summary>
    /// Gets the element which is the parent of the current instance.
    /// </summary>
    /// <returns>The parent element.</returns>
    public override ZptElement GetParentElement()
    {
      var parent = this.Node.ParentNode;
      return (parent != null && parent.NodeType == XmlNodeType.Element)? new ZptXmlElement(parent, this.SourceFile) : null;
    }

    /// <summary>
    /// Gets a collection of the child elements from the current source element.
    /// </summary>
    /// <returns>The children.</returns>
    public override ZptElement[] GetChildElements()
    {
      return this.Node.ChildNodes
        .Cast<XmlNode>()
        .Where(x => x.NodeType == XmlNodeType.Element)
        .Select(x => new ZptXmlElement(x, this.SourceFile))
        .ToArray();
    }

    /// <summary>
    /// Gets a collection of the attributes present upon the current element.
    /// </summary>
    /// <returns>The attributes.</returns>
    public override ZptAttribute[] GetAttributes()
    {
      return this.Node.Attributes
        .Cast<System.Xml.XmlAttribute>()
        .Select(x => new ZptXmlAttribute(x))
        .ToArray();
    }

    /// <summary>
    /// Gets an attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public override ZptAttribute GetAttribute(ZptNamespace attributeNamespace, string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }
      else if(name.Length == 0)
      {
        throw new ArgumentException(ExceptionMessages.NameMustNotBeEmptyString, "name");
      }
      if(attributeNamespace == null)
      {
        throw new ArgumentNullException(nameof(attributeNamespace));
      }

      string query;
      var nsManager = new XmlNamespaceManager(new NameTable());

      if(String.IsNullOrEmpty(attributeNamespace.Uri) || this.IsInNamespace(attributeNamespace))
      {
        query = String.Concat("@", name);
      }
      else
      {
        nsManager.AddNamespace("search", attributeNamespace.Uri);
        query = String.Concat("@search:", name);
      }

      var xmlAttribute = this.Node.SelectNodes(query, nsManager)
        .Cast<System.Xml.XmlAttribute>()
        .FirstOrDefault();

      return (xmlAttribute != null)? new ZptXmlAttribute(xmlAttribute) : null;
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

      var attribs = this.Node.Attributes.Cast<XmlAttribute>()
        .Where(x => x.LocalName == name
                    && (attributeNamespace.Uri == null
                        || x.NamespaceURI == attributeNamespace.Uri))
        .ToArray();

      if(!attribs.Any())
      {
        attribs = new [] { this.Node.OwnerDocument.CreateAttribute(attributeNamespace.Prefix, name, attributeNamespace.Uri) };
        this.Node.Attributes.Append(attribs[0]);
      }

      foreach(var attrib in attribs)
      {
        attrib.Value = value;
      }
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

      var attribs = this.Node.Attributes
        .Cast<XmlAttribute>()
        .Where(x => x.LocalName == name
                    && (attributeNamespace.Uri == null
                        || x.NamespaceURI == attributeNamespace.Uri))
        .ToArray();
      foreach(var attrib in attribs)
      {
        this.Node.Attributes.Remove(attrib);
      }
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public override ZptElement[] SearchChildrenByAttribute(ZptNamespace attributeNamespace, string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }
      else if(name.Length == 0)
      {
        throw new ArgumentException(ExceptionMessages.NameMustNotBeEmptyString, "name");
      }

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

      return this.Node.SelectNodes(query, nsManager)
        .Cast<XmlNode>()
        .Select(x => new ZptXmlElement(x, this.SourceFile))
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

      var elements = this.Node
        .SelectNodes(".//*")
        .Cast<System.Xml.XmlElement>()
        .Union(new [] { (System.Xml.XmlElement) this.Node })
        .ToArray();

      var toRemove = (from ele in elements
                      from attrib in ele.Attributes.Cast<System.Xml.XmlAttribute>()
                      where
                        attrib.NamespaceURI == attributeNamespace.Uri
                        || (attrib.NamespaceURI == "http://www.w3.org/2000/xmlns/"
                            && attrib.Value == attributeNamespace.Uri)
                      select new { Element = ele, Attribute = attrib })
        .ToArray();

      foreach(var item in toRemove)
      {
        item.Element.RemoveAttributeNode(item.Attribute);
      }
    }

    public override void PurgeElements(ZptNamespace elementNamespace)
    {
      var toRemove = this.Node
        .SelectNodes(".//*")
        .Cast<System.Xml.XmlElement>()
        .Union(new [] { this.Node })
        .Where(x => IsInNamespace(elementNamespace, x))
        .ToArray();

      foreach(var item in toRemove)
      {
        new ZptXmlElement(item, this.SourceFile).Omit();
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

      var parent = this.GetParent();
      string indent = String.Empty;

      var previousNode = this.Node.PreviousSibling;
      if(previousNode != null
         && previousNode.NodeType == XmlNodeType.Text)
      {
        XmlText previousText = (XmlText) previousNode;
        var indentMatch = Indent.Match(previousText.Value);
        if(indentMatch.Success)
        {
          indent = indentMatch.Value;
        }
      }

      var commentNode = this.Node.OwnerDocument.CreateComment(comment);

      parent.InsertBefore(commentNode, this.Node);
    }

    /// <summary>
    /// Gets an element or attribute name based upon its prefix and name.
    /// </summary>
    /// <returns>The assembled name.</returns>
    /// <param name="prefix">The name prefix.</param>
    /// <param name="name">The name.</param>
    private string GetName(string prefix, string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }
      else if(name.Length == 0)
      {
        throw new ArgumentException(ExceptionMessages.NameMustNotBeEmptyString, "name");
      }

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
    public override ZptElement Clone()
    {
      var clone = _node.Clone();

      return new ZptXmlElement(clone, this.SourceFile);
    }

    /// <summary>
    /// Gets the file location (typically a line number) for the current instance.
    /// </summary>
    /// <returns>The file location.</returns>
    public override string GetFileLocation()
    {
      return null;
    }

    /// <summary>
    /// Omits the current element, replacing it with its children.
    /// </summary>
    /// <returns>
    /// A collection of the <see cref="ZptElement"/> instances which were children of the element traversed
    /// </returns>
    public override ZptElement[] Omit()
    {
      var children = this.Node.ChildNodes.Cast<XmlNode>().ToArray();
      var parent = this.GetParent();

      foreach(var child in children)
      {
        parent.InsertBefore(child, this.Node);
      }
      parent.RemoveChild(this.Node);

      return children
        .Where(x => x.NodeType == XmlNodeType.Element)
        .Select(x => new ZptXmlElement(x, this.SourceFile))
        .ToArray();
    }

    /// <summary>
    /// Removes the current element from the DOM.
    /// </summary>
    public override void Remove()
    {
      this.GetParent().RemoveChild(this.Node);
    }

    /// <summary>
    /// Removes all child elements from the current element.
    /// </summary>
    public override void RemoveAllChildren()
    {
      var children = this.Node.ChildNodes.Cast<XmlNode>().ToArray();
      foreach(var child in children)
      {
        this.Node.RemoveChild(child);
      }
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

    private bool IsInNamespace(ZptNamespace nSpace, XmlNode node)
    {
      if(nSpace == null)
      {
        throw new ArgumentNullException(nameof(nSpace));
      }

      bool output;

      if(nSpace.Uri != null)
      {
        output = node.NamespaceURI == nSpace.Uri;
      }
      else
      {
        output = String.IsNullOrEmpty(node.NamespaceURI);
      }

      return output;
    }

    /// <summary>
    /// Gets the parent of the current <see cref="Node"/>.
    /// </summary>
    /// <returns>The parent node.</returns>
    private XmlNode GetParent()
    {
      var output = this.Node.ParentNode;

      if(output == null)
      {
        throw new InvalidOperationException(ExceptionMessages.CannotGetParentFromRootNode);
      }

      return output;
    }

    /// <summary>
    /// Imports the given text as a new <c>XmlNode</c>.
    /// </summary>
    /// <returns>A collection of the imported nodes.</returns>
    /// <param name="text">The text to import.</param>
    /// <param name="treatAsXml">If set to <c>true</c> then the text is treated as XML.</param>
    private XmlNode Import(string text, bool treatAsXml)
    {
      string toImport = text?? String.Empty;

      XmlNode output;

      if(treatAsXml)
      {
        var doc = new XmlDocument();
        doc.LoadXml(toImport);
        output = this.Node.OwnerDocument.ImportNode(doc.DocumentElement, true);
      }
      else
      {
        output = this.Node.OwnerDocument.CreateTextNode(toImport);
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ZptXmlElement"/> class.
    /// </summary>
    /// <param name="node">The source XML Node.</param>
    /// <param name="sourceFile">Information about the element's source file.</param>
    /// <param name="isRoot">Whether or not this is the root element.</param>
    /// <param name="isImported">Whether or not this element is imported.</param>
    public ZptXmlElement(XmlNode node,
                         SourceFileInfo sourceFile,
                         bool isRoot = false,
                         bool isImported = false) : base(sourceFile, isRoot, isImported)
    {
      if(node == null)
      {
        throw new ArgumentNullException(nameof(node));
      }

      XmlNode actualNode;

      if(node.NodeType == XmlNodeType.Document)
      {
        actualNode = node.FirstChild;
      }
      else if(node.NodeType != XmlNodeType.Element)
      {
        string message = String.Format(ExceptionMessages.IncorrectWrappedNodeType, "XML", "element", node.NodeType);
        throw new ArgumentException(message, "node");
      }
      else
      {
        actualNode = node;
      }

      _node = actualNode;
    }

    #endregion
  }
}

