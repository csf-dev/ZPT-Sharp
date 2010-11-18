//  
//  TalesStructureException.cs
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
namespace CraigFowler.Web.ZPT.Tales.Exceptions
{
	/// <summary>
	/// <para>
	/// Describes an exception encountered when manipulating a <see cref="TalesStructureProvider"/> instance.
	/// </para>
	/// </summary>
	public class TalesStructureException : TalesException
	{
		#region constants
		
		private const string
			STORE_MESSAGE_TEMPLATE 						= "Could not store the path '{0}' into a TALES structure provider.",
			GENERAL_MESSAGE_TEMPLATE					= "An exception occurred whilst performing {0} action with a TALES " +
																					"structure provider.";
		
		#endregion
		
		#region properties
		
		/// <summary>
		/// <para>Gets and sets a <see cref="TalesPath"/> instance associated with this error.</para>
		/// </summary>
		public TalesPath Path {
			get {
				return (TalesPath) this.Data["Path"];
			}
			set {
				this.Data["Path"] = value;
			}
		}
		
		/// <summary>
		/// <para>
		/// Gets and sets a zero-based index that describes the position within the associated <see cref="Path"/> at which
		/// this exception was encountered.
		/// </para>
		/// </summary>
		public Nullable<int> ErrorPosition {
			get {
				bool hasValue = (this.Data["Error position"] != null && this.Data["Error position"] is Nullable<int>);
				
				return hasValue? (Nullable<int>) this.Data["Error position"] : null;
			}
			set {
				this.Data["Error position"] = value;
			}
		}
		
		/// <summary>
		/// <para>
		/// Gets and sets a reference to the <see cref="TalesStructureProvider"/> associated with this exception.
		/// </para>
		/// </summary>
		public TalesStructureProvider Provider {
			get {
				return (TalesStructureProvider) this.Data["Provider"];
			}
			set {
				this.Data["Provider"] = value;
			}
		}
		
		/// <summary>
		/// <para>Read-only.  Gets the type of error that this exception represents.</para>
		/// </summary>
		public TalesStructureErrorType ErrorType {
			get {
				return (TalesStructureErrorType) this.Data["Error type"];
			}
			protected set {
				this.Data["Error type"] = value;
			}
		}
		
		/// <summary>
		/// <para>Overriden, read-only.  Gets the message associated with this exception.</para>
		/// </summary>
		public override string Message
		{
			get {
				string output;
				
				switch(this.ErrorType)
				{
				case TalesStructureErrorType.Store:
					output = GetStoreErrorMessage();
					break;
				default:
					output = String.Format(GENERAL_MESSAGE_TEMPLATE, "an unknown");
					break;
				}
				
				return output;
			}
		}
		
		#endregion
		
		#region methods
		
		/// <summary>
		/// <para>
		/// Creates and returns an error message related with an exception of type
		/// <see cref="TalesStructureErrorType.Store"/>.
		/// </para>
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		protected string GetStoreErrorMessage()
		{
			string output;
			
			if(this.Path != null)
			{
				output = String.Format(STORE_MESSAGE_TEMPLATE, this.Path.ToString());
			}
			else
			{
				output = String.Format(GENERAL_MESSAGE_TEMPLATE, "a storage");
			}
			
			return output;
		}
		
		#endregion
		
		#region constructor
		
		/// <summary>
		/// <para>Initialises this instance for a given type of error.</para>
		/// </summary>
		/// <param name="errorType">
		/// A <see cref="TalesStructureErrorType"/>
		/// </param>
		public TalesStructureException (TalesStructureErrorType errorType) : this(errorType, null) {}
		
		/// <summary>
		/// <para>Initialises this instance for a given type of error and an inner exception.</para>
		/// </summary>
		/// <param name="errorType">
		/// A <see cref="TalesStructureErrorType"/>
		/// </param>
		/// <param name="inner">
		/// A <see cref="Exception"/>
		/// </param>
		public TalesStructureException(TalesStructureErrorType errorType, Exception inner) : base(inner)
		{
			this.PermanentError = false;
			this.ErrorType = errorType;
		}
		
		#endregion
	}
}

