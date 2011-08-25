//  
//  OutputFormatting.cs
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

namespace CraigFowler.Web.ZPT.Tal
{
	/// <summary>
	/// <para>
	/// Describes information relavant to the formatting of rendered TAL output.  Internally this wraps an
	/// <see cref="XmlWriter"/>.
	/// </para>
	/// </summary>
	public class TalOutput
	{
		#region fields
		
		private XmlWriter innerWriter;
		
		#endregion
		
		#region properties

		/// <summary>
		/// <para>Read-only.  Gets the current level of indentation.</para>
		/// </summary>
		public int CurrentIndentationLevel
		{
			get;
			private set;
		}
		
		/// <summary>
		/// <para>Gets and sets whether reformatting is being performed or not.</para>
		/// </summary>
		public bool PerformReformatting
		{
			get;
			set;
		}
		
		/// <summary>
		/// <para>Read-only.  Gets the underlying <see cref="XmlWriter"/> instance that this instance contains.</para>
		/// </summary>
		public XmlWriter Writer
		{
			get {
				return innerWriter;
			}
			private set {
				if(value == null)
				{
					throw new ArgumentNullException("value");
				}
				
				innerWriter = value;
			}
		}
		
		/// <summary>
		/// <para>Gets and sets whether or not node names and attributes are normalised to lowercase or not.</para>
		/// </summary>
		public bool NormaliseToLowercase
		{
			get;
			set;
		}

		#endregion
		
		#region writer methods
		
		/// <summary>
		/// <para>Conditionally writes a newline to the underlying <see cref="Writer"/>.</para>
		/// <seealso cref="PerformReformatting"/>
		/// </summary>
		public void WriteNewLine()
		{
			if(this.PerformReformatting)
			{
        this.Writer.WriteWhitespace(this.Writer.Settings.NewLineChars);
			}
		}
		
		/// <summary>
		/// <para>Conditionally writes indentation characters to the underlying <see cref="Writer"/>.</para>
		/// <seealso cref="PerformReformatting"/>
		/// <seealso cref="CurrentIndentationLevel"/>
		/// </summary>
		public void WriteIndent()
		{
			if(this.PerformReformatting && this.Writer.Settings.Indent)
      {
        for(int i = 0; i < this.CurrentIndentationLevel; i++)
        {
          this.Writer.WriteWhitespace(this.Writer.Settings.IndentChars);
        }
      }
		}
		
		/// <summary>
		/// <para>Writes the given content to the <see cref="Writer"/> output.</para>
		/// </summary>
		/// <param name="content">
		/// A <see cref="TalContent"/>
		/// </param>
		public void WriteContent(TalContent content)
		{
			if(content == null)
			{
				throw new ArgumentNullException("content");
			}
			
			// Deal with the start element if it is being written
			if(content.WriteElement)
			{
				WriteStartElement(content);
				IncreaseIndentation();
			}
			
			// Deal with the content inside the element
			if(content.WriteContent)
			{
				WriteText(content);
			}
			else
			{
				WriteChildNodes(content.CurrentElement);
			}
			
			// Deal with the end element if it is being written
			if(content.WriteElement)
			{
				WithdrawIndentation();
				WriteEndElement(content);
			}
			
		}
		
		/// <summary>
		/// <para>Writes an XML start-node to the content stream along with all of its attributes.</para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method might also write leading indentation and a trailing newline if the current configuration of this
		/// instance deems that it is appropriate to do so.
		/// </para>
		/// </remarks>
		/// <param name="content">
		/// A <see cref="TalContent"/>
		/// </param>
		public void WriteStartElement(TalContent content)
		{
			XmlNode node;
			
			if(content == null)
			{
				throw new ArgumentNullException("content");
			}
			else if(content.CurrentElement == null)
			{
				throw new ArgumentException("The content does not contain a TAL element node.", "content");
			}
			
			node = content.CurrentElement;
			
			// Indent before the start-node
			WriteIndent();
			
			// Write the start element itself
			this.Writer.WriteStartElement(node.Prefix,
			                              (this.NormaliseToLowercase? node.LocalName.ToLower() : node.LocalName),
			                              node.NamespaceURI);
			
      // Write all of its attributes
      foreach(XmlAttribute attrib in node.Attributes)
      {
        if(OKToRenderAttribute(attrib))
        {
          this.Writer.WriteAttributeString(attrib.Prefix,
                                      		 (this.NormaliseToLowercase? attrib.LocalName.ToLower() : attrib.LocalName),
                                      		 attrib.NamespaceURI,
                                      		 attrib.Value);
        }
      }
			
			// If the element is not an empty node then write a trailing newline.
			if(!content.IsEmptyNode)
			{
				WriteNewLine();
			}
		}
		
		/// <summary>
		/// <para>
		/// Writes the end of an XML element and a trailing newline character if the configuration of this instance deems
		/// that this is appropriate.
		/// </para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method is responsible for writing the end element itself as well as inserting a new
		/// line character.  It is not responsible for any indentation.  The indentation must be performed before and after
		/// this method is used (as appropriate).
		/// </para>
		/// </remarks>
		/// <param name="content">
		/// A <see cref="TalContent"/> that describes the node to be written.
		/// </param>
		public void WriteEndElement(TalContent content)
		{
			if(content == null)
			{
				throw new ArgumentNullException("content");
			}
			else if(content.CurrentElement == null)
			{
				throw new ArgumentException("The content does not contain a TAL element node.", "content");
			}
			
			// If the element is not an empty node then write the leading indentation
			if(!content.IsEmptyNode)
			{
				WriteIndent();
			}
			
			// Write the end element itself
			this.Writer.WriteEndElement();
			
			// A trailing newline
			WriteNewLine();
		}
		
