using System;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

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
    /// Replaces the current element in its respective DOM with the given replacement.
    /// </summary>
    /// <returns>A reference to the replacement element, in its new DOM.</returns>
    /// <param name="replacement">Replacement.</param>
    public override ZptElement ReplaceWith(ZptElement replacement)
    {
      var repl = replacement as ZptXmlElement;
      if(repl == null)
      {
        throw new ArgumentException("The replacement must be a non-null instance of XmlElement.",
                                    "replacement");
      }

      var importedNode = this.Node.OwnerDocument.ImportNode(repl.Node, true);

      this.GetParent().ReplaceChild(importedNode, this.Node);

      return new ZptXmlElement(importedNode,
                            repl.SourceFile,
                            isImported: true);
    }

    /// <summary>
    /// Gets the element which is the parent of the current instance.
    /// </summary>
    /// <returns>The parent element.</returns>
    public override ZptElement GetParentElement()
    {
      return (this.Node.ParentNode != null)? new ZptXmlElement(this.Node.ParentNode, this.SourceFile) : null;
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
    /// <param name="prefix">The attribute prefix.</param>
    /// <param name="name">The attribute name.</param>
    public override ZptAttribute GetAttribute(string attributeNamespace, string prefix, string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException("name");
      }
      else if(name.Length == 0)
      {
        throw new ArgumentException("Name must not be an empty string.", "name");
      }

      string query;
      var nsManager = new XmlNamespaceManager(new NameTable());

      if(String.IsNullOrEmpty(attributeNamespace))
      {
        query = String.Concat("@", name);
      }
      else
      {
        nsManager.AddNamespace("search", attributeNamespace);
        query = String.Concat("@search:", name);
      }

      var xmlAttribute = this.Node.SelectNodes(query, nsManager)
        .Cast<System.Xml.XmlAttribute>()
        .FirstOrDefault();

      return (xmlAttribute != null)? new ZptXmlAttribute(xmlAttribute) : null;
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="prefix">The attribute prefix.</param>
    /// <param name="name">The attribute name.</param>
    public override ZptElement[] SearchChildrenByAttribute(string attributeNamespace, string prefix, string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException("name");
      }
      else if(name.Length == 0)
      {
        throw new ArgumentException("Name must not be an empty string.", "name");
      }

      string query;
      var nsManager = new XmlNamespaceManager(new NameTable());

      if(String.IsNullOrEmpty(attributeNamespace))
      {
        query = String.Concat(".//*[@", name, "]");
      }
      else
      {
        nsManager.AddNamespace("search", attributeNamespace);
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
    /// <param name="prefix">The attribute prefix.</param>
    public override void PurgeAttributes(string attributeNamespace, string prefix)
    {
      if(attributeNamespace == null)
      {
        throw new ArgumentNullException("attributeNamespace");
      }

      var elements = this.Node
        .SelectNodes(".//*")
        .Cast<System.Xml.XmlElement>()
        .Union(new [] { (System.Xml.XmlElement) this.Node })
        .ToArray();

      var toRemove = (from ele in elements
                      from attrib in ele.Attributes.Cast<System.Xml.XmlAttribute>()
                      where
                        attrib.NamespaceURI == attributeNamespace
                        || (attrib.NamespaceURI == "http://www.w3.org/2000/xmlns/"
                            && attrib.Value == attributeNamespace)
                      select new { Element = ele, Attribute = attrib })
        .ToArray();

      foreach(var item in toRemove)
      {
        item.Element.RemoveAttributeNode(item.Attribute);
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
        throw new ArgumentNullException("comment");
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
        throw new ArgumentNullException("name");
      }
      else if(name.Length == 0)
      {
        throw new ArgumentException("Name must not be an empty string.", "name");
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
    /// Removes the current element from the DOM.
    /// </summary>
    public override void Remove()
    {
      this.GetParent().RemoveChild(this.Node);
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
        throw new InvalidOperationException("The current node must not be the root of the DOM.");
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
        throw new ArgumentNullException("node");
      }

      XmlNode actualNode;

      if(node.NodeType == XmlNodeType.Document)
      {
        actualNode = node.FirstChild;
      }
      else if(node.NodeType != XmlNodeType.Element)
      {
        throw new ArgumentException("Node must be an XML 'element' node.", "node");
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

