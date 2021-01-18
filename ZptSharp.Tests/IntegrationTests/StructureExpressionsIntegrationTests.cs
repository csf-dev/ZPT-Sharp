using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Util;

namespace ZptSharp.IntegrationTests
{
    [TestFixture, Parallelizable, Category("Integration")]
    public class StructureExpressionsIntegrationTests
    {
        [Test, Description("For the 'expected rendering' file, rendering the corresponding 'source document' file via ZptSharp should produce the same output.")]
        public async Task Each_output_file_should_render_as_expected([ValueSource(nameof(GetExpectedOutputFiles))] string expectedPath)
        {
            var result = await IntegrationTester.PerformIntegrationTest(expectedPath,
                                                                        model: GetModel(),
                                                                        extraBuilderAction: b => b.AddZptStructureExpressions());
            Assert.That(result, Has.MatchingExpectedAndActualRenderings);
        }

        public static IEnumerable<string> GetExpectedOutputFiles() => TestFiles.GetIntegrationTestExpectedFiles<StructureExpressionsIntegrationTests>();

        object GetModel() => new { myMarkup = "<p>Hello world!</p>" };

        TemplateDirectory GetSourceTemplateDirectory()
            => new TemplateDirectory(TestFiles.GetIntegrationTestSourceDirectory<StructureExpressionsIntegrationTests>());
    }
}