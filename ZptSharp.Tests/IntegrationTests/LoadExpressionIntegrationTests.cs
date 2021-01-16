using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Util;
using Microsoft.Extensions.Logging;

namespace ZptSharp.IntegrationTests
{
    [TestFixture, Parallelizable, Category("Integration")]
    public class LoadExpressionIntegrationTests
    {
        [Test, Description("For the 'expected rendering' file, rendering the corresponding 'source document' file via ZptSharp should produce the same output.")]
        public async Task Each_output_file_should_render_as_expected([ValueSource(nameof(GetExpectedOutputFiles))] string expectedPath)
        {
            var result = await IntegrationTester.PerformIntegrationTest(expectedPath,
                                                                        model: GetModel(),
                                                                        extraBuilderAction: b => b.AddZptLoadExpressions(),
                                                                        logLevel: LogLevel.Debug);
            Assert.That(result, Has.MatchingExpectedAndActualRenderings);
        }

        public static IEnumerable<string> GetExpectedOutputFiles() => TestFiles.GetIntegrationTestExpectedFiles<LoadExpressionIntegrationTests>();

        object GetModel() => new ModelClass(new DirectoryInfo(TestFiles.GetIntegrationTestSourceDirectory<LoadExpressionIntegrationTests>()));

        public class ModelClass
        {
            public TemplateDirectory Documents { get; }

            public IEnumerable<SampleClass> Items { get; }

            public ModelClass(DirectoryInfo sourceDir)
            {
                Documents = new TemplateDirectory(sourceDir);

                Items = new List<SampleClass> {
                    new SampleClass() { Name = "One",   DocumentName = "shared01" },
                    new SampleClass() { Name = "Two",   DocumentName = "shared02" },
                    new SampleClass() { Name = "Three", DocumentName = "shared03" },
                };
            }
        }

        public class SampleClass
        {
            public string Name { get; set; }
            public string DocumentName { get; set; }
        }
    }
}
