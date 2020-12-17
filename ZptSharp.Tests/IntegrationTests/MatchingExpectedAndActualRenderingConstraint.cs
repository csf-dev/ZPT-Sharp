using System;
using System.IO;
using CSF;
using NUnit.Framework.Constraints;

namespace ZptSharp.IntegrationTests
{
    public class MatchingExpectedAndActualRenderingConstraint : Constraint
    {
        string ResultsDirectory => NUnit.Framework.TestContext.CurrentContext.WorkDirectory;

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            if (typeof(TActual) != typeof(IntegrationTester.IntegrationTestResult))
            {
                var ex = new ArgumentException($"The type used with this constraint must be an {typeof(IntegrationTester.IntegrationTestResult)}.", nameof(actual));
                return new ConstraintResult(this, ex, ConstraintStatus.Error);
            }
            if(ReferenceEquals(actual, null))
            {
                return new ConstraintResult(this, actual, ConstraintStatus.Failure);
            }

            return GetResult((IntegrationTester.IntegrationTestResult) (object) actual);
        }

        protected virtual ConstraintResult GetResult(IntegrationTester.IntegrationTestResult result)
        {
            var status = GetConstraintStatus(result);

            if (status == ConstraintStatus.Failure)
                WriteResultsToFile(result);

            return new MatchingExpectedAndActualRenderingConstraintResult(this, result, status);
        }

        ConstraintStatus GetConstraintStatus(IntegrationTester.IntegrationTestResult result)
        {
            if (result.Actual == null || result.Expected == null)
                return ConstraintStatus.Failure;

            return String.Equals(result.Actual, result.Expected) ? ConstraintStatus.Success : ConstraintStatus.Failure;
        }

        void WriteResultsToFile(IntegrationTester.IntegrationTestResult result)
        {
            var expectedFilename = GetExpectedFilename(result.ExpectedRenderingPath);
            var actualFilename = GetActualFilename(result.ExpectedRenderingPath);
            var categoryDirectoryName = new FileInfo(result.ExpectedRenderingPath).Directory.Parent.Name;

            var categoryPath = Path.Combine(ResultsDirectory, categoryDirectoryName);
            var expectedPath = Path.Combine(categoryPath, expectedFilename);
            var actualPath = Path.Combine(categoryPath, actualFilename);

            Directory.CreateDirectory(categoryPath);
            File.WriteAllText(expectedPath, result.Expected);
            File.WriteAllText(actualPath, result.Actual);
        }

        string GetExpectedFilename(string expectedRenderingPath)
            => GetOutputFilename(expectedRenderingPath, "expected");

        string GetActualFilename(string expectedRenderingPath)
            => GetOutputFilename(expectedRenderingPath, "actual");

        string GetOutputFilename(string expectedRenderingPath, string type)
        {
            var expectedRenderingFile = new FileInfo(expectedRenderingPath);

            var builder = FilenameExtensionBuilder.Parse(expectedRenderingFile.Name);
            builder.Extensions.Insert(0, type);
            return builder.ToString();
        }

        public MatchingExpectedAndActualRenderingConstraint()
        {
            Description = $"A non-null {nameof(IntegrationTester.IntegrationTestResult)} with matching actual/expected renderings";
        }

    }
}
