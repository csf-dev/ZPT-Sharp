
using System;

namespace CraigFowler.Cli
{
  /// <summary>
  /// <para>Enumerates the various types of commandline parameters.</para>
  /// </summary>
  public enum ParameterType : int
  {
    /// <summary>
    /// <para>
    /// A parameter that never takes an associated value; the parameter acts as a flag.  Only the presence of this
    /// parameter will be recorded, never any value.
    /// </para>
    /// </summary>
    NoValue,
    
    /// <summary>
    /// <para>
    /// The parameter has an optional value associated with it.  If the following item on the commandline is not a
    /// parameter definition then it will be interpreted as the value to this parameter.  If the following item does
    /// appear to be a parameter definition then it will be treated as such.
    /// </para>
    /// </summary>
    ValueOptional,
    
    /// <summary>
    /// <para>
    /// Similar to <see cref="ValueOptional"/> but the following item on the commandline will be treated as a value
    /// to this parameter regardless of what it looks like.  If there is no following value then this will be treated
    /// as an error.
    /// </para>
    /// </summary>
    ValueRequired
  }
}
