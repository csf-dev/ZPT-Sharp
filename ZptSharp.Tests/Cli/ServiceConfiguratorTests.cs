using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Hosting;

namespace ZptSharp.Cli
{
    [TestFixture, Parallelizable]
    public class ServiceConfiguratorTests
    {
        [Test,AutoMoqData]
        public void ConfigureServices_selects_anglesharp_if_enabled([Frozen] IBuildsHostingEnvironment hostingBuilder,
                                                                    CliArguments args,
                                                                    ServiceConfigurator sut)
        {
            args.UseAngleSharp = true;
            sut.ConfigureServices(args);
            Assert.That(hostingBuilder.ServiceRegistry.DocumentProviderTypes, Has.One.EqualTo(typeof(AngleSharpDocumentProvider)));
        }

        [Test,AutoMoqData]
        public void ConfigureServices_selects_HAP_if_anglesharp_not_enabled([Frozen] IBuildsHostingEnvironment hostingBuilder,
                                                                            CliArguments args,
                                                                            ServiceConfigurator sut)
        {
            args.UseAngleSharp = false;
            sut.ConfigureServices(args);
            Assert.That(hostingBuilder.ServiceRegistry.DocumentProviderTypes, Has.One.EqualTo(typeof(HapDocumentProvider)));
        }
    }
}