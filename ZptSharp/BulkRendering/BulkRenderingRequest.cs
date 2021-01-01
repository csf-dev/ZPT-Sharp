using System.Collections.Generic;
using ZptSharp.Config;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Represents a request to render many template files in bulk.
    /// </summary>
    public class BulkRenderingRequest
    {
        /// <summary>
        /// Gets or sets a collection of paths/glob patterns used to match valid input templates.
        /// </summary>
        /// <value>The included paths where input templates are found.</value>
        public ICollection<string> IncludedPaths { get; set; } = new List<string>();
        
        /// <summary>
        /// Gets or sets a collection of paths/glob patterns used to match files which should
        /// be excluded from the operation.  The files matched by these exclusions
        /// are subtracted from <see cref="IncludedPaths" /> before rendering begins.
        /// </summary>
        /// <value>The excluded paths.</value>
        public ICollection<string> ExcludedPaths { get; set; } = new List<string>();
        
        /// <summary>
        /// Gets or sets a single directory path, used as reference for the 'root' input
        /// directory.  This will be used to determine each input file's relative
        /// path from the input.  This allows it to be written to the same relative
        /// path for output.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If left null or empty, then the current working directory will be used as the root path.
        /// Note that any input files which are not descendents of the root path will not
        /// be included in the rendering operation.
        /// </para>
        /// </remarks>
        /// <value>The root path for the input files.</value>
        public string InputRootPath { get; set; }
    
        /// <summary>
        /// Gets or sets the model object to be used when rendering the templates.
        /// </summary>
        /// <value>The model.</value>
        public object Model { get; set; }
        
        /// <summary>
        /// Gets or sets the rendering configuration to be used when rendering the templates.
        /// </summary>
        /// <value>The rendering config.</value>
        public RenderingConfig RenderingConfig { get; set; } = RenderingConfig.Default;
        
        /// <summary>
        /// Gets the directory path to where the files shall be output once they are rendered.
        /// </summary>
        /// <value>The output path.</value>
        public string OutputPath { get; set; }

        /// <summary>
        /// If set, then output files will have their file extension replaced
        /// with this extension, instead of their previous extension.
        /// </summary>
        /// <value>The output file extension.</value>
        public string OutputFileExtension { get; set; }
    }
}