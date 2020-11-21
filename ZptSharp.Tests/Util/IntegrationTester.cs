using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZptSharp.Config;

namespace ZptSharp.Util
{
    public static class IntegrationTester
    {
        /// <summary>
        /// Gets an instance of <see cref="ServiceProvider"/>, suitable for use when integration-testing ZptSharp.
        /// </summary>
        /// <returns>A service provider.</returns>
        /// <param name="logLevel">The desired log level.</param>
        public static ServiceProvider GetIntegrationTestServiceProvider(LogLevel logLevel = LogLevel.Debug)
        {
            var services = new ServiceCollection()
                .AddZptSharp()
                .AddAngleSharpZptDocuments()
                .AddHapZptDocuments()
                .AddLogging(b => {
                    b.ClearProviders();
                    b.AddConsole(c => {
                        c.DisableColors = true;
                        c.IncludeScopes = true;
                    });
                    b.SetMinimumLevel(logLevel);
                });

            var provider = services.BuildServiceProvider();
            provider
                .UseHapZptDocuments()
                .UseZptPathExpressions();

            return provider;
        }

        /// <summary>
        /// Performs an integration test and returns the result.
        /// </summary>
        /// <returns>The integration test result.</returns>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="expectedRenderingPath">Expected rendering path.</param>
        /// <param name="model">The model to render.</param>
        /// <param name="config">Rendering config.</param>
        public static async Task<IntegrationTestResult> PerformIntegrationTest(string expectedRenderingPath,
                                                                               IServiceProvider serviceProvider = null,
                                                                               object model = null,
                                                                               RenderingConfig config = null)
        {
            bool createdServiceProvider = (serviceProvider == null);
            IServiceProvider provider = null;

            try
            {
                provider = serviceProvider ?? GetIntegrationTestServiceProvider();

                using (var scope = provider.CreateScope())
                {
                    var sourceDocument = TestFiles.GetIntegrationTestSourceFile(expectedRenderingPath);
                    var expected = await TestFiles.GetString(expectedRenderingPath);

                    var fileRenderer = scope.ServiceProvider.GetRequiredService<IRendersZptFile>();
                    var result = await TestFiles.GetString(await fileRenderer.RenderAsync(sourceDocument, model ?? new object(), config));

                    return new IntegrationTestResult
                    {
                        Expected = expected,
                        Actual = result,
                    };
                }
            }
            finally
            {
                if (createdServiceProvider && provider is ServiceProvider disposable)
                    disposable?.Dispose();
            }
        }

        /// <summary>
        /// Represents the result of a ZPT integration test.
        /// </summary>
        public class IntegrationTestResult
        {
            /// <summary>
            /// Gets the actual rendering of a ZPT document.
            /// </summary>
            /// <value>The actual rendering.</value>
            public string Actual { get; set; }

            /// <summary>
            /// Gets the expected rendering of a ZPT document.
            /// </summary>
            /// <value>The expected rendering.</value>
            public string Expected { get; set; }
        }
    }
}
