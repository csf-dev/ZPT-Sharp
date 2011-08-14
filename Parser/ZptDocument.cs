//  
//  ZptDocument.cs
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
using CraigFowler.Web.ZPT.Tal;
using CraigFowler.Web.ZPT.Metal;
using System.Reflection;
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tales;
using System.IO;

namespace CraigFowler.Web.ZPT
{
	/// <summary>
	/// <para>
	/// Base class for simple Zope page templates documents.  Provides the minimal object model for all documents.
	/// </para>
	/// </summary>
	public class ZptDocument : IZptDocument
	{
		#region fields
		
		private ZptMetadata privateMetadata;
		
		#endregion
		
		#region properties
		
		/// <summary>
		/// <para>
		/// Read-only.  Gets whether this ZPT document has macro-expansions (METAL) enabled or not.  If this property is
		/// false then the <see cref="UnderlyingDocument"/> will be a <see cref="TalDocument"/> instead of a
		/// <see cref="MetalDocument"/>.
		/// </para>
		/// </summary>
		public virtual bool MetalEnabled
		{
			get {
				return (this.Metadata.DocumentType == ZptDocumentType.Metal);
			}
		}
		
		/// <summary>
		/// <para>Gets and sets the document metadata for this ZPT document.</para>
		/// </summary>
		public virtual ZptMetadata Metadata
		{
			get {
				return privateMetadata;
			}
			set {
				if(value == null)
				{
					throw new ArgumentNullException("value");
				}
				
				privateMetadata = value;
			}
		}
		
		/// <summary>
		/// <para>
		/// Gets and sets the underlying <see cref="ITemplateDocument"/>.  This may be either a <see cref="TalDocument"/>
		/// or a <see cref="MetalDocument"/> depending on the setting of <see cref="MetalEnabled"/>.
		/// </para>
		/// </summary>
		protected virtual ITemplateDocument UnderlyingDocument
		{
			get;
			set;
		}
    
    /// <summary>
    /// <para>
    /// Used as a cache for the <see cref="GetMacros"/> method, to prevent the macro-discovery process from happening
    /// more than once.
    /// </para>
    /// </summary>
    private MetalMacroCollection MacroCache {
      get;
      set;
    }
    
		#endregion
		
		#region methods
		
		/// <summary>
		/// <para>Gets the XML template document that is associated with this instance.</para>
		/// </summary>
		/// <returns>
		/// An object instance that implements <see cref="ITemplateDocument"/>.
		/// </returns>
		public ITemplateDocument GetTemplateDocument()
		{
			if(this.UnderlyingDocument == null)
			{
				this.UnderlyingDocument = this.Metadata.LoadDocument();
			}
      
      this.AssignModelData(this.UnderlyingDocument.TalesContext);
      
			return this.UnderlyingDocument;
		}
    
    /// <summary>
    /// <para>Gets a collection of the <see cref="MetalMacro"/> instances that this instance contains.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method performs caching of the macros in order to prevent repeated calls to the expensive
    /// macro-discovery process.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A dictionary of <see cref="MetalMacro"/>, indexed by <see cref="System.String"/>
    /// </returns>
    [TalesAlias("macros")]
    public MetalMacroCollection GetMacros()
    {
      if(this.MetalEnabled && this.MacroCache == null)
      {
        ITemplateDocument document = this.GetTemplateDocument();
        
        if(document == null)
        {
          throw new InvalidOperationException("Underlying document is null and attempts to retrieve it failed.");
        }
        
        this.MacroCache = ((IMetalDocument) document).Macros;
      }
      
      return this.MacroCache;
    }
    
