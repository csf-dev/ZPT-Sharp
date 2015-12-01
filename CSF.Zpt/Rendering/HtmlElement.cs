using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="Element"/> based on documents parsed using the HTML Agility Pack.
  /// </summary>
  public class HtmlElement : Element
  {
    #region constants

    private const string
      INDENT_PATTERN        = @"([ \t]+)$",
      HTML_COMMENT_START    = "<!-- ",
      HTML_COMMENT_END      = " -->\n";
    private static readonly Regex Indent = new Regex(INDENT_PATTERN, RegexOptions.Compiled);

    #endregion

    #region fields

    private HtmlNode _node;

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

    #endregion

    #region methods

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current
    /// <see cref="CSF.Zpt.Rendering.HtmlElement"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Rendering.HtmlElement"/>.</returns>
    public override string ToString()
    {
      return this.Node.OuterHtml;
    }

    /// <summary>
    /// Replaces the current element in its respective DOM with the given replacement.
    /// </summary>
    /// <returns>A reference to the replacement element, in its new DOM.</returns>
    /// <param name="replacement">Replacement.</param>
    public override Element ReplaceWith(Element replacement)
    {
      var repl = replacement as HtmlElement;
      if(repl == null)
      {
        throw new ArgumentException("The replacement must be a non-null instance of HtmlElement.",
                                    "replacement");
      }

      var parent = this.GetParent();
      return new HtmlElement(parent.ReplaceChild(repl.Node, this.Node),
                             repl.SourceFile,
                             isImported: true);
    }

    /// <summary>
    /// Gets the element which is the parent of the current instance.
    /// </summary>
    /// <returns>The parent element.</returns>
    public override Element GetParentElement()
    {
      return (this.Node.ParentNode != null)? new HtmlElement(this.Node.ParentNode, this.SourceFile) : null;
    }

    /// <summary>
    /// Gets a collection of the child elements from the current source element.
    /// </summary>
    /// <returns>The children.</returns>
    public override Element[] GetChildElements()
    {
      return this.Node.ChildNodes
        .Where(x => x.NodeType == HtmlNodeType.Element)
        .Select(x => new HtmlElement(x, this.SourceFile))
        .ToArray();
    }

    /// <summary>
    /// Gets a collection of the attributes present upon the current element.
    /// </summary>
    /// <returns>The attributes.</returns>
    public override Attribute[] GetAttributes()
    {
      return this.Node.Attributes
        .Select(x => new HtmlAttribute(x))
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
    public override Attribute GetAttribute(string attributeNamespace, string prefix, string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException("name");
      }

      var attribName = this.GetName(prefix, name);

      var htmlAttribute = this.Node.Attributes
        .FirstOrDefault(x => x.Name == attribName);

      return (htmlAttribute != null)? new HtmlAttribute(htmlAttribute) : null;
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have an
    /// attribute matching the given criteria.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="attributeNamespace">The attribute namespace.</param>
    /// <param name="prefix">The attribute prefix.</param>
    /// <param name="name">The attribute name.</param>
    public override Element[] SearchChildrenByAttribute(string attributeNamespace, string prefix, string name)
    {
      string attribName = this.GetName(prefix, name);

      return (from node in this.Node.Descendants()
              from attrib in node.Attributes
              where attrib.Name == attribName
              select new HtmlElement(node, this.SourceFile))
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
      if(prefix == null)
      {
        throw new ArgumentNullException("prefix");
      }

      var nodes = this.Node
        .Descendants()
        .Union(new [] { this.Node })
        .ToArray();

      var toRemove = (from node in nodes
                      from attrib in node.Attributes
                      where attrib.Name.StartsWith(String.Concat(prefix, ":"))
                      select new { Element = node, Attribute = attrib });

      foreach(var item in toRemove)
      {
        item.Element.Attributes.Remove(item.Attribute);
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
         && previousNode.NodeType == HtmlNodeType.Text)
      {
        HtmlTextNode previousText = (HtmlTextNode) previousNode;
        var indentMatch = Indent.Match(previousText.Text);
        if(indentMatch.Success)
        {
          indent = indentMatch.Value;
        }
      }

      string commentText = String.Concat(HTML_COMMENT_START, comment, HTML_COMMENT_END, indent);
      var commentNode = this.Node.OwnerDocument.CreateComment(commentText);

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
    public override Element Clone()
    {
      var clone = _node.Clone();

      return new HtmlElement(clone, this.SourceFile);
    }

    /// <summary>
    /// Gets the file location (typically a line number) for the current instance.
    /// </summary>
    /// <returns>The file location.</returns>
    public override string GetFileLocation()
    {
      return String.Format("Line {0}", _node.Line);
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
    private HtmlNode GetParent()
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
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.HtmlElement"/> class.
    /// </summary>
    /// <param name="node">The source HTML node.</param>
    /// <param name="sourceFile">Information about the element's source file.</param>
    /// <param name="isRoot">Whether or not this is the root element.</param>
    /// <param name="isImported">Whether or not this element is imported.</param>
    public HtmlElement(HtmlNode node,
                       SourceFileInfo sourceFile,
                       bool isRoot = false,
                       bool isImported = false) : base(sourceFile, isRoot, isImported)
    {
      if(node == null)
      {
        throw new ArgumentNullException("node");
      }

      HtmlNode actualNode;

      if(node.NodeType == HtmlNodeType.Document)
      {
        actualNode = node.FirstChild;
      }
      else if(node.NodeType != HtmlNodeType.Element)
      {
        throw new ArgumentException("Node must be an HTML 'element' node.", "node");
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

