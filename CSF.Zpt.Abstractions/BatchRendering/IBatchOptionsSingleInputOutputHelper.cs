using System.IO;

namespace CSF.Zpt.BatchRendering
{
    public interface IBatchOptionsSingleInputOutputHelper
    {
        IBatchRenderingOptions ToStream(Stream stream);

        IBatchRenderingOptions ToFile(string path);

        IBatchRenderingOptions ToFile(FileInfo file);
    }
}