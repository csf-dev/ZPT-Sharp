using NUnit.Framework;

namespace ZptSharp.Cli
{
    [TestFixture, Parallelizable]
    public class ProgramTests
    {
        [Test]
        public void GetHostBuilder_does_not_throw()
        {
            Assert.That(() => Program.GetHostBuilder(new string[0]), Throws.Nothing);
        }

        [Test]
        public void GetHostBuilder_returns_a_host_builder_which_may_be_used_to_build_the_app()
        {
            Assert.That(() => Program.GetHostBuilder(new string[0]).Build(), Throws.Nothing);
        }
    }
}