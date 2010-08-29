//  
//  TalNode.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2010 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Xml;
using CraigFowler.Web.ZPT.Tales;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tal.Exceptions;

namespace CraigFowler.Web.ZPT.Tal
{
  /// <summary>
  /// <para>Represents an <see cref="XmlElement"/> within a <see cref="TalDocument"/>.</para>
  /// </summary>
  public class TalElement : XmlElement, ITalElement
  {
    #region constants
    
    /// <summary>
    /// <para>
    /// Read-only.  Constant indicates the identifier for making a 'global' variable definition in the
    /// <see cref="TalesContext"/>.
    /// </para>
    /// </summary>
    public const string GlobalDefinitionIdentifier = "global";
    
    /// <summary>
    /// <para>
    /// Read-only.  Constant indicates the identifier for making a 'local' variable definition in the
    /// <see cref="TalesContext"/>.
    /// </para>
    /// </summary>
    public const string LocalDefinitionIdentifier = "local";
    
    /// <summary>
    /// <para>
    /// Read-only.  Constant indicates the identifier used for indicating that content should be written as
    /// <see cref="TalContentType.Text"/>.
    /// </para>
    /// </summary>
    public const string TextTypeIdentifier = "text";
    
    /// <summary>
    /// <para>
    /// Read-only.  Constant indicates the identifier used for indicating that content should be written as
    /// <see cref="TalContentType.Structure"/>.
    /// </para>
    /// </summary>
    public const string StructureTypeIdentifier = "structure";
    
    private const string
      DEFINE_STATEMENTS_PATTERN                   = @"([^;]|;;)+",
      DEFINE_SPECIFICATION_PATTERN                = @"^\s*((global|local) )?([a-zA-Z0-9]+) (.+)$",
      CONTENT_OR_REPLACE_SPECIFICATION_PATTERN    = @"^((text|structure) )?(.+)$",
      REPEAT_SPECIFICATION_PATTERN                = @"^([a-zA-Z0-9]+) (.+)$",
      ATTRIBUTES_SPECIFICATION_PATTERN            = @"^(([^: ]+):)?([^ ]+) (.+)$";
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a <see cref="Regex"/> that matches the individual statements of a TAL 'define' attribute value.
    /// </para>
    /// </summary>
    public static readonly Regex DefineStatements = new Regex(DEFINE_STATEMENTS_PATTERN,
                                                              RegexOptions.Compiled);
    
    /// <summary>
    /// <para>Read-only.  Gets a <see cref="Regex"/> that matches the parts of a TAL 'define' attribute value.</para>
    /// </summary>
    public static readonly Regex DefineSpecification = new Regex(DEFINE_SPECIFICATION_PATTERN,
                                                                 RegexOptions.Compiled);
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a <see cref="Regex"/> that matches the specification for a 'content', 'replace' or 'on-error'
    /// attribute value.
    /// </para>
    /// </summary>
    public static readonly Regex ContentOrReplaceSpecification = new Regex(CONTENT_OR_REPLACE_SPECIFICATION_PATTERN,
                                                                           RegexOptions.Compiled);
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a <see cref="Regex"/> that matches the specification for a 'repeat' attribute value.
    /// </para>
    /// </summary>
    public static readonly Regex RepeatSpecification = new Regex(REPEAT_SPECIFICATION_PATTERN,
                                                                 RegexOptions.Compiled);
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a <see cref="Regex"/> that matches the specification for an 'attributes' attribute value.
    /// </para>
    /// </summary>
    public static readonly Regex AttributesSpecification = new Regex(ATTRIBUTES_SPECIFICATION_PATTERN,
                                                                     RegexOptions.Compiled);
    
    #endregion
    
    #region fields
    
