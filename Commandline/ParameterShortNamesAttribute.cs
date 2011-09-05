using System;
using System.Collections.Generic;

namespace CraigFowler.Cli
{
  /// <summary>
  /// <para>
  /// Used to decorate an enumeration constant, where that constant represents a parameter that an application
  /// accepts.  This attribute is used to identify "short names" for the parameter.  Most parameters have either zero
  /// or one short name.  Short names are ideal for frequently-used parameters as a shortcut.
  /// </para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class ParameterShortNamesAttribute : Attribute
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
    public ParameterShortNamesAttribute (params string[] names)
    {
      this.Names = names;
    }
    
    #endregion
  }
}

