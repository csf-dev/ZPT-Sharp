using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ZptSharp.Mvc
{
    [TestFixture,Parallelizable]
    public class ZptErrorViewTests
    {
        [Test, AutoMoqData, Description("This integration test verifies that the error view returns a non-null stream")]
        public async Task GetErrorStream_returns_a_readable_stream(Exception ex)
        {
            var services = GetServiceProvider();
            var sut = new ZptErrorView(ex, services);

            using (var result = await sut.GetErrorStream())
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
            var services = new ServiceCollection()
                .AddZptSharp()
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
                .UseStandardZptExpressions()
                ;

            return provider;
        }

    }
}
