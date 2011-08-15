//  
//  TalesStructureProvider.cs
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
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tales.Exceptions;

namespace CraigFowler.Web.ZPT.Tales
{
	/// <summary>
	/// <para>
	/// A simple "structure provider" type that assists in the representation of hierarchical structures in a
	/// TALES-based environment.
	/// </para>
	/// </summary>
	public class TalesStructureProvider
	{
		#region properties
		
		/// <summary>
		/// <para>
		/// Read-only.  Gets the underlying dictionary of name-to-object assignments for this level of the structure.
		/// </para>
		/// </summary>
		protected Dictionary<string,object> UnderlyingCollection {
			get;
			private set;
		}
		
		/// <summary>
		/// <para>Read-only.  Gets a named item from the <see cref="UnderlyingCollection"/>.</para>
		/// </summary>
		/// <param name="index">
		/// A <see cref="System.String"/>
		/// </param>
		public virtual object this [string index]
		{
			get {
				return this.UnderlyingCollection[index];
			}
		}
		
		#endregion
		
		#region methods
    
    /// <summary>
    /// <para>
    /// Determines whether or not the <see cref="UnderlyingCollection"/> contains a reference to the given path
    /// component.
    /// </para>
    /// </summary>
    /// <param name="key">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public bool ContainsKey(string key)
    {
      return this.UnderlyingCollection.ContainsKey(key);
    }
		
		/// <summary>
		/// <para>
		/// Overloaded.  Stores an item within the structure at a position indicated by the relative TALES path.
		/// </para>
		/// </summary>
		/// <param name="relativePath">
		/// A <see cref="TalesPath"/> that indicates the path (relative to the position of this structure provider in
		/// a TALES hierarchy) at which to store the <paramref name="item"/>.
		/// </param>
		/// <param name="item">
		/// A <see cref="System.Object"/> to store at the specified point within the hierarchy of this instance.
		/// </param>
		[TalesIgnore]
    public virtual void StoreItem(TalesPath relativePath, object item)
		{
			if(relativePath == null)
			{
				throw new ArgumentNullException("relativePath");
			}
			
      StoreItem(relativePath.Parts, 0, item);
			try
			{
				StoreItem(relativePath.Parts, 0, item);
			}
			catch(NotSupportedException inner)
			{
				TalesStructureException ex = new TalesStructureException(TalesStructureErrorType.Store, inner);
				
				ex.ErrorPosition = (int) inner.Data["Current position"];
				ex.Path = relativePath;
				ex.Provider = this;
				
				throw ex;
			}
		}
    
    /// <summary>
    /// <para>Overloaded.  Removes an item that is stored within this instance.</para>
    /// </summary>
    /// <param name="relativePath">
    /// A <see cref="TalesPath"/> that indicates the path (relative to the position of this structure provider in
    /// a TALES hierarchy) at which to remove the contents.
    /// </param>
    [TalesIgnore]
    public void RemoveItem(TalesPath relativePath)
    {
      if(relativePath == null)
      {
        throw new ArgumentNullException("relativePath");
      }
      
      try
      {
        RemoveItem(relativePath.Parts, 0);
      }
      catch(NotSupportedException inner)
      {
        TalesStructureException ex = new TalesStructureException(TalesStructureErrorType.Store, inner);
        
        ex.ErrorPosition = (int) inner.Data["Current position"];
        ex.Path = relativePath;
        ex.Provider = this;
        
        throw ex;
      }
    }
		
