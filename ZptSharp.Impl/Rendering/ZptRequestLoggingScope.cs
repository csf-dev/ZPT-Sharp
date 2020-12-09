using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// A type to represent the logging scope for a single ZPT rendering request.
    /// </summary>
    public class ZptRequestLoggingScope
    {
        /// <summary>
        /// Gets a unique identifier for the rendering request/operation.  This is an arbitrary
        /// ID, used to correlate log messages which relate to the same request.
        /// </summary>
        /// <value>The request identifier.</value>
        public Guid RequestId { get; }

        /// <summary>
        /// Gets information about the source of the primary document being rendered.
        /// </summary>
        /// <value>The document source info.</value>
        public IDocumentSourceInfo SourceInfo { get; }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="ZptRequestLoggingScope"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="ZptRequestLoggingScope"/>.</returns>
        public override string ToString()
            => $"[ZPT rendering request {RequestId.ToString("D")}:{SourceInfo}]";

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptRequestLoggingScope"/> class.
        /// </summary>
        /// <param name="sourceInfo">Source info.</param>
        /// <param name="requestId">Request identifier.</param>
        public ZptRequestLoggingScope(IDocumentSourceInfo sourceInfo, Guid? requestId = null)
        {
            SourceInfo = sourceInfo ?? throw new ArgumentNullException(nameof(sourceInfo));
            RequestId = requestId ?? Guid.NewGuid();
        }
    }
}
