using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ZptSharp.Config;
using ZptSharp.Util;

namespace ZptSharp.IntegrationTests
{
    public static class IntegrationTester
    {
        /// <summary>
        /// Performs an integration test and returns the result.
        /// </summary>
        /// <returns>The integration test result.</returns>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="expectedRenderingPath">Expected rendering path.</param>
        /// <param name="model">The model to render.</param>
        /// <param name="config">Rendering config.</param>
        /// <param name="logLevel">The logging level</param>
        public static async Task<IntegrationTestResult> PerformIntegrationTest(string expectedRenderingPath,
                                                                               object model = null,
                                                                               RenderingConfig config = null,
                                                                               LogLevel logLevel = LogLevel.Debug)
        {
            if (new FileInfo(expectedRenderingPath).Name.Contains(".ignored."))
                NUnit.Framework.Assert.Ignore("This integration test file includes the word 'ignored' in its filename.");

            using(var host = GetZptEnvironment(logLevel))
            {
                var sourceDocument = TestFiles.GetIntegrationTestSourceFile(expectedRenderingPath);
                var expected = await TestFiles.GetString(expectedRenderingPath);

                var result = await TestFiles.GetString(await host.FileRenderer.RenderAsync(sourceDocument, model ?? new object(), config));

                return new IntegrationTestResult
                {
                    Expected = expected,
                    Actual = result,
                    ExpectedRenderingPath = expectedRenderingPath,
                };
            }
        }

        static Hosting.IHostsZptSharp GetZptEnvironment(LogLevel logLevel)
        {
            return ZptSharpHost.GetHost(builder => {
                builder
                    .AddStandardZptExpressions()
                    .AddHapZptDocuments()
                    .AddXmlZptDocuments()
                    .AddZptPythonExpressions();
                
                builder.ServiceRegistrations.Add(serviceCollection => {
                    serviceCollection
                        .AddLogging(b => {
                            b.ClearProviders();
                            b.AddConsole();
                            b.AddConsoleFormatter<ConsoleFormatter, SimpleConsoleFormatterOptions>(o => {
                                o.ColorBehavior = LoggerColorBehavior.Disabled;
                                o.IncludeScopes = true;
                            });
                            b.SetMinimumLevel(logLevel);
                        });
                });
            });
        }

        /// <summary>
        /// Represents the result of a ZPT integration test.
        /// </summary>
        public class IntegrationTestResult
        {
            /// <summary>
            /// Gets or sets the file path to the 'expected rendering' test file.
            /// </summary>
            /// <value>The expected rendering path.</value>
            public string ExpectedRenderingPath { get; set; }

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