    private TalesContext context;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the <see cref="TalesContext"/> for this element node.</para>
    /// </summary>
    public TalesContext TalesContext
    {
      get {
        return context;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        context = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Overloaded.  Convenience method that renders the output of this document to a string.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>, the rendered output document.
    /// </returns>
    public string Render()
    {
      StringBuilder output = new StringBuilder();
      
      using(TextWriter writer = new StringWriter(output))
      {
        using (XmlWriter xmlWriter = new XmlTextWriter(writer))
        {
          this.Render(xmlWriter);
        }
      }
      
      return output.ToString();
    }
    
    /// <summary>
    /// <para>Overloaded.  Renders the output of this TAL element to the given <see cref="XmlWriter"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>The order of operation for TAL directives is listed below.</para>
    /// <para>
    /// Note that I am using the TAL 1.4.1 'proposed' ordering method.  That is, processing omit-tag before attributes.
    /// For more information about this, see the comment at the bottom of
    /// <c>http://wiki.zope.org/ZPT/TALSpecification14</c> and also the page
    /// <c>https://bugs.launchpad.net/zope.tal/+bug/430662</c>.
    /// </para>
    /// <list type="number">
    /// <item>define</item>
    /// <item>condition</item>
    /// <item>repeat</item>
    /// <item>content or replace</item>
    /// <item>omit-tag</item>
    /// <item>attributes</item>
    /// </list>
    /// </remarks>
    /// <param name="writer">
    /// An <see cref="XmlWriter"/> to write the rendered output of this element to.
    /// </param>
    public void Render(XmlWriter writer)
    {
      this.TalesContext.ParentContext = this.GetParentTalesContext();
      
      /* Developer's note
       * ----------------
       * This method only actually handles the 'define', 'condition', 'repeat' and 'on-error' attributes directly.
       * The ProcessElementContent() method handles 'content', 'replace', 'omit-tag' and 'attributes' attributes.
       */
      
      try
      {
        // Process any variable definitions specified by a TAL 'define' attribute.
        ProcessTalDefineAttribute();
        
        // If a TAL 'condition' attribute says that we should not render this element then skip over it.
        if(ProcessTalConditionAttribute())
        {
          RepeatVariable currentRepeatVariable;
          string repeatAlias;
          
          /* If this element contains a TAL 'repeat' attribute then the processing of the content is performed
           * many times.  If there is not then it is only performed once.
           */
          if(ProcessTalRepeatAttribute(out currentRepeatVariable, out repeatAlias))
          {
            while(currentRepeatVariable.MoveNext())
            {
              this.TalesContext.AddDefinition(repeatAlias, currentRepeatVariable.Current);
              ProcessElementContent(writer);
            }
          }
          else
          {
            ProcessElementContent(writer);
          }
        }
      }
      catch(Exception ex)
      {
        bool handled;
        object content = null;
        TalContentType type;
        
        /* Any exceptions raised in the above block could be handled by a TAL 'on-error' attribute.
         * If the attribute handles the error then write the appropriate content.
         * If not, then rethrow the exception to be caught by a lower-down TalElement.
         */
        content = ProcessTalOnErrorAttribute(ex, out type, out handled);
        
        if(handled)
        {
          WriteContentTo(content, writer, type, false);
        }
        else
        {
          throw;
        }
      }
    }
    
    /// <summary>
    /// <para>Gets the parent <see cref="TalesContext"/> from the <see cref="XmlElement.ParentNode"/>.</para>
    /// <para>If this is the root node in the document then the context from the doument node will be returned.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="TalesContext"/> instance.
    /// </returns>
    public TalesContext GetParentTalesContext()
    {
      XmlNode parentNode = this.ParentNode;
      
      while(parentNode != null && !(parentNode is ITalElement))
      {
        parentNode = (XmlElement) parentNode.ParentNode;
      }
      
      return (parentNode == null)? null : ((ITalElement) parentNode).TalesContext;
    }
    
    /// <summary>
    /// <para>Handles that TAL 'define' attribute.</para>
    /// </summary>
    private void ProcessTalDefineAttribute()
    {
      string attributeValue = GetTalAttribute("define");
      
      // If the attribute value is an empty string then just skip adding definitions
      if(attributeValue != String.Empty)
      {
        MatchCollection statementMatches = DefineStatements.Matches(attributeValue);
        
        foreach(Match statementMatch in statementMatches)
        {
          string
            statement = statementMatch.Value,
            variableName,
            expression;
          DefinitionType type = DefinitionType.Local;
          Match partsMatch = DefineSpecification.Match(statement);
          
          if(!partsMatch.Success)
          {
            TalParsingException ex = new TalParsingException("The TAL 'define' statement is not valid.");
            ex.ProblemString = statement;
            throw ex;
          }
          
          if(partsMatch.Groups[2].Success)
          {
            switch(partsMatch.Groups[2].Value)
            {
            case GlobalDefinitionIdentifier:
              type = DefinitionType.Global;
              break;
            case LocalDefinitionIdentifier:
              type = DefinitionType.Local;
              break;
            }
          }
          
          variableName = partsMatch.Groups[3].Value;
          expression = partsMatch.Groups[4].Value;
          
          this.TalesContext.AddDefinition(variableName,
                                          this.TalesContext.CreateExpression(expression).GetValue(),
                                          type);
        }
      }
    }
    
    /// <summary>
    /// <para>Handles the TAL 'condition' attribute.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Boolean"/> that indicates whether the 'condition' attribute is satisfied or not.
    /// </returns>
    private bool ProcessTalConditionAttribute()
    {
      bool output = true;
      string attributeValue = GetTalAttribute("condition");
      
      // If the attribute value is an empty string then just skip checking a condition
      if(attributeValue != String.Empty)
      {
        output = this.TalesContext.CreateExpression(attributeValue).GetBooleanValue();
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Handles the TAL 'on-error' attribute.</para>
    /// </summary>
    /// <param name="ex">
    /// An <see cref="Exception"/>, the exception that must be handled.
    /// </param>
    /// <param name="contentType">
    /// A <see cref="TalContentType"/> indicating how the return value should be treated.
    /// </param>
    /// <param name="handled">
    /// A <see cref="System.Boolean"/> indicating whether the error is handled or not.  If it is not handled then the
    /// exception will be rethrown.
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>, the content that is to be displayed.
    /// </returns>
    private object ProcessTalOnErrorAttribute(Exception ex, out TalContentType contentType, out bool handled)
    {
      string attributeValue = GetTalAttribute("on-error");
      object output = null;
      
      if(attributeValue != String.Empty)
      {
        Match statementMatch = ContentOrReplaceSpecification.Match(attributeValue);
        string expression;
        
        // If this comes out true then we're really in trouble, an error whilst processing the error-handler?  Oh dear.
        if(!statementMatch.Success)
        {
          string message = "Error encountered whilst processing a TAL 'on-error' attribute.";
          TalParsingException parsingException = new TalParsingException(message, ex);
          parsingException.ProblemString = attributeValue;
          throw parsingException;
        }
        
        /* As an addition to the TAL spec, we can make the current error available to the current TALES context
         * under the alias 'currentError'.
         */
        this.TalesContext.AddDefinition("currentError", ex);
        
        // Determine the content type
        if(statementMatch.Groups[2].Success)
        {
          switch(statementMatch.Groups[2].Value)
          {
          case TextTypeIdentifier:
            contentType = TalContentType.Text;
            break;
          case StructureTypeIdentifier:
            contentType = TalContentType.Structure;
            break;
          default:
            throw new NotSupportedException("Unsupported TAL content type");
          }
        }
        else
        {
          contentType = TalContentType.Text;
        }
        
        expression = statementMatch.Groups[3].Value;
        output = this.TalesContext.CreateExpression(expression).GetValue();
        handled = true;
      }
      else
      {
        // In this case, there's no error handler here.  Set handled to false and let the exception be re-thrown.
        contentType = TalContentType.Text;
        handled = false;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Handles the TAL 'repeat' attribute.</para>
    /// </summary>
    /// <param name="repeatVariable">
    /// A <see cref="RepeatVariable"/> that was created in this step.
    /// </param>
    /// <param name="alias">
    /// A <see cref="System.String"/>, the alias for the <paramref name="repeatVariable"/>.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/> indicating whether or not a repeat variable was found.
    /// </returns>
    private bool ProcessTalRepeatAttribute(out RepeatVariable repeatVariable, out string alias)
    {
      bool output;
      string attributeValue = GetTalAttribute("repeat");
      
      // If the attribute value is an empty string then just skip dealing with a repeated element
      if(attributeValue != String.Empty)
      {
        Match statement = RepeatSpecification.Match(attributeValue);
        string collectionExpression;
        
        if(!statement.Success)
        {
          TalParsingException ex = new TalParsingException("Error processing a TAL repeat attribute.");
          ex.ProblemString = attributeValue;
          throw ex;
        }
        
        alias = statement.Groups[1].Value;
        collectionExpression = statement.Groups[2].Value;
        
        repeatVariable = this.TalesContext.AddRepeatVariable(alias, collectionExpression);
        output = true;
      }
      else
      {
        output = false;
        repeatVariable = null;
        alias = null;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Handles TAL 'content' and 'replace' attributes.  Replace is given priority over content in the scenario in which
    /// both attributes exist on the same element.
    /// </para>
    /// </summary>
    /// <param name="content">
    /// A <see cref="System.Object"/> containing the content to be written to the XML output.
    /// </param>
    /// <param name="contentType">
    /// A <see cref="TalContentType"/> indicating how the <paramref name="content"/> should be treated.
    /// </param>
    /// <param name="replaceElement">
    /// A <see cref="System.Boolean"/> indicating whether or not the parent XML element should be replaced or preserved.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/> indicating whether or not there is custom content to write.
    /// </returns>
    private bool ProcessTalContentOrReplaceAttribute(out object content,
                                                     out TalContentType contentType,
                                                     out bool replaceElement)
    {
      bool output;
      string contentAttribute;
      
      // Just default values for these parameters
      content = null;
      contentType = TalContentType.Text;
      replaceElement = false;
      
      /* First try looking for a "replace" attribute, then if that fails a "content" attribute.
       * If we find either then we are writing content to the document.
       */
      contentAttribute = GetTalAttribute("replace");
      if(contentAttribute != String.Empty)
      {
        replaceElement = true;
        output = true;
      }
      else
      {
        contentAttribute = GetTalAttribute("content");
        output = (contentAttribute != String.Empty);
        replaceElement = false;
      }
      
      // If we are providing some content then we figure out what it is
      if(output)
      {
        Match contentStatement = ContentOrReplaceSpecification.Match(contentAttribute);
        
        if(!contentStatement.Success)
        {
          TalParsingException ex = new TalParsingException("Error whilst parsing a 'content' or 'replace' attribute.");
          ex.ProblemString = contentAttribute;
          throw ex;
        }
        
        if(contentStatement.Groups[2].Success)
        {
          switch(contentStatement.Groups[2].Value)
          {
          case TextTypeIdentifier:
            contentType = TalContentType.Text;
            break;
          case StructureTypeIdentifier:
            contentType = TalContentType.Structure;
            break;
          default:
            throw new NotSupportedException("Unsupported content type.");
          }
        }
        
        content = contentStatement.Groups[3].Value;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Handles the TAL 'omit-tag' attribute.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Boolean"/> indicating whether the element tag should be omitted or not.
    /// </returns>
    private bool ProcessTalOmitTagAttribute()
    {
      string omitTagAttribute;
      bool output = false;
      
      omitTagAttribute = GetTalAttribute("omit-tag");
      if(omitTagAttribute != String.Empty)
      {
        output = this.TalesContext.CreateExpression(omitTagAttribute).GetBooleanValue();
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Handles the TAL 'attributes' attribute.</para>
    /// </summary>
    private void ProcessTalAttributesAttribute()
    {
      string attributeValue = GetTalAttribute("attributes");
      
      if(attributeValue != String.Empty)
      {
        MatchCollection statementMatches = DefineStatements.Matches(attributeValue);
        
        foreach(Match statementMatch in statementMatches)
        {
          string
            statement = statementMatch.Value,
            prefix = null,
            localName,
            expression;
          Match attributesStatementMatch = AttributesSpecification.Match(statement);
          object expressionResult;
          XmlAttribute attribute;
          
          if(!attributesStatementMatch.Success)
          {
            TalParsingException ex = new TalParsingException("Error whilst parsing an 'attributes' attribute.");
            ex.ProblemString = statement;
            throw ex;
          }
          
          if(attributesStatementMatch.Groups[2].Success)
          {
            prefix = attributesStatementMatch.Groups[2].Value;
          }
          
          localName = attributesStatementMatch.Groups[3].Value;
          expression = attributesStatementMatch.Groups[4].Value;
          
          if(prefix != null)
          {
            attribute = this.Attributes[localName, this.GetNamespaceOfPrefix(prefix)];
          }
          else
          {
            attribute = this.Attributes[localName];
          }
          
          expressionResult = this.TalesContext.CreateExpression(expression).GetValue();
          
          /* How it works
           * ------------
           * * If the expressionResult is non-null then the attribute is created or set to the result
           * * If the attribute already exists and the expressionResult is null tnen the attribute is removed
           */
          if(expressionResult != null)
          {
            if(prefix != null)
            {
              this.SetAttribute(localName, this.GetNamespaceOfPrefix(prefix), expressionResult.ToString());
            }
            else
            {
              this.SetAttribute(localName, expressionResult.ToString());
            }
          }
          else if(attribute != null && expressionResult == null)
          {
            if(prefix != null)
            {
              this.RemoveAttribute(localName, this.GetNamespaceOfPrefix(prefix));
            }
            else
            {
              this.RemoveAttribute(localName);
            }
          }
        }
      }
    }
    
    /// <summary>
    /// <para>Processes the content of a TAL element.</para>
    /// </summary>
    /// <param name="writer">
    /// An <see cref="XmlWriter"/> to write the element content to.
    /// </param>
    /// <remarks>
    /// <para>Collectively these are the 'content', 'replace', 'omit-tag' and 'attributes' attributes.</para>
    /// </remarks>
    private void ProcessElementContent(XmlWriter writer)
    {
      object contentToWrite;
      bool replaceElement, writeContent;
      TalContentType contentType;
      
      // Determine whether we are writing any custom content or not
      writeContent = ProcessTalContentOrReplaceAttribute(out contentToWrite, out contentType, out replaceElement);
      
      // Handle the 'omit-tag' attribute
      if(!replaceElement)
      {
        replaceElement = ProcessTalOmitTagAttribute();
      }
      
      if(!replaceElement)
      {
        ProcessTalAttributesAttribute();
      }
      
      if(writeContent)
      {
        WriteContentTo(contentToWrite, writer, contentType, replaceElement);
      }
      else if(!replaceElement)
      {
        writer.WriteStartElement(this.Prefix, this.LocalName, this.NamespaceURI);
        
        foreach(XmlAttribute attribute in this.Attributes)
        {
          if(attribute.NamespaceURI != TalDocument.TalNamespace &&
             attribute.NamespaceURI != TalDocument.MetalNamespace)
          {
            writer.WriteAttributeString(attribute.Prefix,
                                        attribute.LocalName,
                                        attribute.NamespaceURI,
                                        attribute.Value);
          }
        }
        
        foreach(XmlNode node in this.ChildNodes)
        {
          if(node is ITalElement)
          {
            ((ITalElement) node).Render(writer);
          }
          else
          {
            node.WriteTo(writer);
          }
        }
        
        writer.WriteEndElement();
      }
    }
    
    /// <summary>
    /// <para>Overloaded.  Writes the content of this TAL node to an <see cref="XmlWriter"/>.</para>
    /// </summary>
    /// <param name="content">
    /// A <see cref="System.String"/>, the content to write.
    /// </param>
    /// <param name="writer">
    /// A <see cref="XmlWriter"/>, the writer instance to write to.
    /// </param>
    /// <param name="contentType">
    /// A <see cref="TalContentType"/> indicating whether the content should be treated as text or XML structure.  If
    /// <see cref="TalContentType.Text"/> is indicated then greater-than, less-than and ampersand symbols are escaped
    /// to their XML entity equivalents.  Otherwise the <paramref name="content"/> is left as-is.
    /// </param>
    /// <param name="replaceElement">
    /// A <see cref="System.Boolean"/> indicating whether the entire element should be replaced or not.
    /// </param>
    private void WriteContentTo(object content, XmlWriter writer, TalContentType contentType, bool replaceElement)
    {
      if(!replaceElement)
      {
        // Open the new element
        writer.WriteStartElement(this.Prefix, this.LocalName, this.NamespaceURI);
        
        // Write all of its attributes
        foreach(XmlAttribute attribute in this.Attributes)
        {
          if(attribute.NamespaceURI != TalDocument.TalNamespace &&
             attribute.NamespaceURI != TalDocument.MetalNamespace)
          {
            writer.WriteAttributeString(attribute.Prefix,
                                        attribute.LocalName,
                                        attribute.NamespaceURI,
                                        attribute.Value);
          }
        }
        
        // Write the text inside the element
        if(contentType == TalContentType.Structure)
        {
          writer.WriteRaw(content.ToString());
        }
        else
        {
          writer.WriteString(content.ToString());
        }
        
        // Close the element
        writer.WriteEndElement();
      }
      else
      {
        // We're just writing the text inside the element
        if(contentType == TalContentType.Structure)
        {
          writer.WriteRaw(content.ToString());
        }
        else
        {
          writer.WriteString(content.ToString());
        }
      }
    }
    
    /// <summary>
    /// <para>
    /// Gets the contents of the requested TAL attribute residing on this element node.
    /// Returns <see cref="System.String.Empty"/> if the attribute is not defined or if it contains an empty value.
    /// </para>
    /// </summary>
    /// <param name="attributeName">
    /// A <see cref="System.String"/>, the name of the attribute requested.
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>, the content of the attribute or an empty string if the given attribute is not
    /// defined on this element (or contains an empty value).
    /// </returns>
    private string GetTalAttribute(string attributeName)
    {
      if(attributeName == null)
      {
        throw new ArgumentNullException("attributeName");
      }
      else if(attributeName == String.Empty)
      {
        throw new ArgumentOutOfRangeException("attributeName", "Attribute name must not be an empty string.");
      }
      
      return this.GetAttribute(attributeName, TalDocument.TalNamespace);
    }
    
    #endregion

    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with the given information.</para>
    /// </summary>
    /// <param name="prefix">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="localName">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="namespaceURI">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="document">
    /// A <see cref="TalDocument"/>
    /// </param>
    public TalElement(string prefix,
                      string localName,
                      string namespaceURI,
                      TalDocument document) : base(prefix, localName, namespaceURI, document)
    {
      this.TalesContext = new TalesContext();
    }
    
    #endregion
  }
}