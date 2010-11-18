//  
//  Path.cs
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
using System.IO;
using CraigFowler.Web.ZPT.Tales.Exceptions;

namespace CraigFowler.Web.ZPT.Tales
{
  /// <summary>
  /// <para>Represents an individual path used within a <see cref="Expressions.PathExpression"/>.</para>
  /// </summary>
  public class TalesPath
  {
    #region constants
    
    private const char
			PARTS_SEPARATOR 									= '/',
			PREFIX_SEPARATOR									= ':';
    
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
    
    #endregion
    
    #region fields
    
    private string rawPath;
		private List<string> parts;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the raw text that this path represents.</para>
    /// </summary>
    public string Text
    {
      get {
        return rawPath;
      }
      private set {
        rawPath = null;
        
        if(value != null)
        {
          rawPath = value.Trim();
        }
        
        if(String.IsNullOrEmpty(rawPath))
        {
          rawPath = null;
        }
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets an ordered collection of parts to this path - IE the object names separated by forward-slash
    /// characters.
    /// </para>
    /// </summary>
    public List<string> Parts
    {
      get {
				return parts;
			}
      private set {
				if(value == null)
				{
					throw new ArgumentNullException("value");
				}
				
				parts = value;
			}
    }
		
		/// <summary>
		/// <para>Read-only.  Gets the prefix for this TALES path.</para>
		/// </summary>
		public string Prefix
		{
			get;
			private set;
		}
		
		/// <summary>
		/// <para>
		/// Read-only.  Gets a nullable <see cref="DefinitionType"/> based on the current value of <see cref="Prefix"/>.
		/// </para>
		/// </summary>
		public DefinitionType? VariableType
		{
			get {
				DefinitionType? output = null;
				
				switch(this.Prefix)
				{
				case LocalDefinitionIdentifier:
					output = DefinitionType.Local;
					break;
				case GlobalDefinitionIdentifier:
					output = DefinitionType.Global;
					break;
				}
				
				return output;
			}
		}
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Returns a string representation of this path instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public override string ToString ()
    {
      return this.ToString(this.Parts.Count);
    }
    
    /// <summary>
    /// <para>
    /// Returns a string representation of this path instance, showing only the given number of path pieces.
    /// </para>
    /// </summary>
    /// <param name="partCount">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public string ToString(int partCount)
    {
			string output;
			
      if(partCount < 0)
      {
        throw new ArgumentOutOfRangeException("partCount", "Parameter 'partCount' must be more than or equal to zero.");
      }
      
			output = String.Join(PARTS_SEPARATOR.ToString(), this.Parts.ToArray(), 0, partCount);
			
			if(!String.IsNullOrEmpty(this.Prefix))
			{
				output = String.Format("{0}:{1}", this.Prefix, output);
			}
			
      return output;
    }
    
    /// <summary>
    /// <para>Overridden.  Evaluates whether this instance equals the given <paramref name="obj"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This will only return true if <paramref name="obj"/> is a <see cref="TalesPath"/> and its
    /// <see cref="TalesPath.Text"/> matches the Text property on this instance.
    /// </para>
    /// </remarks>
    /// <param name="obj">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public override bool Equals (object obj)
    {
      bool output;
      
      if(obj != null && obj is TalesPath)
      {
        output = ((TalesPath) obj).Text == this.Text;
      }
      else
      {
        output = false;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Gets a hash code for this instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>
    public override int GetHashCode ()
    {
      return (this.Text != null)? this.Text.GetHashCode() : "{NULL PATH}".GetHashCode();
    }
    
		/// <summary>
		/// <para>Creates and returns a new <see cref="TalesPath"/> instance based on a subset of this path.</para>
		/// </summary>
		/// <param name="startPosition">
		/// A <see cref="System.Int32"/>, the zero-based index position (based on the collection <see cref="Parts"/>) at
		/// which to begin the sub-path.
		/// </param>
		/// <returns>
		/// A <see cref="TalesPath"/> representing the resultant sub-path.
		/// </returns>
		public TalesPath SubPath(int startPosition)
		{
			return this.SubPath(startPosition, (this.Parts.Count - startPosition));
		}
		
		/// <summary>
		/// <para>Creates and returns a new <see cref="TalesPath"/> instance based on a subset of this path.</para>
		/// </summary>
		/// <param name="startPosition">
		/// A <see cref="System.Int32"/>, the zero-based index position (based on the collection <see cref="Parts"/>) at
		/// which to begin the sub-path.
		/// </param>
		/// <param name="count">
		/// A <see cref="System.Int32"/>, the number of path parts to include within the sub-path.
		/// </param>
		/// <returns>
		/// A <see cref="TalesPath"/> representing the resultant sub-path.
		/// </returns>
		public TalesPath SubPath(int startPosition, int count)
		{
			List<string> newParts = this.Parts.GetRange(startPosition, count);
			return new TalesPath(newParts);
		}
		
    #endregion
    
    #region private methods
    
    /// <summary>
    /// <para>
    /// Generates a collection of the parts of the given path, separating the input by forward-slash characters.
    /// </para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="prefix">
    /// A <see cref="System.String"/>, the prefix for the path that this instance is to represent.
    /// </param>
    /// <returns>
    /// A collection of <see cref="System.String"/> instances.
    /// </returns>
    /// <exception cref="FormatException">
    /// If the given <paramref name="path" /> contains a component that is null or empty then this exception is raised.
    /// </exception>
    private List<string> ExtractPathParts(string path, out string prefix)
    {
      string[] rawParts;
      List<string> output = new List<string>();
			
			if(path != null)
			{
				int prefixEnd = path.IndexOf(PREFIX_SEPARATOR);
				string remainingPath;
				
				if(prefixEnd >= 1)
				{
					prefix = path.Substring(0, prefixEnd);
					remainingPath = path.Substring(prefixEnd + 1);
				}
				else
				{
					prefix = null;
					remainingPath = path;
				}
				
				rawParts = remainingPath.Split(new char[] { PARTS_SEPARATOR });
				
	      foreach(string part in rawParts)
	      {
	        if(String.IsNullOrEmpty(part))
	        {
	          throw new FormatException("The given path is not a correctly-formatted path specification.  " +
	                                    "A part of the path is null or an empty string.");
	        }
	        else
	        {
	          output.Add(part);
	        }
	      }
			}
			else
			{
				prefix = null;
			}
      
      return output;
    }
    
    #endregion
    
    #region constructors
    
		/// <summary>
		/// <para>Private constructor initialises properties to empty values.</para>
		/// </summary>
		private TalesPath ()
		{
			this.Text = null;
			this.Prefix = null;
		}
		
    /// <summary>
    /// <para>Initialises this instance with a new path string.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="System.String"/>
    /// </param>
    /// <exception cref="FormatException">
    /// If the given <paramref name="path" /> contains a component that is null or empty then this exception is raised.
    /// </exception>
    public TalesPath(string path) : this()
    {
			string prefix;
			
      this.Text = path;
      this.Parts = this.ExtractPathParts(this.Text, out prefix);
			this.Prefix = prefix;
    }
		
		/// <summary>
		/// <para>
		/// Initialises an instance with a given list of <paramref name="parts"/> that should be used as the path.
		/// </para>
		/// </summary>
		/// <param name="parts">
		/// A collection of <see cref="System.String"/> that will become the parts of the path.
		/// </param>
		protected TalesPath(List<string> parts) : this()
		{
			this.Parts = parts;
			this.Text = this.ToString();
		}
    
    /// <summary>
    /// <para>Initialises this instance with an enumerable collection of <see cref="System.String"/>.</para>
    /// </summary>
    /// <param name="parts">
    /// An enumerable collection of <see cref="System.String"/>
    /// </param>
    public TalesPath(IEnumerable<string> parts) : this()
    {
      if(parts == null)
      {
        throw new ArgumentNullException("parts");
      }
      
      this.Parts = new List<string>();
      this.Parts.AddRange(parts);
      this.Text = this.ToString();
      
    }
    
    #endregion
    
    #region static methods
    
    /// <summary>
    /// <para>
    /// Gets a <see cref="TalesPath"/> instance that represents the relative path of <paramref name="childPath"/> from
    /// <paramref name="basePath"/>.
    /// </para>
    /// </summary>
    /// <param name="basePath">
    /// A <see cref="FileSystemInfo"/>
    /// </param>
    /// <param name="childPath">
    /// A <see cref="FileSystemInfo"/>
    /// </param>
    /// <returns>
    /// A <see cref="TalesPath"/>
    /// </returns>
    public static TalesPath GetRelativePath(FileSystemInfo basePath, FileSystemInfo childPath)
    {
      string
        baseName,
        childName,
        relativePath;
      
      if(basePath == null)
      {
        throw new ArgumentNullException("basePath");
      }
      else if(childPath == null)
      {
        throw new ArgumentNullException("childPath");
      }
      
      baseName = basePath.FullName;
      childName = childPath.FullName;
      
      if(!childName.StartsWith(baseName))
      {
        string message = "Cannot create a relative TalesPath; the child path is not a child of the base path.";
        TalesException ex = new TalesException(message);
        ex.Data["Base path"] = basePath;
        ex.Data["Child path"] = childPath;
        
        throw ex;
      }
      
      relativePath = childName.Substring(baseName.Length + 1);
      relativePath = relativePath.Replace('\\', '/');
      
      return new TalesPath(relativePath);
    }
    
    #endregion
  }
}
