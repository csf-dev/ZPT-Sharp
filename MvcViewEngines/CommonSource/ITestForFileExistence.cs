#if MVC5
namespace ZptSharp.Mvc5
#elif MVCCORE
namespace ZptSharp.MvcCore
#endif
{
    /// <summary>
    /// An object which tests for the existence of a file.
    /// </summary>
    public interface ITestForFileExistence
    {
        /// <summary>
        /// Gets a value indicating whether a file exists at the specified path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns><c>true</c> if a file exists at the specified path; <c>false</c> otherwise.</returns>
        bool DoesFileExist(string path);
    }
}