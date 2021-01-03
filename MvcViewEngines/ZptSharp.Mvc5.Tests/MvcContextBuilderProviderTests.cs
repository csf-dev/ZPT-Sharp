using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Mvc5.Autofixture;

namespace ZptSharp.Mvc5
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
            Mock.Get(helper).Verify(x => x.AddToRootContext("Cache", context.HttpContext.Cache), Times.Once, $"{nameof(HttpContextBase.Cache)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("Request", context.HttpContext.Request), Times.Once, $"{nameof(HttpContextBase.Request)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("Response", context.HttpContext.Response), Times.Once, $"{nameof(HttpContextBase.Response)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("RouteData", context.RouteData), Times.Once, $"{nameof(ViewContext.RouteData)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("Server", context.HttpContext.Server), Times.Once, $"{nameof(HttpContextBase.Server)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("Session", context.HttpContext.Session), Times.Once, $"{nameof(HttpContextBase.Session)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("Model", context.ViewData.Model), Times.Once, $"{nameof(ViewDataDictionary.Model)} set correctly");
            Mock.Get(helper).Verify(x => x.AddToRootContext("request", context.HttpContext.Request), Times.Once, $"{nameof(HttpContextBase.Request)} set correctly (in lowercase)");
        }

        [Test, AutoMoqData]
        public void GetRootContextBuilder_returns_builder_function_which_creates_correct_application_dictionary([Frozen, MockContext] ViewContext context,
                                                                                                                MvcContextBuilderProvider sut,
                                                                                                                IConfiguresRootContext helper,
                                                                                                                IServiceProvider serviceProvider,
                                                                                                                IDictionary<string,object> appValues)
        {
            Mock.Get(context.HttpContext.Application).SetupGet(x => x.AllKeys).Returns(appValues.Keys.ToArray());
            foreach (var kvp in appValues)
                Mock.Get(context.HttpContext.Application).SetupGet(x => x[kvp.Key]).Returns(kvp.Value);

            var action = sut.GetRootContextBuilder();
            action(helper, serviceProvider);

            Mock.Get(helper)
                .Verify(x => x.AddToRootContext("Application", It.Is<IDictionary<string,object>>(d => d.SequenceEqual(appValues))), Times.Once);
        }

        [Test, AutoMoqData]
        public void GetRootContextBuilder_returns_builder_function_which_creates_correct_views_directory([Frozen, MockContext] ViewContext context,
                                                                                                         [Frozen] string viewsPath,
                                                                                                         MvcContextBuilderProvider sut,
                                                                                                         IConfiguresRootContext helper,
                                                                                                         IServiceProvider serviceProvider,
                                                                                                         string mappedViewsPath)
        {
            Mock.Get(context.HttpContext.Server)
                .Setup(x => x.MapPath(viewsPath)).Returns(mappedViewsPath);

            var action = sut.GetRootContextBuilder();
            action(helper, serviceProvider);

            Mock.Get(helper)
                .Verify(x => x.AddToRootContext("Views", It.Is<TemplateDirectory>(d => d.Path == mappedViewsPath)), Times.Once);
        }
    }
}
