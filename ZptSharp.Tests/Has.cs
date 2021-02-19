using System;
using NUnit.Framework.Constraints;
using ZptSharp.IntegrationTests;

namespace ZptSharp
{
    public class Has : NUnit.Framework.Has
    {
        /// <summary>
        /// Gets a constraint for a <see cref="IntegrationTester.IntegrationTestResult"/>, testing
        /// that its actual &amp; expected renderings match.
        /// </summary>
        /// <value>The NUnit constraint.</value>
        public static Constraint MatchingExpectedAndActualRenderings
            => new MatchingExpectedAndActualRenderingConstraint();

        /// <summary>
        /// Gets a constraint for a <see cref="IntegrationTester.IntegrationTestResult"/>, testing
        /// that its actual &amp; expected renderings match.  This constraint allows for differences in directory
        /// separator characters, though.
        /// </summary>
        /// <value>The NUnit constraint.</value>
        public static Constraint MatchingExpectedAndActualRenderingsExceptDirectorySeparators
            => new MatchingExpectedAndActualRenderingExceptForSourceAnnotationPathsConstraint();
        
    }
}
