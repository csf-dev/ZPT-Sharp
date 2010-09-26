
using System;
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Web.ZPT.Mocks
{
	/// <summary>
	/// <para>Mock object for TALES tests as defined in Zope's TALES test suite.</para>
	/// </summary>
	public class TalesTestObject
	{
		#region fields
		
		private Dictionary<string, object> data;
		
		#endregion
		
		#region properties
		
		/// <summary>
		/// <para>Gets and sets values within this mock instance.</para>
		/// </summary>
		/// <param name="key">
		/// A <see cref="System.String"/>
		/// </param>
		public object this[string key]
		{
			get {
				return data[key];
			}
			set {
				data[key] = value;
			}
		}
		
		#endregion
		
		#region methods
		
		/// <summary>
		/// <para>Creates a new <see cref="TalesContext"/> from this instance.</para>
		/// </summary>
		public TalesContext CreateContext()
		{
			TalesContext output = new TalesContext();
			
			foreach(string key in data.Keys)
			{
				output.AddDefinition(key, data[key]);
			}
			
			return output;
		}
		
		#endregion
		
		#region constructor
		
		/// <summary>
		/// <para>Initialises this instance ready for tests.</para>
		/// </summary>
		public TalesTestObject()
		{
			Dictionary<string, object>
				xData = new Dictionary<string, object>(),
				yData = new Dictionary<string, object>(),
				atData = new Dictionary<string, object>();
			
			xData["name"] = "xander";
			xData["y"] = new Dictionary<string, object>();
			((Dictionary<string, object>) xData["y"])["name"] = "yikes";
			((Dictionary<string, object>) xData["y"])["z"] = new Dictionary<string, object>();
			((Dictionary<string, object>) ((Dictionary<string, object>) xData["y"])["z"])["name"] = "zope";
			
			yData["z"] = 3;
			
			atData["name"] = "yikes";
			atData["_d"] = xData;
			
			data = new Dictionary<string, object>();
			data["x"] = xData;
			data["y"] = yData;
			data["b"] = "boot";
			data["B"] = 2;
			data["adapterTest"] = atData;
			data["dynamic"] = "z";
			data["ErrorGenerator"] = new ExceptionThrower();
		}
		
		#endregion
		
		#region inner class
		
		/// <summary>
		/// <para>Utility class to generate and throw exceptions.</para>
		/// </summary>
		public class ExceptionThrower
		{
			/// <summary>
			/// <para>Read-only.  Constant value gets the default exception message.</para>
			/// </summary>
			public const string ExceptionMessage = "Exception thrown as requested";
			
			public static readonly string[] ExceptionTypes = new string[] { "ArgumentException",
																																			"InvalidOperationException",
																																		  "Undefined" };
			
			/// <summary>
			/// <para>Always throws an exception - the exact type depending upon the <paramref name="key"/>.</para>
			/// </summary>
			/// <param name="key">
			/// A <see cref="System.String"/>
			/// </param>
			public string this[string key]
			{
				get {
					Exception exceptionToThrow;
					
					switch(key)
					{
					case "ArgumentException":
						exceptionToThrow = new ArgumentException(ExceptionMessage);
						break;
					case "InvalidOperationException":
						exceptionToThrow = new InvalidOperationException(ExceptionMessage);
						break;
					default:
						exceptionToThrow = new Exception(ExceptionMessage);
						break;
					}
					
					throw exceptionToThrow;
				}
			}
		}
		
		#endregion
	}
}
