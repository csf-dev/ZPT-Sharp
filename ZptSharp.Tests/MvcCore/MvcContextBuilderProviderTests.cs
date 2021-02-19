using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.MvcCore
{
    [TestFixture,Parallelizable]
    public class MvcContextBuilderProviderTests
    {
        [Test, AutoMoqData]
        public void GetRootContextBuilder_returns_builder_function_which_creates_variables_for_correct_static_properties_of_ViewContext([Frozen,MockContext] ViewContext context,
                                                                                                                                        MvcContextBuilderProvider sut,
                                                                                                                                        IConfiguresRootContext helper,
                                                                                                                                        IServiceProvider serviceProvider)
        {
            var action = sut.GetRootContextBuilder();
            action(helper, serviceProvider);

            Mock.Get(helper).Verify(x => x.AddToRootContext("ViewContext", context), Times.Once, $"{nameof(ViewContext)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("ViewData", context.ViewData), Times.Once, $"{nameof(ViewContext.ViewData)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("TempData", context.TempData), Times.Once, $"{nameof(ViewContext.TempData)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("Request", context.HttpContext.Request), Times.Once, $"{nameof(HttpContext.Request)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("Response", context.HttpContext.Response), Times.Once, $"{nameof(HttpContext.Response)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("RouteData", context.RouteData), Times.Once, $"{nameof(ViewContext.RouteData)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("Model", context.ViewData.Model), Times.Once, $"{nameof(ViewDataDictionary.Model)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("request", context.HttpContext.Request), Times.Once, $"{nameof(HttpContext.Request)} set correctly (in lowercase)");
        }

        [Test, AutoMoqData]
        public void GetRootContextBuilder_returns_builder_function_which_creates_correct_views_directory([Frozen, MockContext] ViewContext context,
                                                                                                         [Frozen] string viewsPath,
                                                                                                         MvcContextBuilderProvider sut,
                                                                                                         IConfiguresRootContext helper,
                                                                                                         IServiceProvider serviceProvider)
        {
            var action = sut.GetRootContextBuilder();
            action(helper, serviceProvider);

            Mock.Get(helper)
                .Verify(x => x.AddToRootContext("Views", It.Is<TemplateDirectory>(d => d.Path == viewsPath)), Times.Once);
        }
    }
}
