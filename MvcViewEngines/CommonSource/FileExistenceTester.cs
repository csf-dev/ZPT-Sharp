#if MVC5
namespace ZptSharp.Mvc5
#elif MVCCORE
namespace ZptSharp.MvcCore
#endif
{
    /// <summary>
    /// Default implementation of <see cref="ITestForFileExistence" /> which simply
    /// wraps a call to <see cref="System.IO.File.Exists(string)" />.
    /// </summary>
    public class FileExistenceTester : ITestForFileExistence
    {
        /// <summary>
        /// Gets a value indicating whether a file exists at the specified path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns><c>true</c> if a file exists at the specified path; <c>false</c> otherwise.</returns>
        public bool DoesFileExist(string path) => System.IO.File.Exists(path);
    }
}