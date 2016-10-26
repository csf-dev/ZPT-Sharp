using System.IO;

namespace CSF.Zpt.BatchRendering
{
    public interface IBatchOptionsMultipleInputOutputHelper
    {
        IBatchRenderingOptions ToDirectory(string path, string extensionOverride = null);

        IBatchRenderingOptions ToDirectory(DirectoryInfo directory, string extensionOverride = null);
    }
}