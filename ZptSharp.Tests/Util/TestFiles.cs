using System;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Util
{
    /// <summary>
    /// Testing helper class used to access external files which are part of the tests.
    /// </summary>
    public static class TestFiles
    {
        const string
            integrationTestExpectedDir = "ExpectedOutputs",
            integrationTestSourceDir = "SourceDocuments";

        static readonly string[] expectedRenderingExtensions =
        {
            ".html",
            ".htm",
            ".xml",
            ".xhtml",
        };

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
        /// Gets the full path to a file or directory within the "TestFiles" directory.
        /// </summary>
        /// <returns>The full file path.</returns>
        /// <param name="relativePathParts">The parts of the path to the desired file, relative to the TestFiles directory..</param>
        public static string GetPath(params string[] relativePathParts)
            => GetPath(String.Join(Path.DirectorySeparatorChar, relativePathParts));

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

        /// <summary>
        /// Gets a collection of all of the files in an integration test "expected renderings"
        /// directory.
        /// </summary>
        /// <returns>The expected files.</returns>
        /// <param name="categoryName">The name of a category of integration tests.</param>
        public static IEnumerable<string> GetIntegrationTestExpectedFiles(string categoryName)
            => GetExpectedRenderingsInDirectory(categoryName, integrationTestExpectedDir);

        /// <summary>
        /// Gets a collection of all of the files in an integration test "expected renderings"
        /// directory.
        /// </summary>
        /// <returns>The expected files.</returns>
        /// <typeparam name="T">The test fixture (test category) type.</typeparam>
        public static IEnumerable<string> GetIntegrationTestExpectedFiles<T>()
            => GetIntegrationTestExpectedFiles(typeof(T).Name);

        /// <summary>
        /// Gets the source directory path for integration test files.
        /// directory.
        /// </summary>
        /// <returns>The source renderings directory.</returns>
        /// <param name="categoryName">The name of a category of integration tests.</param>
        public static string GetIntegrationTestSourceDirectory(string categoryName)
            => GetPath(Path.Combine(categoryName, integrationTestSourceDir));

        /// <summary>
        /// Gets the source directory path for integration test files.
        /// directory.
        /// </summary>
        /// <returns>The source renderings directory.</returns>
        /// <typeparam name="T">The test fixture (test category) type.</typeparam>
        public static string GetIntegrationTestSourceDirectory<T>()
            => GetIntegrationTestSourceDirectory(typeof(T).Name);

        /// <summary>
        /// Gets the path to an integration test "source rendering" file, based upon the path to
        /// a corresponding "expected rendering" file.
        /// </summary>
        /// <returns>The integration test source file.</returns>
        /// <param name="expectedRenderingPath">The path to an expected rendering.</param>
        public static string GetIntegrationTestSourceFile(string expectedRenderingPath)
        {
            var expectedRenderingFile = new FileInfo(expectedRenderingPath);
            var baseDirectory = expectedRenderingFile.Directory.Parent;
            return Path.Combine(baseDirectory.FullName, integrationTestSourceDir, expectedRenderingFile.Name);
        }

        /// <summary>
        /// Gets a collection of all of the files in the directory which represent expected test renderings.
        /// The directory is identified by zero or more 'path parts' from the root of the TestFiles path.
        /// </summary>
        /// <returns>The files in the specified directory.</returns>
        /// <param name="pathParts">Zero or more path parts.</param>
        public static IEnumerable<string> GetExpectedRenderingsInDirectory(params string[] pathParts)
        {
            // Skip files which don't match any of the expected file extensions for
            // "expected rendering" files.  Avoids ancilliary files like ".directory" etc.
            return Directory.GetFiles(GetPath(Path.Combine(pathParts)))
                .Where(HasExpectedRenderingExtension)
                .ToList();
        }

        static bool HasExpectedRenderingExtension(string filePath)
            => expectedRenderingExtensions.Any(filePath.EndsWith);


        /// <summary>
        /// Gets a collection of all of the files in the directory, identified by
        /// zero or more 'path parts' from the root of the TestFiles path.
        /// </summary>
        /// <returns>The files in the specified directory.</returns>
        /// <param name="pathParts">Zero or more path parts.</param>
        public static IEnumerable<string> GetFilesInDirectory(params string[] pathParts)
            => Directory.GetFiles(GetPath(Path.Combine(pathParts)));
    }
}
