using NUnit.Framework;

namespace ZptSharp.Cli
{
    [TestFixture, Parallelizable]
    public class CliArgumentsParserTests
    {
        [Test]
        public void Parse_returns_a_non_null_object()
        {
            Assert.That(() => CliArgumentsParser.Parse(new string[0]), Is.Not.Null);
        }
    }
}