using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Cli
{
    [TestFixture, Parallelizable]
    public class ServiceConfiguratorTests
    {
        [Test,AutoMoqData]
        public void ConfigureServices_selects_anglesharp_if_enabled([Frozen] IServiceProvider serviceProvider,
                                                                    CliArguments args,
                                                                    ServiceConfigurator sut,
                                                                    IRegistersDocumentReaderWriter registry,
                                                                    AngleSharpDocumentProvider anglesharp,
                                                                    XmlDocumentProvider xml,
                                                                    IRegistersExpressionEvaluator evaluatorRegistry)
        {
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(IRegistersExpressionEvaluator))).Returns(evaluatorRegistry);
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(IRegistersDocumentReaderWriter))).Returns(registry);
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(AngleSharpDocumentProvider))).Returns(anglesharp);
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(XmlDocumentProvider))).Returns(xml);
            args.UseAngleSharp = true;

            sut.ConfigureServices(args);

            Mock.Get(registry)
                .Verify(x => x.RegisterDocumentReaderWriter(anglesharp), Times.Once);
        }

        [Test,AutoMoqData]
        public void ConfigureServices_selects_HAP_if_anglesharp_not_enabled([Frozen] IServiceProvider serviceProvider,
                                                                            CliArguments args,
                                                                            ServiceConfigurator sut,
                                                                            IRegistersDocumentReaderWriter registry,
                                                                            HapDocumentProvider hap,
                                                                            XmlDocumentProvider xml,
                                                                            IRegistersExpressionEvaluator evaluatorRegistry)
        {
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(IRegistersExpressionEvaluator))).Returns(evaluatorRegistry);
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(IRegistersDocumentReaderWriter))).Returns(registry);
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(HapDocumentProvider))).Returns(hap);
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(XmlDocumentProvider))).Returns(xml);
            args.UseAngleSharp = false;

            sut.ConfigureServices(args);

            Mock.Get(registry)
                .Verify(x => x.RegisterDocumentReaderWriter(hap), Times.Once);
        }
    }
}