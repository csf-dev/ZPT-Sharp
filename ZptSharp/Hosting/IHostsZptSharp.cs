using System;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// An object which is capable of hosting a ZptSharp environment, using its own
    /// dependency injection, separate from those of the containing application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this object when you wish to make use of the the entry-points to ZptSharp,
    /// without depending upon ZptSharp being wired up into your applications's dependency
    /// injection.
    /// </para>
    /// </remarks>
    public interface IHostsZptSharp : IDisposable
    {
        /// <summary>
        /// Gets a service which renders ZPT template files from disk paths.
        /// </summary>
        /// <value>The file renderer.</value>
        IRendersZptFile FileRenderer { get; }

        /// <summary>
        /// Gets a service which renders ZPT templates from streams (whether they came from a file or not).
        /// </summary>
        /// <value>The document renderer.</value>
        IRendersZptDocument DocumentRenderer { get; }
    }
}