    /// <summary>
    /// <para>
    /// Overloaded.  Retrieves a single item from this instance, from a location identified by <paramref name="path"/>.
    /// </para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    public object RetrieveItem(string path)
    {
      TalesPath talesPath = new TalesPath(path);
      return this.RetrieveItem(talesPath);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Retrieves a single item from this instance, from a location identified by <paramref name="path"/>.
    /// </para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>
    /// </returns>
    public object RetrieveItem(TalesPath path)
    {
      TalesContext context = new TalesContext();
      
      if(path == null)
      {
        throw new ArgumentNullException("path");
      }
      else if(path.Parts.Count < 1)
      {
        throw new ArgumentException("TALES path has no parts.", "path");
      }
      
      context.AddDefinition(path.Parts[0], this);
      return context.CreateExpression(path.ToString()).GetValue();
    }
    
		/// <summary>
		/// <para>
		/// Overloaded.  Stores an item either within this instance or (if a deeper level of hierarchy is indicated) at a
		/// deeper level of the hierarchy.  Child branches within the hierarchy are automatically created if they do not
		/// already exist.
		/// </para>
		/// </summary>
		/// <param name="pathParts">
		/// A collection of <see cref="System.String"/> that indicates the path where the <paramref name="item"/> should
		/// be stored.
		/// </param>
		/// <param name="currentPosition">
		/// A <see cref="System.Int32"/> that indicates the current position (zero-based) in traversing the
		/// <paramref name="pathParts"/>.
		/// </param>
		/// <param name="item">
		/// A <see cref="System.Object"/> to store at the tip of the path described by <paramref name="pathParts"/>.
		/// </param>
		protected void StoreItem(List<string> pathParts, int currentPosition, object item)
		{
			bool atLastPosition;
			string currentName;
			
			if(pathParts == null)
			{
				throw new ArgumentNullException("pathParts");
			}
			else if(pathParts.Count == 0)
			{
				throw new ArgumentException("The path at which to store the item is empty", "pathParts");
			}
			
			atLastPosition = (currentPosition == pathParts.Count - 1);
			currentName = pathParts[currentPosition];
			
			if(atLastPosition)
			{
				this.UnderlyingCollection[currentName] = item;
			}
			else
			{
				TalesStructureProvider childStructure;
				
				if(!this.UnderlyingCollection.ContainsKey(currentName))
				{
					this.UnderlyingCollection.Add(currentName, new TalesStructureProvider());
				}
				else if(!(this.UnderlyingCollection[currentName] is TalesStructureProvider))
				{
					string message = "Cannot traverse into an object instance that is not a TalesStructureProvider";
					NotSupportedException ex = new NotSupportedException(message);
					
					ex.Data["Current position"] = currentPosition;
					
					throw ex;
				}
				
				childStructure = (TalesStructureProvider) this.UnderlyingCollection[currentName];
				childStructure.StoreItem(pathParts, currentPosition + 1, item);
			}
		}
    
    /// <summary>
    /// <para>
    /// Overloaded.  Removes an item that is stored within this instance.
    /// </para>
    /// </summary>
    /// <param name="pathParts">
    /// A collection of <see cref="System.String"/> that indicates the path where the <paramref name="item"/> should
    /// be removed from.
    /// </param>
    /// <param name="currentPosition">
    /// A <see cref="System.Int32"/> that indicates the current position (zero-based) in traversing the
    /// <paramref name="pathParts"/>.
    /// </param>
    protected void RemoveItem(List<string> pathParts, int currentPosition)
    {
      bool atLastPosition;
      string currentName;
      
      if(pathParts == null)
      {
        throw new ArgumentNullException("pathParts");
      }
      else if(pathParts.Count == 0)
      {
        throw new ArgumentException("The path at which to store the item is empty", "pathParts");
      }
      
      atLastPosition = (currentPosition == pathParts.Count - 1);
      currentName = pathParts[currentPosition];
      
      if(atLastPosition)
      {
        this.UnderlyingCollection.Remove(currentName);
      }
      else
      {
        TalesStructureProvider childStructure;
        
        if(!this.UnderlyingCollection.ContainsKey(currentName))
        {
          /* Do nothing here - the path we have been asked to remove from does not exist.
           * In this scenario we are going to silently fail to do anything.
           */
        }
        else if(!(this.UnderlyingCollection[currentName] is TalesStructureProvider))
        {
          string message = "Cannot traverse into an object instance that is not a TalesStructureProvider";
          NotSupportedException ex = new NotSupportedException(message);
          
          ex.Data["Current position"] = currentPosition;
          
          throw ex;
        }
        else
        {
          childStructure = (TalesStructureProvider) this.UnderlyingCollection[currentName];
          childStructure.RemoveItem(pathParts, currentPosition + 1);
        }
      }
    }
		
		#endregion
		
		#region constructor
		
		/// <summary>
		/// <para>Initialises this instance with an empty <see cref="UnderlyingCollection"/>.</para>
		/// </summary>
		public TalesStructureProvider ()
		{
			this.UnderlyingCollection = new Dictionary<string, object>();
		}
		
		#endregion
	}
}

