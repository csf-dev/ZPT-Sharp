using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// Describes the source of an <see cref="Dom.IDocument"/>, usable for
    /// debugging, diagnostic or informational purposes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In most common scenarios ZPT source documents are files on a file system.  This is not
    /// a requirement for ZptSharp though.
    /// Source documents might come from anywhere, including (but not limited to):
    /// </para>
    /// <list type="bullet">
    /// <item><description>A database</description></item>
    /// <item><description>Embedded resources</description></item>
    /// <item><description>A remote API</description></item>
    /// </list>
    /// <para>
    /// Because the document provider API: <see cref="Dom.IReadsAndWritesDocument"/> deals only
    /// with streams rather than files, the source of the document does not matter there.
    /// Implementations of this interface are then used to describe the source of the document
    /// in an extensible manner.
    /// If an application introduces a new 'kind' of source (for example a primary key from a
    /// database table) then that application may create a new implementation of this interface
    /// in order to describe that new source.
    /// </para>
    /// <para>
    /// ZptSharp comes with three implementations already created:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="FileSourceInfo"/> represents the most common scenario where a document originated
    /// as a file on a file system.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="OtherSourceInfo"/> represents an abstract concept of a source which may be
    /// described by a string, but which is specifically not a file.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="UnknownSourceInfo"/> is used in the fall-back scenario where source information
    /// is not provided.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public interface IDocumentSourceInfo : IEquatable<IDocumentSourceInfo>
    {
        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Every document source must have a name/string representation in some manner.
        /// This name should reasonably identify the source such that it would be enough for a developer
        /// to identify that source.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// In the case of a source file, for example, this name might return the file path.
        /// In the case of a document from a database, this name might be a unique identifier
        /// for a row.
        /// </para>
        /// </example>
        /// <value>The name of the document source.</value>
        string Name { get; }
    }
}
