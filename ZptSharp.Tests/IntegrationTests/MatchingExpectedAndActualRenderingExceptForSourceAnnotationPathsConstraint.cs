using System;
using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;
using ZptSharp.SourceAnnotation;

namespace ZptSharp.IntegrationTests
{
    public class MatchingExpectedAndActualRenderingExceptForSourceAnnotationPathsConstraint : MatchingExpectedAndActualRenderingConstraint
    {
        static readonly string sourceAnnotationPattern = $@"{AnnotationProvider.GetDivider()}
.*
{AnnotationProvider.GetDivider()}";
        static readonly Regex sourceAnnotation = new Regex(sourceAnnotationPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Pre-processes the <paramref name="actual" /> object, replacing the expected
        /// directory separator characters with the correct ones for the current environment.
        /// </summary>
        protected override ConstraintResult GetResult(IntegrationTester.IntegrationTestResult actual)
        {
            actual.Expected = sourceAnnotation.Replace(actual.Expected, match => {
                var value = match.Value;
                return value.Replace('/', System.IO.Path.DirectorySeparatorChar);
            });
            return base.GetResult(actual);
        }
    }
}