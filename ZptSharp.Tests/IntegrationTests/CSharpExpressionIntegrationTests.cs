using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Util;

namespace ZptSharp.IntegrationTests
{
    [TestFixture, Parallelizable, Category("Integration")]
    public class CSharpExpressionIntegrationTests
    {
        [Test, Description("For the 'expected rendering' file, rendering the corresponding 'source document' file via ZptSharp should produce the same output.")]
        [Ignore("Temporarily ignored")]
        public async Task Each_output_file_should_render_as_expected([ValueSource(nameof(GetExpectedOutputFiles))] string expectedPath)
        {
            var result = await IntegrationTester.PerformIntegrationTest(expectedPath, model: GetModel(), config: GetConfig());
            Assert.That(result, Has.MatchingExpectedAndActualRenderings);
        }

        public static IEnumerable<string> GetExpectedOutputFiles() => TestFiles.GetIntegrationTestExpectedFiles<CSharpExpressionIntegrationTests>();

        object GetModel() => new object();

        RenderingConfig GetConfig()
        {
            var builder = RenderingConfig.CreateBuilder();

            builder.ContextBuilder = (c, s) =>
            {
                c.AddToRootContext("documents", GetSourceTemplateDirectory());
                c.AddToRootContext("tests", new { input = GetSourceTemplateDirectory(), });
                c.AddToRootContext("pnome_macros_page", IntegrationTestDataProvider.GetPnomeTemplatePageMacro(s));
                c.AddToRootContext("acme_macros_page", IntegrationTestDataProvider.GetAcmeTemplatePageMacro(s));
                c.AddToRootContext("options", IntegrationTestDataProvider.GetOptionsObject(s));
            };

            return builder.GetConfig();
        }

        TemplateDirectory GetSourceTemplateDirectory()
            => new TemplateDirectory(TestFiles.GetIntegrationTestSourceDirectory<CSharpExpressionIntegrationTests>());
    }
}
