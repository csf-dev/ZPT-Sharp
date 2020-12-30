using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Autofixture;

namespace ZptSharp.Mvc
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
