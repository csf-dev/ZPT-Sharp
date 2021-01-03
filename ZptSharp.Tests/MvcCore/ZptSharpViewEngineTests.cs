using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.MvcCore
{
    [TestFixture, Parallelizable]
    public class ZptSharpViewEngineTests
    {
        [Test,AutoMoqData]
        public void FindView_throws_if_context_has_no_controller_name(ZptSharpViewEngine sut,
                                                                      [MockContext] ActionContext context,
                                                                      string viewName,
                                                                      bool isMainPage)
        {
            context.ActionDescriptor.RouteValues.Clear();
            Assert.That(() => sut.FindView(context, viewName, isMainPage), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void FindView_returns_found_result_when_view_finder_finds_view([Frozen] IFindsView viewFinder,
                                                                              ZptSharpViewEngine sut,
                                                                              [MockContext] ActionContext context,
                                                                              string viewName,
                                                                              string controllerName,
                                                                              bool isMainPage,
                                                                              string path)
        {
            context.ActionDescriptor.RouteValues.Clear();
            context.ActionDescriptor.RouteValues.Add("controller", controllerName);
            Mock.Get(viewFinder)
                .Setup(x => x.FindView(controllerName, viewName, It.IsAny<string[]>()))
                .Returns(() => new FindViewResult(path));

            Assert.That(() => sut.FindView(context, viewName, isMainPage),
                        Has.Property(nameof(ViewEngineResult.Success)).True
                            .And.Property(nameof(ViewEngineResult.View)).InstanceOf<ZptSharpView>());
        }

        [Test,AutoMoqData]
        public void FindView_returns_not_found_result_when_view_finder_cant_find_view([Frozen] IFindsView viewFinder,
                                                                                      ZptSharpViewEngine sut,
                                                                                      [MockContext] ActionContext context,
                                                                                      string viewName,
                                                                                      string controllerName,
                                                                                      bool isMainPage,
                                                                                      string[] locations)
        {
            context.ActionDescriptor.RouteValues.Clear();
            context.ActionDescriptor.RouteValues.Add("controller", controllerName);
            Mock.Get(viewFinder)
                .Setup(x => x.FindView(controllerName, viewName, It.IsAny<string[]>()))
                .Returns(() => new FindViewResult(locations));

            Assert.That(() => sut.FindView(context, viewName, isMainPage),
                        Has.Property(nameof(ViewEngineResult.Success)).False
                            .And.Property(nameof(ViewEngineResult.SearchedLocations)).EqualTo(locations));
        }

        [Test,AutoMoqData]
        public void GetView_returns_found_result_when_view_exists_with_absolute_path([Frozen] ITestForFileExistence fileTester,
                                                                                     ZptSharpViewEngine sut,
                                                                                     string executingFilePath,
                                                                                     bool isMainPage)
        {
            var viewPath = "~/Foo/Bar/MyView.html";
            Mock.Get(fileTester).Setup(x => x.DoesFileExist("Foo/Bar/MyView.html")).Returns(true);
            var result = sut.GetView(executingFilePath, viewPath, isMainPage);

            Assert.That(result?.View, Is.InstanceOf<ZptSharpView>().With.Property(nameof(ZptSharpView.Path)).EqualTo("Foo/Bar/MyView.html"));
        }

        [Test,AutoMoqData]
        public void GetView_returns_found_result_when_view_exists_with_relative_path([Frozen] ITestForFileExistence fileTester,
                                                                                     ZptSharpViewEngine sut,
                                                                                     bool isMainPage)
        {
            var executingFilePath = "Foo/SomeFile.exe";
            var viewPath = "Bar/MyView.html";

            Mock.Get(fileTester).Setup(x => x.DoesFileExist("Foo/Bar/MyView.html")).Returns(true);
            var result = sut.GetView(executingFilePath, viewPath, isMainPage);

            Assert.That(result?.View, Is.InstanceOf<ZptSharpView>().With.Property(nameof(ZptSharpView.Path)).EqualTo("Foo/Bar/MyView.html"));
        }

        [Test,AutoMoqData]
        public void GetView_returns_not_found_result_when_view_does_not_exist([Frozen] ITestForFileExistence fileTester,
                                                                              ZptSharpViewEngine sut,
                                                                              string executingFilePath,
                                                                              bool isMainPage)
        {
            var viewPath = "~/Foo/Bar/MyView.html";
            Mock.Get(fileTester).Setup(x => x.DoesFileExist("Foo/Bar/MyView.html")).Returns(false);
            var result = sut.GetView(executingFilePath, viewPath, isMainPage);

            Assert.That(result.Success, Is.False);
        }
    }
}