using NUnit.Framework.Constraints;

namespace ZptSharp.IntegrationTests
{
    public class MatchingExpectedAndActualRenderingConstraintResult : ConstraintResult
    {
        IntegrationTester.IntegrationTestResult Result
            => (IntegrationTester.IntegrationTestResult) ActualValue;

        string Actual => Result.Actual ?? "<null>";

        string Expected => Result.Expected ?? "<null>";

        public override void WriteMessageTo(MessageWriter writer)
        {
            writer.WriteLine($@"Actual rendering does not match expected rendering.
========
Expected
========
{Expected}
========
 Actual
========
{Actual}
========");
        }

        public MatchingExpectedAndActualRenderingConstraintResult(IConstraint constraint,
                                                                    IntegrationTester.IntegrationTestResult actual,
                                                                    ConstraintStatus status)
            : base(constraint, actual, status) { }
    }
}