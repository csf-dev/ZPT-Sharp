using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using ZptSharp.Util;

namespace ZptSharp
{
    [TestFixture,Parallelizable]
    public class ZptIntegrationTests
    {
        const string
            rootDirectory = nameof(ZptIntegrationTests),
            expectedSubdirectory = "ExpectedOutputs",
            sourceSubdirectory = "SourceDocuments";

        ServiceProvider serviceProvider;

        [Test,
         Description("For every file in the 'expected output' directory of the integration test path, the file should be rendered as-expected."),
         Explicit("These tests are not yet ready for prime-time because functionality isn't complete")
        ]
        public async Task Each_output_file_should_render_as_expected([ValueSource(nameof(GetExpectedOutputFiles))] string expectedPath)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var sourceDocument = GetSourceDocumentPath(expectedPath);
                var expected = await TestFiles.GetString(expectedPath);

                var fileRenderer = scope.ServiceProvider.GetRequiredService<IRendersZptFile>();
                var result = await TestFiles.GetString(await fileRenderer.RenderAsync(sourceDocument, GetModel()));

                Assert.That(result,
                            Is.EqualTo(expected),
                            () => $@"
Expected
========
{expected}

Actual
======
{result}");
            }
        }

        public static IEnumerable<string> GetExpectedOutputFiles()
        {
            var expectedDocsDirectory = TestFiles.GetPath(Path.Combine(rootDirectory, expectedSubdirectory));
            return Directory.GetFiles(expectedDocsDirectory);
        }

        [OneTimeSetUp]
        public void InitialSetup()
        {
            serviceProvider = GetServiceProvider();
        }

        [OneTimeTearDown]
        public void FinalTeardown()
        {
            serviceProvider?.Dispose();
        }

        ServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddZptSharp();
            services.AddAngleSharpZptDocuments();
            services.AddHapZptDocuments();

            AddLogging(services);

            var provider = services.BuildServiceProvider();
            provider.UseHapZptDocuments();
            provider.UseZptPathExpressions();
            return provider;
        }

        void AddLogging(ServiceCollection services)
        {
            services.AddLogging(b => {
                b.ClearProviders();
                b.AddConsole(c => c.DisableColors = true);
                b.SetMinimumLevel(LogLevel.Information);
            });

            services.AddTransient(s => s.GetRequiredService<ILoggerFactory>().CreateLogger("ZptSharp-UnitTests"));
        }

        object GetModel()
        {
            return new
            {
                documents = new TemplateDirectory(GetSourceDocumentDirectory())
            };
        }

        string GetSourceDocumentDirectory() => TestFiles.GetPath(Path.Combine(rootDirectory, sourceSubdirectory));

        string GetSourceDocumentPath(string expectedFilename)
        {
            var expectedFile = new FileInfo(expectedFilename);
            return Path.Combine(GetSourceDocumentDirectory(), expectedFile.Name);
        }
    }
}