    /// <summary>
    /// <para>
    /// Helper method to perform assignments of data (perhaps from a domain model of some kind) to the root
    /// <see cref="TalesContext"/> of the ZPT document that is to be rendered by this view.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method does nothing on its own, it serves as a hook that implementing classes may override in order to
    /// provide data to the TALES context of the view that will be rendering.
    /// </para>
    /// <para>
    /// In a derived class, data may be added to the <paramref name="talesContext"/> using the
    /// <see cref="TalesContext.AddDefinition(string,object)"/> method.  These definitions are added at the root level
    /// of the TALES context of the document.  This data to be added may be taken from a domain model of some kind or
    /// from a session state or any other source.
    /// </para>
    /// </remarks>
    /// <param name="talesContext">
    /// A <see cref="TalesContext"/>
    /// </param>
    protected virtual void AssignModelData(TalesContext talesContext)
    {
      // Intentionally do nothing here.  It is up to implementing classes to add code to overrides of this method.
    }
		
		#endregion
		
		#region constructors
		
		/// <summary>
		/// <para>Initialises this instance with the given <paramref name="metadata"/>.</para>
		/// </summary>
		/// <param name="metadata">
		/// A <see cref="ZptMetadata"/>
		/// </param>
		public ZptDocument(ZptMetadata metadata) : this(metadata, null) {}
    
    /// <summary>
    /// <para>
    /// Initialises this instance with the given <paramref name="metadata"/> and underlying
    /// <paramref name="document"/>.
    /// </para>
    /// </summary>
    /// <param name="metadata">
    /// A <see cref="ZptMetadata"/>
    /// </param>
    /// <param name="document">
    /// A <see cref="ITemplateDocument"/>
    /// </param>
    public ZptDocument(ZptMetadata metadata, ITemplateDocument document)
    {
      this.MacroCache = null;
      this.Metadata = metadata;
      this.UnderlyingDocument = document;
    }
		
		#endregion
		
		#region static methods
		
		/// <summary>
		/// <para>
    /// Overloaded.  Constructs a new object instance that implements <see cref="IZptDocument"/> and returns it.
    /// </para>
		/// </summary>
		/// <param name="metadata">
		/// A <see cref="ZptMetadata"/> instance that describes the document to create.
		/// </param>
		/// <returns>
		/// An <see cref="IZptDocument"/>
		/// </returns>
		public static IZptDocument DocumentFactory(ZptMetadata metadata)
		{
			ConstructorInfo constructor;
			object createdInstance;
			
			if(metadata == null)
			{
				throw new ArgumentNullException("metadata");
			}
			
			constructor = metadata.DocumentClass.GetConstructor(new Type[] { typeof(ZptMetadata) });
			
			if(constructor == null)
			{
				throw new ArgumentException("The DocumentClass described by the metadata does not provide a " +
																		"constructor that takes a 'ZptMetadata' instance.",
				                            "metadata");
			}
			
			createdInstance = constructor.Invoke(new object[] { metadata });
			return (IZptDocument) createdInstance;
		}
    
    /// <summary>
    /// <para>
    /// Overloaded.  Constructs a new object instance that implements <see cref="IZptDocument"/> and returns it.
    /// </para>
    /// </summary>
    /// <param name="documentPath">
    /// A <see cref="System.String"/> containing a filesystem path to a ZPT document file.  The metadata and document
    /// instance will be created based on the contents of this file.
    /// </param>
    /// <returns>
    /// A <see cref="IZptDocument"/>
    /// </returns>
    public static IZptDocument DocumentFactory(string documentPath)
    {
      return DocumentFactory(ZptMetadata.GetMetadata(documentPath));
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Constructs a new object instance that implements <see cref="IZptDocument"/> and returns it.
    /// </para>
    /// </summary>
    /// <param name="documentFile">
    /// A <see cref="FileInfo"/> describing the filesystem location of a ZPT document file.  The metadata and document
    /// instance will be created based on the contents of this file.
    /// </param>
    /// <returns>
    /// A <see cref="IZptDocument"/>
    /// </returns>
    public static IZptDocument DocumentFactory(FileInfo documentFile)
    {
      return DocumentFactory(ZptMetadata.GetMetadata(documentFile));
    }
		
		#endregion
	}
}

