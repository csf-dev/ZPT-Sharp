using System;
using NUnit.Framework.Constraints;

namespace ZptSharp.IntegrationTests
{
    public class MatchingExpectedAndActualRenderingConstraint : Constraint
    {
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

        protected virtual ConstraintResult GetResult(IntegrationTester.IntegrationTestResult actual)
        {
            var status = (actual.Actual != null
                       && actual.Expected != null
                       && String.Equals(actual.Actual, actual.Expected))
                ? ConstraintStatus.Success
                : ConstraintStatus.Failure;

            return new MatchingExpectedAndActualRenderingConstraintResult(this, actual, status);
        }

        public MatchingExpectedAndActualRenderingConstraint()
        {
            Description = $"A non-null {nameof(IntegrationTester.IntegrationTestResult)} with matching actual/expected renderings";
        }

    }
}
