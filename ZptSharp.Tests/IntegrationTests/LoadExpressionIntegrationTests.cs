using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Util;

namespace ZptSharp.IntegrationTests
{
    [TestFixture, Parallelizable, Category("Integration")]
    public class LoadExpressionIntegrationTests
    {
        [Test, Description("For the 'expected rendering' file, rendering the corresponding 'source document' file via ZptSharp should produce the same output.")]
        [Ignore("Ignored whilst this functionality is re-implemented")]
        public async Task Each_output_file_should_render_as_expected([ValueSource(nameof(GetExpectedOutputFiles))] string expectedPath)
        {
            var result = await IntegrationTester.PerformIntegrationTest(expectedPath, model: GetModel());
            Assert.That(result, Has.MatchingExpectedAndActualRenderings);
        }

        public static IEnumerable<string> GetExpectedOutputFiles() => TestFiles.GetIntegrationTestExpectedFiles<LoadExpressionIntegrationTests>();

        object GetModel()
        {
            throw new NotImplementedException("This needs to be implemented using the notes below");

            // This is the old code for this test

            //public class ModelClass
            //{
            //    public TemplateDirectory Documents { get; private set; }

            //    public IEnumerable<SampleClass> Items { get; private set; }

            //    public ModelClass(DirectoryInfo sourceDir)
            //    {
            //        Documents = new TemplateDirectory(sourceDir);
            //        Items = new List<SampleClass>() {
            //  new SampleClass() { Name = "One",   DocumentName = "shared01" },
            //  new SampleClass() { Name = "Two",   DocumentName = "shared02" },
            //  new SampleClass() { Name = "Three", DocumentName = "shared03" },
            //};
            //    }
            //}

            //public class SampleClass
            //{
            //    public string Name { get; set; }
            //    public string DocumentName { get; set; }
            //}
        }
    }
}
