using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using CSF.Zpt.Resources;
using System.Web;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.DocumentProviders
{
  /// <summary>
  /// Implementation of <see cref="ZptElement"/> based on documents parsed using the HTML Agility Pack.
  /// </summary>
  public class ZptHtmlElement : ZptElement
  {
    #region constants

    private const string
      INDENT_PATTERN        = @"([ \t]+)$",
      HTML_COMMENT_START    = "<!--",
      HTML_COMMENT_END      = "-->",
      PREFIX_SEPARATOR      = ":",
      XMLNS_ATTRIBUTE       = "xmlns";
    private const char NEWLINE = '\n';

    #endregion

    #region fields

    private HtmlNode _node;
    private int? _cachedHashCode, _filePosition;

    #endregion

    #region properties

    /// <summary>
    /// Gets the HTML node object wrapped by the current instance.
    /// </summary>
    /// <value>The node.</value>
    public HtmlNode Node
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

    /// <summary>
    /// Gets a value indicating whether this instance has a parent element or not.
    /// </summary>
    /// <value><c>true</c> if this instance has a parent element; otherwise, <c>false</c>.</value>
    public override bool HasParent
    {
      get {
        return this.Node.ParentNode != null;
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
        return typeof(ZptHtmlDocument);
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
    /// Returns a <see cref="System.String"/> that represents the current <see cref="ZptHtmlElement"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="ZptHtmlElement"/>.</returns>
    public override string ToString()
    {
      return this.Node.OuterHtml;
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.ZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.DocumentProviders.ZptHtmlElement"/>.
    /// </summary>
    /// <param name="other">
    /// The <see cref="CSF.Zpt.Rendering.ZptElement"/> to compare with the current
    /// <see cref="CSF.Zpt.DocumentProviders.ZptHtmlElement"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.ZptElement"/> is equal to the current
    /// <see cref="CSF.Zpt.DocumentProviders.ZptHtmlElement"/>; otherwise, <c>false</c>.
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
        var ele = other as ZptHtmlElement;
        output = (ele != null && ele.Node == this.Node);
      }

      return output;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.DocumentProviders.ZptHtmlElement"/> object.
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
      var repl = ConvertTo<ZptHtmlElement>(replacement);

      try
      {
        var parent = this.GetParent();
        return new ZptHtmlElement(parent.ReplaceChild(repl.Node, this.Node),
                                  repl.GetSourceInfo(),
                                OwnerDocument,
                                isImported: true);
      }
      catch(InvalidOperationException)
      {
        this.Node.Remove();
        return new ZptHtmlElement(repl.Node,
                                  repl.GetSourceInfo(),
                                OwnerDocument,
                                  isImported: true);
      }
    }

    /// <summary>
    /// Replaces the current element instance with the given content.
    /// </summary>
    /// <returns>A collection of <see cref="IZptElement"/>, indicating the element(s) which replaced the current instance.</returns>
    /// <param name="content">The content with which to replace the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    public override IZptElement[] ReplaceWith(string content, bool interpretContentAsStructure)
    {
      var newNodes = this.Import(content, interpretContentAsStructure);

      var parent = this.GetParent();
      foreach(var node in newNodes)
      {
        parent.InsertBefore(node, this.Node);
      }
      this.Remove();

      return newNodes
        .Where(x => x.NodeType == HtmlNodeType.Element)
        .Select(x => new ZptHtmlElement(x, UnknownSourceFileInfo.Instance, this.OwnerDocument))
        .ToArray();
    }

    /// <summary>
    /// Removes all children of the current element instance and replaces them with the given content.
    /// </summary>
    /// <param name="content">The content with which to replace the children of the current element.</param>
    /// <param name="interpretContentAsStructure">If set to <c>true</c> then the content is interpreted as structure.</param>
    public override void ReplaceChildrenWith(string content, bool interpretContentAsStructure)
    {
      var newNodes = this.Import(content, interpretContentAsStructure);

      this.RemoveAllChildren();
      foreach(var node in newNodes)
      {
        this.Node.AppendChild(node);
      }
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
      
      ZptHtmlElement
        existingElement = ConvertTo<ZptHtmlElement>(existing),
        newChildElement = ConvertTo<ZptHtmlElement>(newChild);

      HtmlNode existingNode = existingElement.Node;

      var prevNode = existingNode.PreviousSibling;
      if(prevNode.NodeType == HtmlNodeType.Text)
      {
        var lastLine = prevNode.InnerText
          .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
          .Last();
        if(String.IsNullOrWhiteSpace(lastLine))
        {
          var newLine = this.Node.OwnerDocument.CreateTextNode(String.Concat(System.Environment.NewLine, lastLine));
          existingNode = this.Node.InsertBefore(newLine, existingNode);
        }
      }

      var output = this.Node.InsertBefore(newChildElement.Node, existingNode);
      return new ZptHtmlElement(output, newChild.GetSourceInfo(), this.OwnerDocument, isImported: true);
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
      ZptHtmlElement
        existingElement = ConvertTo<ZptHtmlElement>(existing),
        newChildElement = ConvertTo<ZptHtmlElement>(newChild);

      var output = this.Node.InsertAfter(newChildElement.Node, existingElement.Node);
      return new ZptHtmlElement(output, newChild.GetSourceInfo(), this.OwnerDocument, isImported: true);
    }

    /// <summary>
    /// Gets the element which is the parent of the current instance.
    /// </summary>
    /// <returns>The parent element.</returns>
    public override IZptElement GetParentElement()
    {
      var parent = this.Node.ParentNode;
      return (parent != null && parent.NodeType == HtmlNodeType.Element)? new ZptHtmlElement(parent, this.GetSourceInfo(), this.OwnerDocument) : null;
    }

    /// <summary>
    /// Gets a collection of the child elements from the current source element.
    /// </summary>
    /// <returns>The children.</returns>
    public override IZptElement[] GetChildElements()
    {
      return this.Node.ChildNodes
        .Where(x => x.NodeType == HtmlNodeType.Element)
        .Select(x => new ZptHtmlElement(x, this.GetSourceInfo(), this.OwnerDocument))
        .ToArray();
    }

    /// <summary>
    /// Gets a collection of the attributes present upon the current element.
    /// </summary>
    /// <returns>The attributes.</returns>
    public override IZptAttribute[] GetAttributes()
    {
      return this.Node.Attributes
        .Select(x => new ZptHtmlAttribute(x))
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
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      var attribName = this.IsInNamespace(attributeNamespace)? name : GetNameWithPrefix(attributeNamespace, name);
      var htmlAttribute = this.Node.Attributes
        .FirstOrDefault(x => {
          bool output;
          string nameWithPrefix = GetNameWithPrefix(attributeNamespace, name);

          if(this.IsInNamespace(attributeNamespace))
          {
            output = (x.Name == attribName || x.Name == nameWithPrefix);
          }
          else
          {
            output = x.Name == nameWithPrefix;
          }

          return output;
        });

      return (htmlAttribute != null)? new ZptHtmlAttribute(htmlAttribute) : null;
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

      var formattedName = GetNameWithPrefix(attributeNamespace, name);
      this.Node.SetAttributeValue(formattedName, value);
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

      var formattedName = GetNameWithPrefix(attributeNamespace, name);
      var attribs = this.Node.Attributes.Where(x => x.Name == formattedName).ToArray();
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
      string
        attribName = GetNameWithPrefix(attributeNamespace, name),
        prefix = GetPrefix(attributeNamespace);

      return (from node in this.Node.Descendants()
              from attrib in node.Attributes
              where
                attrib.Name == attribName
                || (node.Name.StartsWith(prefix)
                    && attrib.Name == name)
              select new ZptHtmlElement(node, this.GetSourceInfo(), this.OwnerDocument))
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

      var nodes = this.Node
        .Descendants()
        .Union(new [] { this.Node })
        .ToArray();

      var toRemove = (from node in nodes
                      from attrib in node.Attributes
                      where
                        attrib.Name.StartsWith(String.Concat(attributeNamespace.Prefix, PREFIX_SEPARATOR))
                        || attrib.Name == String.Concat(XMLNS_ATTRIBUTE, PREFIX_SEPARATOR, attributeNamespace.Prefix)
                      select attrib)
        .ToArray();

      foreach(var item in toRemove)
      {
        item.Remove();
      }
    }

    /// <summary>
    /// Recursively searches for elements with a given namespace or prefix and removes them using the
    /// <see cref="Omit"/> behaviour.
    /// </summary>
    /// <param name="elementNamespace">The element namespace.</param>
    public override void PurgeElements(ZptNamespace elementNamespace)
    {
      var toRemove = this.Node
        .Descendants()
        .Union(new [] { this.Node })
        .Where(x => IsInNamespace(elementNamespace, x))
        .ToArray();

      foreach(var item in toRemove)
      {
        new ZptHtmlElement(item, this.GetSourceInfo(), this.OwnerDocument).Omit();
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

      string commentText = String.Concat(HTML_COMMENT_START, comment, HTML_COMMENT_END, String.Empty);
      var commentNode = this.Node.OwnerDocument.CreateComment(commentText);

      if(this.HasParent)
      {
        this.GetParent().InsertBefore(commentNode, this.Node);
      }
      else
      {
        this.Node.PrependChild(commentNode);
      }
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

      string commentText = String.Concat(HTML_COMMENT_START, comment, HTML_COMMENT_END, String.Empty);
      var commentNode = this.Node.OwnerDocument.CreateComment(commentText);

      this.Node.InsertBefore(commentNode, this.Node.FirstChild);
    }

    /// <summary>
    /// Adds a new comment to the DOM immediately after the current element.
    /// </summary>
    /// <param name="comment">The comment text.</param>
    public override void AddCommentAfter(string comment)
    {
      string commentText = String.Concat(HTML_COMMENT_START, comment, HTML_COMMENT_END, String.Empty);
      var commentNode = this.Node.OwnerDocument.CreateComment(commentText);

      if(this.HasParent)
      {
        this.GetParent().InsertAfter(commentNode, this.Node);
      }
      else
      {
        this.Node.AppendChild(commentNode);
      }
    }

    /// <summary>
    /// Clone this instance into a new Element instance, which may be manipulated without affecting the original.
    /// </summary>
    public override IZptElement Clone()
    {
      var clone = this.Node.Clone();

      return new ZptHtmlElement(clone, this.GetSourceInfo(), this.OwnerDocument, this.IsRoot, true) {
        _filePosition = _filePosition.HasValue? _filePosition : this.Node.Line,
      };
    }

    /// <summary>
    /// Gets the file location (typically a line number) for the current instance.
    /// </summary>
    /// <returns>The file location.</returns>
    protected override string GetNativeFileLocation()
    {
      var loc = GetStartTagFileLocation();
      return loc.HasValue? loc.Value.ToString() : String.Empty;
    }

    /// <summary>
    /// Gets the file location (typically a line number) for the end tag matched with the current instance.
    /// </summary>
    /// <returns>The end tag file location.</returns>
    protected override string GetNativeEndTagFileLocation()
    {
      string output = String.Empty;
      var loc = GetStartTagFileLocation();
      if(loc.HasValue)
      {
        var outerHtml = this.Node.OuterHtml;
        output = (loc.Value + outerHtml.Count(x => x == NEWLINE)).ToString();
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
      var children = this.Node.ChildNodes.ToArray();
      var parent = this.GetParent();

      foreach(var child in children)
      {
        parent.InsertBefore(child, this.Node);
      }
      this.Node.Remove();

      return children
        .Where(x => x.NodeType == HtmlNodeType.Element)
        .Select(x => new ZptHtmlElement(x, this.GetSourceInfo(), this.OwnerDocument))
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
      this.Node.RemoveAllChildren();
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

      var typedOther = ConvertTo<ZptHtmlElement>(other);

      return this.Node.OwnerDocument == typedOther.Node.OwnerDocument;
    }

    /// <summary>
    /// Creates a new <see cref="IZptDocument"/> which contains only the current element and its children.
    /// </summary>
    /// <returns>A new document instance.</returns>
    public override IZptDocument CreateDocumentFromThisElement()
    {
      var doc = new HtmlDocument();
      doc.DocumentNode.AppendChild(Node);

      return new ZptHtmlDocument(doc, GetSourceInfo());
    }

    private bool IsInNamespace(ZptNamespace nSpace, HtmlNode node)
    {
      if(nSpace == null)
      {
        throw new ArgumentNullException(nameof(nSpace));
      }

      bool output;

      if(nSpace.Prefix != null)
      {
        output = node.Name.StartsWith(String.Concat(nSpace.Prefix, PREFIX_SEPARATOR));
      }
      else
      {
        output = !node.Name.Contains(PREFIX_SEPARATOR);
      }

      return output;
    }

    /// <summary>
    /// Gets the parent of the current <see cref="Node"/>.
    /// </summary>
    /// <returns>The parent node.</returns>
    private HtmlNode GetParent()
    {
      var output = this.Node.ParentNode;

      EnforceParentNodeNotNull(output);

      return output;
    }

    /// <summary>
    /// Imports the given text as a new <c>HtmlNode</c>.
    /// </summary>
    /// <returns>A collection of the imported nodes.</returns>
    /// <param name="text">The text to import.</param>
    /// <param name="treatAsHtml">If set to <c>true</c> then the text is treated as HTML.</param>
    private HtmlNode[] Import(string text, bool treatAsHtml)
    {
      string toImport = text?? String.Empty;

      HtmlNode[] output;

      if(treatAsHtml)
      {
        var doc = new HtmlDocument();
        doc.LoadHtml(toImport);
        output = doc.DocumentNode.ChildNodes.ToArray();
      }
      else
      {
        var sanitised = HtmlEntity.Entitize(toImport, true, true);
        output = new HtmlNode[] { this.Node.OwnerDocument.CreateTextNode(sanitised) };
      }

      return output;
    }

    /// <summary>
    /// Gets an element or attribute name, adding a namespace prefix where appropriate.
    /// </summary>
    /// <returns>The formatted name.</returns>
    /// <param name="nSpace">A namespace.</param>
    /// <param name="name">An element or attribute name.</param>
    internal static string GetNameWithPrefix(ZptNamespace nSpace, string name)
    {
      if(nSpace == null)
      {
        throw new ArgumentNullException(nameof(nSpace));
      }
      EnforceNameNotEmpty(name);

      return (nSpace.Prefix != null)? String.Concat(nSpace.Prefix, PREFIX_SEPARATOR, name) : name;
    }

    /// <summary>
    /// Gets the prefix from a given namespace.
    /// </summary>
    /// <returns>The prefix.</returns>
    /// <param name="nSpace">An element or attribute namespace.</param>
    internal static string GetPrefix(ZptNamespace nSpace)
    {
      if(nSpace == null)
      {
        throw new ArgumentNullException(nameof(nSpace));
      }

      return (nSpace.Prefix != null)? String.Concat(nSpace.Prefix, PREFIX_SEPARATOR) : String.Empty;
    }

    private int? GetStartTagFileLocation()
    {
      int line = _filePosition.HasValue? _filePosition.Value : this.Node.Line;
      return (line >= 1)? line : (int?) null;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.DocumentProviders.ZptHtmlElement"/> class.
    /// </summary>
    /// <param name="node">The source HTML node.</param>
    /// <param name="sourceFile">Information about the element's source file.</param>
    /// <param name="isRoot">Whether or not this is the root element.</param>
    /// <param name="isImported">Whether or not this element is imported.</param>
    /// <param name="ownerDocument">The ZPT document which owns the element.</param>
    public ZptHtmlElement(HtmlNode node,
                          ISourceInfo sourceFile,
                          IZptDocument ownerDocument,
                          bool isRoot = false,
                          bool isImported = false) : base(sourceFile, isRoot, isImported, ownerDocument)
    {
      if(node == null)
      {
        throw new ArgumentNullException(nameof(node));
      }

      _node = node;
    }

    #endregion
  }
}

