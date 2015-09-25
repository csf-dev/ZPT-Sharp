using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="Element"/> based on documents parsed using the HTML Agility Pack.
  /// </summary>
  public class HtmlElement : Element
  {
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
      var parent = this.Node.ParentNode;

      if(repl == null)
      {
        throw new ArgumentException("The replacement must be a non-null instance of HtmlElement.",
                                    "replacement");
      }
      else if(parent == null)
      {
        throw new InvalidOperationException("The current node must not be the root of the DOM.");
      }

      return new HtmlElement(parent.ReplaceChild(repl.Node, this.Node));
    }

    /// <summary>
    /// Gets a collection of the child elements from the current source element.
    /// </summary>
    /// <returns>The children.</returns>
    public override Element[] GetChildElements()
    {
      return this.Node.ChildNodes
        .Where(x => x.NodeType == HtmlNodeType.Element)
        .Select(x => new HtmlElement(x))
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
              select new HtmlElement(node))
        .ToArray();
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

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.HtmlElement"/> class.
    /// </summary>
    /// <param name="node">The HTML node.</param>
    public HtmlElement(HtmlNode node)
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

