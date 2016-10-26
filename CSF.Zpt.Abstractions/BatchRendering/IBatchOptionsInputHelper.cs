using System.Collections.Generic;
using System.IO;

namespace CSF.Zpt.BatchRendering
{
    public interface IBatchOptionsInputHelper
    {
        IBatchOptionsSingleInputOutputHelper FromStream(Stream stream, RenderingMode mode);

        IBatchOptionsSingleInputOutputHelper FromFile(string path, RenderingMode? mode);

        IBatchOptionsSingleInputOutputHelper FromFile(FileInfo file, RenderingMode? mode);

        IBatchOptionsMultipleInputOutputHelper FromFiles(IEnumerable<string> paths, RenderingMode? mode);

        IBatchOptionsMultipleInputOutputHelper FromFiles(IEnumerable<FileInfo> files, RenderingMode? mode);

        IBatchOptionsMultipleInputOutputHelper FromDirectory(string path, RenderingMode? mode, IEnumerable<string> ignoredPaths = null, string searchPattern = null);

        IBatchOptionsMultipleInputOutputHelper FromDirectory(DirectoryInfo directory, RenderingMode? mode, IEnumerable<FileSystemInfo> ignoredPaths = null, string searchPattern = null);
    }
}