		/// <summary>
		/// <para>
		/// Writes text content to the output.  This method might also write leading indentation and a trailing newline
		/// if the configuration of this instance deems that these are appropriate.
		/// </para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// When writing content, if <see cref="PerformReformatting"/> is enabled then the content will be trimmed of
		/// leading and trailing whitespace before being written to the output stream.
		/// </para>
		/// </remarks>
		/// <param name="content">
		/// A <see cref="TalContent"/>
		/// </param>
		public void WriteText(TalContent content)
		{
			string contentToWrite;
			
			if(content == null)
			{
				throw new ArgumentNullException("content");
			}
			else if(!content.WriteContent)
			{
				throw new ArgumentException("Content indicates that there is nothing to write");
			}
			
			// Format the content to be written appropriately
			contentToWrite = String.IsNullOrEmpty(content.Content)? String.Empty : content.Content;
			contentToWrite = this.PerformReformatting? contentToWrite.Trim() : contentToWrite;
			
			// A leading indent
			WriteIndent();
			
			// Write the actual text content.
			switch(content.Type)
			{
			case TalContentType.Structure:
				this.Writer.WriteRaw(contentToWrite);
				break;
			case TalContentType.Text:
				this.Writer.WriteString(contentToWrite);
				break;
			default:
				throw new NotSupportedException("Unsupported content type.");
			}
			
			// A trailing newline
			WriteNewLine();
		}
		
		/// <summary>
		/// <para>Writes the child nodes for the given <see cref="TalElement"/>.</para>
		/// </summary>
		/// <param name="node">
		/// A <see cref="TalElement"/>
		/// </param>
		public void WriteChildNodes(TalElement node)
		{
			if(node == null)
			{
				throw new ArgumentNullException("node");
			}

			for(int i = 0; i < node.ChildNodes.Count; i++)
			{
				XmlNode child = node.ChildNodes[i];
				
				if(child is TalElement)
				{
					((TalElement) child).Render(this);
				}
				else if(child is XmlText)
				{
					TalContent innerContent = new TalContent(((XmlText) child).Value);
					WriteText(innerContent);
				}
				else
				{
					WriteIndent();
					child.WriteTo(this.Writer);
					WriteNewLine();
				}
			}
		}
		
		#endregion
		
		#region helper methods
		
		/// <summary>
		/// <para>Increments the <see cref="CurrentIndentationLevel"/> by one.</para>
		/// </summary>
		public void IncreaseIndentation()
		{
			ChangeIndentation(1);
		}
		
		/// <summary>
		/// <para>Decrements the <see cref="CurrentIndentationLevel"/> by one.</para>
		/// </summary>
		public void WithdrawIndentation()
		{
			ChangeIndentation(-1);
		}
		
		/// <summary>
		/// <para>Alters the <see cref="CurrentIndentationLevel"/> by the given <paramref name="change"/>.</para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// Generally this method should not be used on its own.  Instead <see cref="IncreaseIndentation"/> and
		/// <see cref="WithdrawIndentation"/> should be used to increment and decrement the indentation by a single level.
		/// </para>
		/// </remarks>
		/// <param name="change">
		/// A <see cref="System.Int32"/>
		/// </param>
		public void ChangeIndentation(int change)
		{
			if(this.CurrentIndentationLevel + change < 0)
			{
				throw new ArgumentOutOfRangeException("change",
				                                      "The alteration to the indent would bring the indent level " +
				                                      "to less than zero.");
			}
			
			this.CurrentIndentationLevel += change;
		}
		
		/// <summary>
		/// <para>Determines whether the given <paramref name="attribute"/> should be rendered in the final output.</para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// TAL and METAL attributes should never be rendered, nor should 'xmlns' declarations for these namespaces.
		/// </para>
		/// </remarks>
		/// <param name="attribute">
		/// A <see cref="XmlAttribute"/>, the candidate attribute
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> indicating whether the <paramref name="attribute"/> should be rendered or not.
		/// </returns>
		private bool OKToRenderAttribute(XmlAttribute attribute)
		{
			bool output;
			
			if(attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			
			if(attribute.NamespaceURI == TalDocument.TalNamespace ||
			   attribute.NamespaceURI == TalDocument.MetalNamespace)
			{
				// It's an attribute in one of the namespaces that we are choosing not to render
				output = false;
			}
			else if(attribute.NamespaceURI == TalDocument.XmlnsNamespace &&
			        (attribute.Value == TalDocument.TalNamespace || attribute.Value == TalDocument.MetalNamespace))
			{
				// It's a namespace declaration for one of the namespaces that we are choosing not to render
				output = false;
			}
			else
			{
				// It's some other kind of attribute that should be rendered
				output = true;
			}
			
			return output;
		}
		
		#endregion
		
		#region constructors
		
		/// <summary>
		/// <para>Intiialises this instance with an <see cref="XmlWriter"/>.</para>
		/// </summary>
		/// <param name="writer">
		/// A <see cref="XmlWriter"/>
		/// </param>
		public TalOutput(XmlWriter writer)
		{
			this.CurrentIndentationLevel = 0;
			this.PerformReformatting = false;
			this.Writer = writer;
		}
		
		#endregion
	}
}

