using System.Collections.Generic;

namespace ZptSharp.Cli
{
    /// <summary>
    /// Represents the arguments (options) which are available to the CLI application.
    /// </summary>
    public class CliArguments
    {
        /// <summary>
        /// The only mandatory argument; this gets or sets the root path which
        /// shall be used for the bulk file rendering operation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Whilst this property is a list, it should always have precisely one value.
        /// </para>
        /// </remarks>
        /// <value>The root path.</value>
        public IList<string> RootPath { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a comma-separated list of file/path glob patterns to
        /// include in the operation.  If left unset then this defaults to
        /// <c>*.*</c>.  IE: All files in the directory, but no recursive searching.
        /// </summary>
        /// <value>The include patterns.</value>
        public string CommaSeparatedIncludePatterns { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of file/path glob patterns to
        /// exclude from the operation.  If left unset then this defaults to
        /// empty/no exclusions.
        /// </summary>
        /// <value>The exclude patterns.</value>
        public string CommaSeparatedExcludePatterns { get; set; }

        /// <summary>
        /// Gets or sets an optional path to a JSON-formatted file which shall be used
        /// as the model for the rendering operation.
        /// </summary>
        /// <value>The model JSON file path.</value>
        public string PathToModelJson { get; set; }

        /// <summary>
        /// Gets or sets an optional collection of key/value pairs which shall be used
        /// in the rendering operation as the "TALES keyword options".
        /// </summary>
        /// <remarks>
        /// <para>
        /// These should be passed as comma-separated list of key/value pairs in the
        /// format <c>key=value</c>.  For example, the string <c>weather=cloudy,time=13:45</c>
        /// would create two keyword options. Those would be <c>weather</c> with a value
        /// of <c>cloudy</c> &amp; <c>time</c> with a value of <c>13:45</c>.
        /// </para>
        /// </remarks>
        /// <value>The keyword options.</value>
        public string CommaSeparatedKeywordOptions { get; set; }

        /// <summary>
        /// Gets the directory path to where output files should be saved.  If this is null
        /// then the current directory shall be used.
        /// </summary>
        /// <value>The output path.</value>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets or sets an optional file extension to be used for the output files.
        /// If left unset then the files will be output with the same extension as the
        /// source files.
        /// </summary>
        /// <value>The output file extension.</value>
        public string OutputFileExtension { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates whether or not AngleSharp should be used for
        /// rendering HTML ZPT documents.  If <see langword="false" /> then HTML Agility Pack
        /// will be used instead.
        /// </summary>
        /// <value>Whether or not to use AngleSharp for HTML.</value>
        public bool UseAngleSharp { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates whether or not source annotation should be
        /// enabled.
        /// </summary>
        /// <value>Whether or not to use source annotation.</value>
        public bool EnableSourceAnnotation { get; set; }
    }
}