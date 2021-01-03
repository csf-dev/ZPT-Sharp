using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Mvc5.Autofixture;

namespace ZptSharp.Mvc5
{
    [TestFixture, Parallelizable]
    public class ZptSharpViewEngineTests
    {
        [Test,AutoMoqData]
        public void FindView_returns_found_result_and_a_view_if_it_exists([MockContext] ControllerContext controllerContext,
                                                                          string viewName,
                                                                          string controllerName,
                                                                          IFindsView viewFinder,
                                                                          string viewPath,
                                                                          string masterName,
                                                                          bool useCache)
        {
            var sut = GetSut(viewFinder: viewFinder);
            controllerContext.RouteData.Values["controller"] = controllerName;
            Mock.Get(viewFinder)
                .Setup(x => x.FindView(controllerName, viewName, It.IsAny<string[]>()))
                .Returns(() => new FindViewResult(viewPath));
            
            Assert.That(() => sut.FindView(controllerContext, viewName, masterName, useCache)?.View,
                        Is.InstanceOf<ZptSharpView>());
        }

        [Test,AutoMoqData]
        public void FindPartialView_returns_found_result_and_a_view_if_it_exists([MockContext] ControllerContext controllerContext,
                                                                                 string viewName,
                                                                                 string controllerName,
                                                                                 IFindsView viewFinder,
                                                                                 string viewPath,
                                                                                 bool useCache)
        {
            var sut = GetSut(viewFinder: viewFinder);
            controllerContext.RouteData.Values["controller"] = controllerName;
            Mock.Get(viewFinder)
                .Setup(x => x.FindView(controllerName, viewName, It.IsAny<string[]>()))
                .Returns(() => new FindViewResult(viewPath));
            
            Assert.That(() => sut.FindPartialView(controllerContext, viewName, useCache)?.View,
                        Is.InstanceOf<ZptSharpView>());
        }

        [Test,AutoMoqData]
        public void FindPartialView_returns_not_found_result_and_a_view_if_it_does_not_exist([MockContext] ControllerContext controllerContext,
                                                                                             string viewName,
                                                                                             string controllerName,
                                                                                             IFindsView viewFinder,
                                                                                             string viewPath,
                                                                                             bool useCache,
                                                                                             string[] locations)
        {
            var sut = GetSut(viewFinder: viewFinder);
            controllerContext.RouteData.Values["controller"] = controllerName;
            Mock.Get(viewFinder)
                .Setup(x => x.FindView(controllerName, viewName, It.IsAny<string[]>()))
                .Returns(() => new FindViewResult(locations));
            
            Assert.That(() => sut.FindPartialView(controllerContext, viewName, useCache),
                        Has.Property(nameof(ViewEngineResult.View)).Null
                            .And.Property(nameof(ViewEngineResult.SearchedLocations)).EqualTo(locations));
        }

        ZptSharpViewEngine GetSut(string[] viewLocationFormats = null,
                                  string viewsPath = "~/Views",
                                  RenderingConfig config = null,
                                  IFindsView viewFinder = null)
        {
            return new ZptSharpViewEngine(b => {}, viewLocationFormats, viewsPath, config, viewFinder);
        }
    }
}