using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Util;

namespace ZptSharp
{
    [TestFixture,Parallelizable]
    public class ZptIntegrationTests
    {
        [Test,
         Description("For every file in the 'expected output' directory of the integration test path, the file should be rendered as-expected."),
         // Explicit("These tests are not yet ready for prime-time because functionality isn't complete")
        ]
        public async Task Each_output_file_should_render_as_expected([ValueSource(nameof(GetExpectedOutputFiles))] string expectedPath)
        {
            var result = await IntegrationTester.PerformIntegrationTest(expectedPath, model: GetModel());
            Assert.That(result, Has.MatchingExpectedAndActualRenderings);
        }

        public static IEnumerable<string> GetExpectedOutputFiles() => TestFiles.GetIntegrationTestExpectedFiles<ZptIntegrationTests>();

        object GetModel()
        {
            return new
            {
                documents = new TemplateDirectory(TestFiles.GetIntegrationTestSourceDirectory<ZptIntegrationTests>())
            };
        }
    }
}
