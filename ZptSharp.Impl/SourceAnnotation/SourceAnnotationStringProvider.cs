using System;
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

        /// <summary>
        /// Gets a string which represents information about a document's source information.
        /// </summary>
        /// <returns>The source info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        public string GetSourceInfo(IDocumentSourceInfo sourceInfo)
        {
            if (sourceInfo == null) return null;
            if (!(sourceInfo is FileSourceInfo fileSourceInfo))
                return sourceInfo.ToString();

            var path = fileSourceInfo.ToString();
            if (String.IsNullOrEmpty(config.SourceAnnotationBasePath))
                return path;

            if (path.StartsWith(config.SourceAnnotationBasePath, StringComparison.InvariantCulture))
                return path.Substring(config.SourceAnnotationBasePath.Length);

            return path;
        }

        /// <summary>
        /// Gets a string which represents information about the source of an element start-tag.
        /// </summary>
        /// <returns>The start-tag info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        public string GetStartTagInfo(ElementSourceInfo sourceInfo)
            => GetSourceInfo(sourceInfo?.Document, sourceInfo?.StartTagLineNumber);

        /// <summary>
        /// Gets a string which represents information about the source of an element end-tag.
        /// </summary>
        /// <returns>The end-tag info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        public string GetEndTagInfo(ElementSourceInfo sourceInfo)
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
        public SourceAnnotationStringProvider(RenderingConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }
    }
}
