using System;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace ZptSharp.Util
{
    /// <summary>
    /// Testing helper class used to access external files which are part of the tests.
    /// </summary>
    public static class TestFiles
    {
        /// <summary>
        /// Gets the full path to a file or directory within the "TestFiles" directory.
        /// </summary>
        /// <returns>The full file path.</returns>
        /// <param name="relativePath">The path of the desired file, relative to the TestFiles directory..</param>
        public static string GetPath(string relativePath)
        {
            var testDirectory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            // The test directory is "ProjectDirectory/bin/{Configuration}/{Framework}/" so we need to navigate
            // three levels upwards to get to the project directory.
            var projectDirectory = testDirectory.Parent.Parent.Parent;

            return Path.Combine(projectDirectory.FullName, nameof(TestFiles), relativePath);
        }

        /// <summary>
        /// Gets a string from a specified stream.
        /// </summary>
        /// <returns>The string rendering of the stream.</returns>
        /// <param name="stream">Stream.</param>
        public static async Task<string> GetString(Stream stream)
        {
            using (var reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Gets a string from a specified file path.
        /// </summary>
        /// <returns>The string rendering of the file.</returns>
        /// <param name="filePath">File path.</param>
        public static async Task<string> GetString(string filePath)
        {
            using (var reader = new StreamReader(filePath))
                return await reader.ReadToEndAsync();
        }
    }
}
