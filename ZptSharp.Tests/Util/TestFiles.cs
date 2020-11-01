using System;
using NUnit.Framework;
using System.IO;

namespace ZptSharp.Util
{
    /// <summary>
    /// Testing helper class used to access external files which are part of the tests.
    /// </summary>
    public static class TestFiles
    {
        /// <summary>
        /// Gets the full path to a file within the "TestFiles" directory.
        /// </summary>
        /// <returns>The full file path.</returns>
        /// <param name="relativePath">The path of the desired file, relative to the TestFiles directory..</param>
        public static string GetFilePath(string relativePath)
        {
            var testDirectory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            // The test directory is "ProjectDirectory/bin/Configuration/Framework/" so we need to navigate
            // three levels upwards to get to the project directory.
            var projectDirectory = testDirectory.Parent.Parent.Parent;

            return Path.Combine(projectDirectory.FullName, nameof(TestFiles), relativePath);
        }
    }
}
