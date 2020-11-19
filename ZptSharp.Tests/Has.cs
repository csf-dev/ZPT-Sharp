using System;
using NUnit.Framework.Constraints;
using ZptSharp.Util;

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
    }
}
