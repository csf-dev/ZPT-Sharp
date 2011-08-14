//  
//  ZptMetadata.cs
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
using System.IO;
using System.Xml.Serialization;
using CraigFowler.Web.ZPT.Metal;
using CraigFowler.Web.ZPT.Tal;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace CraigFowler.Web.ZPT
{
	/// <summary>
	/// <para>Container for metadata information about a <see cref="ZptDocument"/>.</para>
	/// </summary>
	[XmlRoot("zptMetadata")]
	public class ZptMetadata
	{
		#region constants
		
		/// <summary>
		/// <para>Read-only.  Constant gets the filename extension for document template files.</para>
		/// </summary>
		public const string ZptTemplateDocumentExtension = ".pt";
		
		/// <summary>
		/// <para>Read-only.  Constant gets the XML namespace for ZPT metadata.</para>
		/// </summary>
		public const string ZptMetadataNamespace = "http://xml.craigfowler.me.uk/namespaces/ZptMetadata";
		
		private const string METADATA_EXTENSION_SUFFIX = ".metadata";
		
		/// <summary>
		/// <para>Read-only.  Gets the filename extension for document template metadata files.</para>
		/// </summary>
		public static readonly string ZptTemplateMetadataExtension = String.Format("{0}{1}",
		                                                                           ZptTemplateDocumentExtension,
		                                                                           METADATA_EXTENSION_SUFFIX);
		/// <summary>
    /// <para>
    /// Read-only runtime constant.  Gets a <see cref="System.Type"/> reference to the interface that all ZPT document
    /// classes must implement.
    /// </para>
    /// </summary>
    public static readonly Type RequiredDocumentInterface = typeof(IZptDocument);
    
		#endregion
		
		#region fields
		
		private Type documentClass;
		
		#endregion
		
		#region properties
		
		/// <summary>
		/// <para>Gets and sets the type of document that this instance describes.</para>
		/// </summary>
		[XmlElementAttribute("documentType")]
		public ZptDocumentType DocumentType
		{
			get;
			set;
		}
		
		/// <summary>
		/// <para>
		/// Gets and sets a <see cref="System.Type"/> that is used to provide logic for this document.  By default this
		/// is set to <see cref="ZptDocument"/> and any new value must be a type that implements ZptDocument.
		/// </para>
		/// </summary>
		[XmlIgnoreAttribute]
		public Type DocumentClass
		{
			get {
				return documentClass;
			}
			set {
				if(value == null)
				{
					throw new ArgumentNullException("value");
				}
				
				bool implementsIZptDocument = ImplementsRequiredInterface(value);
				
				// If we found a type that implements IZptDocument then we use it, otherwise throw an exception
				if(implementsIZptDocument)
				{
					documentClass = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException("value",
					                                      "The type passed to this property does not implement IZptDocument.");
				}
			}
		}
		
		/// <summary>
		/// <para>
		/// Gets and sets the <see cref="DocumentClass"/> property by a <see cref="System.String"/> name, suitable for
		/// the input received from a metadata file.
		/// </para>
		/// </summary>
		[XmlElementAttribute("className")]
		public string DocumentClassName
		{
			get {
				return this.DocumentClass.FullName;
			}
			set {
				Type discoveredType;
				
				discoveredType = Type.GetType(value, false);
				
				if(discoveredType == null)
				{
					if(ForeignDocumentClasses.ContainsKey(value))
					{
						discoveredType = ForeignDocumentClasses[value];
					}
					else
					{
						throw new ArgumentOutOfRangeException("Could not load a type of the given name and it was not found " +
																									"amongst the collection of registered 'foreign' document classes.");
					}
				}
				
				this.DocumentClass = discoveredType;
			}
		}
		
		/// <summary>
		/// <para>Gets and sets the filesystem path to the template file.</para>
		/// </summary>
		[XmlIgnore]
		public string DocumentFilePath
		{
			get;
			set;
		}
		
		/// <summary>
		/// <para>
		/// Read-only.  Gets a collection of the 'foreign' document classes that can provide <see cref="IZptDocument"/>.
		/// </para>
		/// </summary>
		protected static Dictionary<string,Type> ForeignDocumentClasses
		{
			get;
			private set;
		}
		
		#endregion
		
		#region methods
		
		/// <summary>
		/// <para>
		/// Gets an <see cref="ITemplateDocument"/> instance that represents the XML page template file described by
		/// this metadata.
		/// </para>
		/// </summary>
		/// <returns>
		/// An object instance that implements <see cref="ITemplateDocument"/>.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// <see cref="DocumentFilePath"/> is null or an empty string.
		/// </exception>
		/// <exception cref="FileNotFoundException">
		/// The XML template file described by <see cref="DocumentFilePath"/> could not be found.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// <see cref="DocumentFilePath"/> does not describe a supported document type.
		/// </exception>
		public ITemplateDocument LoadDocument()
		{
			FileInfo fileInfo;
			ITemplateDocument output;
			
			if(String.IsNullOrEmpty(this.DocumentFilePath))
			{
				throw new InvalidOperationException("This instance does not describe a path to a ZPT document " +
																						"template file.");
			}
			
			fileInfo = new FileInfo(this.DocumentFilePath);
			
			if(!fileInfo.Exists)
			{
				throw new FileNotFoundException("The ZPT document template file could not be found.", fileInfo.FullName);
			}
			
			switch(this.DocumentType)
			{
			case ZptDocumentType.Metal:
				output = new MetalDocument();
				break;
			case ZptDocumentType.Tal:
				output = new TalDocument();
				break;
			default:
				throw new NotSupportedException("The selected document type is not supported.");
			}
			
			output.Load(fileInfo.FullName);
			
			return output;
		}
		
		/// <summary>
		/// <para>Overloaded.  Writes this instance to XML and returns the generated XML text.</para>
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string WriteToXml()
		{
			StringBuilder output = new StringBuilder();
			
			using(TextWriter writer = new StringWriter(output))
			{
				WriteToXml(writer);
			}
			
			return output.ToString();
		}
		
		/// <summary>
		/// <para>
		/// Overloaded.  Writes this instance to XML and stores it in a given <paramref name="outputFile"/>.
		/// </para>
		/// </summary>
		/// <param name="outputFile">
		/// A <see cref="FileInfo"/>
		/// </param>
		public void WriteToXml(FileInfo outputFile)
		{
			if(outputFile == null)
			{
				throw new ArgumentNullException("outputFile");
			}
			
			using(TextWriter writer = new StreamWriter(outputFile.FullName))
			{
				WriteToXml(writer);
			}
		}
		
		/// <summary>
		/// <para>Overloaded.  Writes this instance to XML using the given <paramref name="writer"/>.</para>
		/// </summary>
		/// <param name="writer">
		/// A <see cref="TextWriter"/>
		/// </param>
		private void WriteToXml(TextWriter writer)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			
			using(XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
			{
				XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
				
				namespaces.Add(String.Empty, String.Empty);
				
				XmlSerializer serialiser = new XmlSerializer(typeof(ZptMetadata), ZptMetadataNamespace);
				serialiser.Serialize(xmlWriter, this, namespaces);
			}
		}
		
		#endregion
		
		#region constructors
		
		/// <summary>
		/// <para>Inititlaises this instance with default values.</para>
		/// </summary>
		public ZptMetadata()
		{
			this.DocumentType = ZptDocumentType.Metal;
			this.DocumentClass = typeof(ZptDocument);
			this.DocumentFilePath = null;
		}
		
		/// <summary>
		/// <para>Static constructor initialises the list of foriegn document classes.</para>
		/// </summary>
		static ZptMetadata()
		{
			ForeignDocumentClasses = new Dictionary<string, Type>();
		}
		
		#endregion
		
		#region static methods
		
		/// <summary>
		/// <para>Overloaded.  Gets an instance of <see cref="ZptMetadata"/> for a ZPT template document.</para>
		/// </summary>
    /// <remarks>
    /// <para>
    /// In any non-exception scenario this method will always return a non-null <see cref="ZptMetadata"/> instance.
    /// If the template file has no explicit metadata defined then a default metadata instance will be created for that
    /// template.
    /// </para>
    /// </remarks>
		/// <param name="documentFilePath">
		/// A <see cref="System.String"/> - the file path to the ZPT document file.  The metadata file will be
		/// automatically detected based on its extension.
		/// <seealso cref="ZptTemplateMetadataExtension"/>
		/// </param>
		/// <returns>
		/// A <see cref="ZptMetadata"/> instance for the ZPT template file.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="documentFilePath"/> is null.
		/// </exception>
		/// <exception cref="FileNotFoundException">
		/// No template file can be found at <paramref name="documentFilePath"/>.
		/// </exception>
		public static ZptMetadata GetMetadata(string documentFilePath)
		{
      FileInfo documentFile;
      
      if(documentFilePath == null)
      {
        throw new ArgumentNullException("documentFilePath");
      }
      
      documentFile = new FileInfo(documentFilePath);
      
      return GetMetadata(documentFile);
		}
    
    /// <summary>
    /// <para>Overloaded.  Gets an instance of <see cref="ZptMetadata"/> for a ZPT template document.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// In any non-exception scenario this method will always return a non-null <see cref="ZptMetadata"/> instance.
    /// If the template file has no explicit metadata defined then a default metadata instance will be created for that
    /// template.
    /// </para>
    /// </remarks>
    /// <param name="documentFile">
    /// A <see cref="FileInfo"/> that describes the location of the ZPT document file.  The metadata file will be
    /// automatically detected based on its extension.
    /// <seealso cref="ZptTemplateMetadataExtension"/>
    /// </param>
    /// <returns>
    /// A <see cref="ZptMetadata"/> instance for the ZPT template file.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="documentFile"/> is null.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// No template file can be found at <paramref name="documentFile"/>.
    /// </exception>
    public static ZptMetadata GetMetadata(FileInfo documentFile)
    {
      FileInfo metadataFile;
      ZptMetadata output;
      
      if(documentFile == null)
      {
        throw new ArgumentNullException("documentFile");
      }
      else if(!documentFile.Exists)
      {
        throw new FileNotFoundException("The ZPT template document file was not found", documentFile.FullName);
      }
      
      metadataFile = GetMetadataFileInfo(documentFile.FullName);
      
      // It's OK if the metadata file does not exist.  If it doesn't then just use the default metadata settings.
      if(!metadataFile.Exists)
      {
        output = new ZptMetadata();
      }
      else
      {
        output = CreateMetadataFromXml(metadataFile);
      }
      
      output.DocumentFilePath = documentFile.FullName;
      
      return output;
    }
		
		/// <summary>
		/// <para>Gets information about the document metadata file, based on the path of the template file itself.</para>
		/// </summary>
		/// <param name="documentFilePath">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="FileInfo"/>
		/// </returns>
		public static FileInfo GetMetadataFileInfo(string documentFilePath)
		{
			if(documentFilePath == null)
			{
				throw new ArgumentNullException("documentFilePath");
			}
			
			return new FileInfo(String.Format("{0}{1}", documentFilePath, METADATA_EXTENSION_SUFFIX));
		}
		
		/// <summary>
		/// <para>Overloaded.  Creates a new metadata instance from an XML file.</para>
		/// </summary>
		/// <param name="metadataFile">
		/// A <see cref="FileInfo"/>, that describes the XML metadata file.
		/// </param>
		/// <returns>
		/// A <see cref="ZptMetadata"/>
		/// </returns>
		public static ZptMetadata CreateMetadataFromXml(FileInfo metadataFile)
		{
			ZptMetadata output;
			
			using(FileStream stream = new FileStream(metadataFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (TextReader reader = new StreamReader(stream))
				{
					output = CreateMetadataFromXml(reader);
				}
			}
			
			return output;
		}
		
		/// <summary>
		/// <para>Overloaded.  Creates a new metadata instance from a <see cref="TextReader"/>.</para>
		/// </summary>
		/// <param name="reader">
		/// A <see cref="TextReader"/>
		/// </param>
		/// <returns>
		/// A <see cref="ZptMetadata"/>
		/// </returns>
		public static ZptMetadata CreateMetadataFromXml(TextReader reader)
		{
			return (ZptMetadata) new XmlSerializer(typeof(ZptMetadata), ZptMetadataNamespace).Deserialize(reader);
		}
		
		/// <summary>
		/// <para>
		/// Registers a new <see cref="Type"/> (which must implement <see cref="IZptDocument"/>) into the static collection
		/// of known 'foreign' document classes.
		/// </para>
		/// </summary>
		/// <param name="typeToRegister">
		/// A <see cref="Type"/>
		/// </param>
		public static void RegisterDocumentClass(Type typeToRegister)
		{
			if(typeToRegister == null)
			{
				throw new ArgumentNullException("typeToRegister");
			}
			else if(!ImplementsRequiredInterface(typeToRegister))
			{
				throw new ArgumentOutOfRangeException("typeToRegister", "The type does not implement IZptDocument.");
			}
			
      ForeignDocumentClasses.Add(typeToRegister.FullName, typeToRegister);
		}
    
    /// <summary>
    /// <para>
    /// Registers all of the compatible types within an <see cref="Assembly"/> using
    /// <see cref="RegisterDocumentClass"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is a shortcut to separately calling <see cref="RegisterDocumentClass"/> on each type within an
    /// <see cref="Assembly"/>.  Instead this method scans the assembly for every compatible type and registers them.
    /// </para>
    /// </remarks>
    /// <param name="fromAssembly">
    /// A <see cref="Assembly"/>
    /// </param>
    public static void RegisterDocumentClasses(Assembly fromAssembly)
    {
      Type[] allTypes;
      
      if(fromAssembly == null)
      {
        throw new ArgumentNullException("fromAssembly");
      }
      
      allTypes = fromAssembly.GetExportedTypes();
      
      foreach(Type type in allTypes)
      {
        if(ImplementsRequiredInterface(type))
        {
          RegisterDocumentClass(type);
        }
      }
    }
		
		/// <summary>
		/// <para>
		/// Removes a <see cref="Type"/> from the static collection of known 'foreign' document classes.
		/// </para>
		/// </summary>
		/// <param name="typeToRegister">
		/// A <see cref="Type"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool UnregisterDocumentClass(Type typeToRegister)
		{
			if(typeToRegister == null)
			{
				throw new ArgumentNullException("typeToRegister");
			}
			
			return ForeignDocumentClasses.Remove(typeToRegister.FullName);
		}
		
    /// <summary>
    /// <para>
    /// Determines whether or not the given <paramref name="targetType"/> implements the required interface to
    /// correctly describe a ZPT document.
    /// </para>
    /// </summary>
    /// <param name="targetType">
    /// A <see cref="Type"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool ImplementsRequiredInterface(Type targetType)
    {
      if(targetType == null)
      {
        throw new ArgumentNullException("targetType");
      }
      
      return (targetType.GetInterface(RequiredDocumentInterface.FullName) == RequiredDocumentInterface);
    }
    
		#endregion
	}
}

