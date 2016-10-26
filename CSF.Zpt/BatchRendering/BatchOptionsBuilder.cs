using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Default implementation of <see cref="IBatchOptionsInputHelper"/>.
  /// </summary>
  internal class BatchOptionsBuilder : IBatchOptionsInputHelper, IBatchOptionsSingleInputOutputHelper, IBatchOptionsMultipleInputOutputHelper
  {
    #region fields

    private Stream _inputStream = null, _outputStream = null;
    private IEnumerable<FileSystemInfo> _inputPaths = null;
    private IEnumerable<DirectoryInfo> _ignoredPaths = null;
    private FileSystemInfo _outputPath = null;
    private string _inputSearchPattern = null, _outputExtensionOverride = null;
    private RenderingMode? _renderingMode = null;

    #endregion

    #region input helper

    /// <summary>
    /// Specifies that the batch options are to be built from a <c>System.IO.Stream</c>.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="stream">The stream.</param>
    /// <param name="mode">The rendering mode.</param>
    public IBatchOptionsSingleInputOutputHelper FromStream(Stream stream, RenderingMode mode)
    {
      if(stream == null)
      {
        throw new ArgumentNullException(nameof(stream));
      }
      mode.RequireDefinedValue(nameof(mode));

      _inputStream = stream;
      _renderingMode = mode;

      return this;
    }

    /// <summary>
    /// Specifies that the batch options are to be built from a single input file.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="path">The file path.</param>
    /// <param name="mode">An optional rendering mode.</param>
    public IBatchOptionsSingleInputOutputHelper FromFile(string path, RenderingMode? mode)
    {
      if(path == null)
      {
        throw new ArgumentNullException(nameof(path));
      }

      var file = new FileInfo(path);
      return FromFile(file, mode);
    }

    /// <summary>
    /// Specifies that the batch options are to be built from a single input file.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="file">The file.</param>
    /// <param name="mode">An optional rendering mode.</param>
    public IBatchOptionsSingleInputOutputHelper FromFile(FileInfo file, RenderingMode? mode)
    {
      if(file == null)
      {
        throw new ArgumentNullException(nameof(file));
      }
      if(mode.HasValue)
      {
        mode.Value.RequireDefinedValue(nameof(mode));
      }

      _inputPaths = new [] { file };
      _renderingMode = mode;

      return this;
    }

    /// <summary>
    /// Specifies that the batch options are to be built from a collection of input files.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="paths">The file paths.</param>
    /// <param name="mode">An optional rendering mode.</param>
    public IBatchOptionsMultipleInputOutputHelper FromFiles(IEnumerable<string> paths, RenderingMode? mode)
    {
      if(paths == null)
      {
        throw new ArgumentNullException(nameof(paths));
      }

      var files = paths.Select(x => new FileInfo(x));
      return FromFiles(files, mode);
    }

    /// <summary>
    /// Specifies that the batch options are to be built from a collection of input files.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="files">The files.</param>
    /// <param name="mode">An optional rendering mode.</param>
    public IBatchOptionsMultipleInputOutputHelper FromFiles(IEnumerable<FileInfo> files, RenderingMode? mode)
    {
      if(files == null)
      {
        throw new ArgumentNullException(nameof(files));
      }
      if(mode.HasValue)
      {
        mode.Value.RequireDefinedValue(nameof(mode));
      }

      _inputPaths = files;
      _renderingMode = mode;

      return this;
    }

    /// <summary>
    /// Specifies that the batch options are to be built from the files found within a directory path.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="path">The directory path.</param>
    /// <param name="mode">An optional rendering mode.</param>
    /// <param name="ignoredPaths">An optional collection of ignored paths.</param>
    /// <param name="searchPattern">An optional search pattern for files within the directory.</param>
    public IBatchOptionsMultipleInputOutputHelper FromDirectory(string path, RenderingMode? mode, IEnumerable<string> ignoredPaths = null, string searchPattern = null)
    {
      if(path == null)
      {
        throw new ArgumentNullException(nameof(path));
      }

      var directory = new DirectoryInfo(path);

      IEnumerable<DirectoryInfo> ignoredDirectories;
      if(ignoredPaths != null)
      {
        ignoredDirectories = ignoredPaths.Select(x => new DirectoryInfo(x));
      }
      else
      {
        ignoredDirectories = null;
      }

      return FromDirectory(directory, mode, ignoredDirectories, searchPattern);
    }

    /// <summary>
    /// Specifies that the batch options are to be built from the files found within a directory path.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="directory">The directory.</param>
    /// <param name="mode">An optional rendering mode.</param>
    /// <param name="ignoredPaths">An optional collection of ignored paths.</param>
    /// <param name="searchPattern">An optional search pattern for files within the directory.</param>
    public IBatchOptionsMultipleInputOutputHelper FromDirectory(DirectoryInfo directory, RenderingMode? mode, IEnumerable<DirectoryInfo> ignoredPaths = null, string searchPattern = null)
    {
      if(directory == null)
      {
        throw new ArgumentNullException(nameof(directory));
      }
      if(mode.HasValue)
      {
        mode.Value.RequireDefinedValue(nameof(mode));
      }

      _inputPaths = new [] { directory };
      _renderingMode = mode;
      _ignoredPaths = ignoredPaths;
      _inputSearchPattern = searchPattern;

      return this;
    }

    #endregion

    #region single-input output helper

    /// <summary>
    /// Indicates that the output is to a <c>System.IO.Stream</c>.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="stream">The stream.</param>
    public IBatchRenderingOptions ToStream(Stream stream)
    {
      if(stream == null)
      {
        throw new ArgumentNullException(nameof(stream));
      }

      _outputStream = stream;
      return BuildOptions();
    }

    /// <summary>
    /// Indicates that the output is to a single file.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="path">The file path.</param>
    public IBatchRenderingOptions ToFile(string path)
    {
      if(path == null)
      {
        throw new ArgumentNullException(nameof(path));
      }

      var file = new FileInfo(path);
      return ToFile(file);
    }

    /// <summary>
    /// Indicates that the output is to a single file.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="file">The file.</param>
    public IBatchRenderingOptions ToFile(FileInfo file)
    {
      if(file == null)
      {
        throw new ArgumentNullException(nameof(file));
      }

      _outputPath = file;
      return BuildOptions();
    }

    #endregion

    #region multiple-input output helper

    /// <summary>
    /// Indicates that the output is to a filesystem directory.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="path">The directory path.</param>
    /// <param name="extensionOverride">An optional override for the file extension of rendered documents.</param>
    public IBatchRenderingOptions ToDirectory(string path, string extensionOverride = null)
    {
      if(path == null)
      {
        throw new ArgumentNullException(nameof(path));
      }

      var directory = new DirectoryInfo(path);
      return ToDirectory(directory);
    }

    /// <summary>
    /// Indicates that the output is to a filesystem directory.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="directory">The directory.</param>
    /// <param name="extensionOverride">An optional override for the file extension of rendered documents.</param>
    public IBatchRenderingOptions ToDirectory(DirectoryInfo directory, string extensionOverride = null)
    {
      if(directory == null)
      {
        throw new ArgumentNullException(nameof(directory));
      }

      _outputPath = directory;
      _outputExtensionOverride = extensionOverride;
      return BuildOptions();
    }

    #endregion

    #region methods

    /// <summary>
    /// Builds and returns the options from the state of the current instance.
    /// </summary>
    /// <returns>The built options.</returns>
    private IBatchRenderingOptions BuildOptions()
    {
      return new BatchRenderingOptions(inputStream: _inputStream,
                                       outputStream: _outputStream,
                                       inputPaths: _inputPaths,
                                       outputPath: _outputPath,
                                       inputSearchPattern: _inputSearchPattern,
                                       outputExtensionOverride: _outputExtensionOverride,
                                       ignoredPaths: _ignoredPaths,
                                       renderingMode: _renderingMode);
    }

    #endregion
  }
}

