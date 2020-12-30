using NUnit.Framework;

namespace ZptSharp
{
    [TestFixture, Parallelizable]
    public class ZptSharpHostTests
    {
        [Test]
        public void GetHost_returns_non_null_object()
        {
            Assert.That(() => ZptSharpHost.GetHost(b => {}), Is.Not.Null);
        }
    }
}