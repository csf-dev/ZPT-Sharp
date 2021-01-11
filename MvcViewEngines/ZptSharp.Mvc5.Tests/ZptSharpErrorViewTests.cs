using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;
using ZptSharp.Rendering;

namespace ZptSharp.Mvc5
{
    [TestFixture,Parallelizable]
    public class ZptSharpErrorViewTests
    {
        [Test, AutoMoqData, Description("This integration test verifies that the error view returns a non-null stream")]
        public async Task GetErrorStreamAsync_returns_a_readable_stream(Exception ex)
        {
            var services = GetServiceProvider();
            var sut = new ZptSharpErrorView(services.GetRequiredService<IGetsZptDocumentRendererForFilePath>());

            using (var result = await sut.GetErrorStreamAsync(ex))
            using(var reader = new StreamReader(result))
            {
                Assert.That(() => reader.ReadToEnd(), Is.Not.Null);
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="ServiceProvider"/>, suitable for use when integration-testing ZptSharp.
        /// </summary>
        /// <returns>A service provider.</returns>
        /// <param name="logLevel">The desired log level.</param>
        static ServiceProvider GetServiceProvider(LogLevel logLevel = LogLevel.Debug)
        {
            var builder = new ServiceCollection()
                .AddZptSharp()
                .AddStandardZptExpressions()
                .AddHapZptDocuments();

            builder.ServiceCollection
                .AddLogging(b => {
                    b.ClearProviders();
                    b.AddSimpleConsole(o => {
                        o.ColorBehavior = LoggerColorBehavior.Disabled;
                        o.IncludeScopes = true;
                    });
                    b.SetMinimumLevel(logLevel);
                });

            return builder.ServiceCollection.BuildServiceProvider();
        }

    }
}
