using System;
using CraigFowler.Cli;

namespace CraigFowler.Web.ZPT.Builder
{
  /// <summary>
  /// <para>Enumerates the commandline parameters that are accepted by the builder application.</para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// These options are used to populate many of the properties that are available on the
  /// <see cref="CraigFowler.Web.ZPT.ZptPageBuilder"/> that will ultimately be called to build the output files.
  /// </para>
  /// </remarks>
  public enum BuilderParameterType : int
  {
    /// <summary>
    /// <para>A filename pattern that is used to detect files that should be parsed.</para>
    /// <seealso cref="CraigFowler.Web.ZPT.ZptPageBuilder.InputFilenamePattern"/>
    /// </summary>
    [Parameter(Type = ParameterType.ValueRequired)]
    [ParameterLongNames("input-filename-pattern")]
    [ParameterShortNames("i")]
    InputFilenamePattern,
    
    /// <summary>
    /// <para>The root directory path where output files will be saved.</para>
    /// <seealso cref="CraigFowler.Web.ZPT.ZptPageBuilder.OutputPath"/>
    /// </summary>
    [Parameter(Type = ParameterType.ValueRequired)]
    [ParameterLongNames("output")]
    [ParameterShortNames("o")]
    OutputPath,
    
    /// <summary>
    /// <para>The file extension that will be applied to the saved output files.</para>
    /// <seealso cref="CraigFowler.Web.ZPT.ZptPageBuilder.OutputFilenameExtension"/>
    /// </summary>
    [Parameter(Type = ParameterType.ValueRequired)]
    [ParameterLongNames("output-filename-extension")]
    [ParameterShortNames("e")]
    OutputFileExtension,
    
    /// <summary>
    /// <para>
    /// The filename for the 'ignore marker' file that (where detected in a directory) indicates that template
    /// documents within that directory (and subdirectories) should not be built as output files.
    /// </para>
    /// <seealso cref="CraigFowler.Web.ZPT.ZptPageBuilder.IgnoreMarkerFilename"/>
    /// </summary>
    [Parameter(Type = ParameterType.ValueRequired)]
    [ParameterLongNames("ignore-marker-filename")]
    [ParameterShortNames("g")]
    IgnoreMarkerFilename,
    
    /// <summary>
    /// <para>An aggregate string that contains the command-line variable assignments for the page builder.</para>
    /// <seealso cref="CraigFowler.Web.ZPT.ZptPageBuilder.CommandLineOptions"/>
    /// </summary>
    [Parameter(Type = ParameterType.ValueRequired)]
    [ParameterLongNames("options")]
    [ParameterShortNames("c")]
    OptionDefinitions
  }
}

