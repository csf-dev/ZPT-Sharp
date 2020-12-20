using System;
using System.IO;
using Microsoft.Extensions.Logging;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// Default implementation of <see cref="IGetsSourceAnnotationString"/> which gets source information strings.
    /// </summary>
    public class SourceAnnotationStringProvider : IGetsSourceAnnotationString
    {
        readonly RenderingConfig config;
        readonly ILogger logger;

        /// <summary>
        /// Gets a string which represents information about a document's source information.
        /// </summary>
        /// <returns>The source info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        public string GetSourceInfo(IDocumentSourceInfo sourceInfo)
        {
            if (sourceInfo == null) return null;

            if (logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace("Source info is {info_type}: {source_info}", sourceInfo.GetType().Name, sourceInfo);

            if (!(sourceInfo is FileSourceInfo fileSourceInfo))
                return sourceInfo.ToString();

            if (logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace("Configuration source annotation base path: {base_path}", config.SourceAnnotationBasePath ?? "<null>");

            var path = fileSourceInfo.ToString();
            if (String.IsNullOrEmpty(config.SourceAnnotationBasePath))
                return path;

            return StripSourceAnnotationBasePathAndLeadingDirectorySeparators(path);
        }

        string StripSourceAnnotationBasePathAndLeadingDirectorySeparators(string path)
        {
            if (!path.StartsWith(config.SourceAnnotationBasePath, StringComparison.InvariantCulture))
                return path;

            var relativePath = path.Substring(config.SourceAnnotationBasePath.Length);

            while (relativePath.Length > 0 && relativePath[0] == (Path.DirectorySeparatorChar))
                relativePath = relativePath.Substring(1);

            return relativePath;
        }

        /// <summary>
        /// Gets a string which represents information about the source of an node start-tag.
        /// </summary>
        /// <returns>The start-tag info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        public string GetStartTagInfo(NodeSourceInfo sourceInfo)
            => GetSourceInfo(sourceInfo?.Document, sourceInfo?.StartTagLineNumber);

        /// <summary>
        /// Gets a string which represents information about the source of an node end-tag.
        /// </summary>
        /// <returns>The end-tag info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        public string GetEndTagInfo(NodeSourceInfo sourceInfo)
            => GetSourceInfo(sourceInfo?.Document, sourceInfo?.EndTagLineNumber);

        string GetSourceInfo(IDocumentSourceInfo sourceInfo, int? lineNumber)
        {
            if (sourceInfo == null) return null;
            var docSouce = GetSourceInfo(sourceInfo);

            if (lineNumber == null) return docSouce;
            return $"{docSouce} (line {lineNumber})";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceAnnotationStringProvider"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="logger">Logger</param>
        public SourceAnnotationStringProvider(RenderingConfig config, ILogger<SourceAnnotationStringProvider> logger)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
