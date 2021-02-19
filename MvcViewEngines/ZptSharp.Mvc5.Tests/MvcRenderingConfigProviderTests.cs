using System;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Mvc5.Autofixture;

namespace ZptSharp.Mvc5
{
    [TestFixture,Parallelizable]
    public class MvcRenderingConfigProviderTests
    {
        [Test, AutoMoqData]
        public void GetMvcRenderingConfig_returns_config_with_context_builder(MvcRenderingConfigProvider sut,
                                                                              [MockContext] ViewContext viewContext,
                                                                              string viewsPath,
                                                                              IConfiguresRootContext contextHelper,
                                                                              IServiceProvider serviceProvider,
                                                                              RenderingConfig config)
        {
            var result = sut.GetMvcRenderingConfig(config, viewContext, viewsPath);
            result.ContextBuilder(contextHelper, serviceProvider);
            Mock.Get(contextHelper).Verify(x => x.AddToRootContext(It.IsAny<string>(), It.IsAny<object>()), Times.AtLeastOnce);
        }
    }
}
