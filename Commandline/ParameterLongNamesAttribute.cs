using System;
using System.Collections.Generic;

namespace CraigFowler.Cli
{
  /// <summary>
  /// <para>
  /// Used to decorate an enumeration constant, where that constant represents a parameter that an application
  /// accepts.  This attribute is used to identify "long names" for the parameter.  Most parameters have exactly one
  /// long name.
  /// </para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class ParameterLongNamesAttribute : Attribute
  {
    #region properties
    
    /// <summary>
    /// <para>Gets and sets a collection of the names for the parameter.</para>
    /// </summary>
    public IList<string> Names {
      get;
      set;
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance with a collection of names.</para>
    /// </summary>
    /// <param name="names">
    /// A <see cref="System.String[]"/>
    /// </param>
    public ParameterLongNamesAttribute (params string[] names)
    {
      this.Names = names;
    }
    
    #endregion
  }
}

