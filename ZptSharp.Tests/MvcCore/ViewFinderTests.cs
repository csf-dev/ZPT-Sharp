using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.MvcCore
{
    [TestFixture, Parallelizable]
    public class ViewFinderTests
    {
        [Test,AutoMoqData]
        public void FindView_returns_success_result_if_view_exists_in_first_location([Frozen] IMapsLocation mapper,
                                                                                     [Frozen] ITestForFileExistence fileTester,
                                                                                     ViewFinder sut,
                                                                                     string controllerName,
                                                                                     string viewName)
        {
            var locationFormat1 = "Foo/{0}/{1}";
            var expectedPath = $"Foo/{viewName}/{controllerName}";
            Mock.Get(fileTester).Setup(x => x.DoesFileExist(expectedPath)).Returns(true);
            Mock.Get(mapper).Setup(x => x.MapLocation(It.IsAny<string>())).Returns((string s) => s);

            Assert.That(() => sut.FindView(controllerName, viewName, new [] { locationFormat1 }),
                        Has.Property(nameof(FindViewResult.Success)).True
                            .And.Property(nameof(FindViewResult.Path)).EqualTo(expectedPath));
        }

        [Test,AutoMoqData]
        public void FindView_returns_success_result_if_view_exists_in_second_location([Frozen] IMapsLocation mapper,
                                                                                      [Frozen] ITestForFileExistence fileTester,
                                                                                      ViewFinder sut,
                                                                                      string controllerName,
                                                                                      string viewName)
        {
            var locationFormat1 = "Foo/{0}/{1}";
            var locationFormat2 = "Bar/{0}/{1}";
            var expectedPath = $"Bar/{viewName}/{controllerName}";
            Mock.Get(fileTester).Setup(x => x.DoesFileExist(It.IsAny<string>())).Returns(false);
            Mock.Get(fileTester).Setup(x => x.DoesFileExist(expectedPath)).Returns(true);
            Mock.Get(mapper).Setup(x => x.MapLocation(It.IsAny<string>())).Returns((string s) => s);
            
            Assert.That(() => sut.FindView(controllerName, viewName, new [] { locationFormat1, locationFormat2 }),
                        Has.Property(nameof(FindViewResult.Success)).True
                            .And.Property(nameof(FindViewResult.Path)).EqualTo(expectedPath));
        }

        [Test,AutoMoqData]
        public void FindView_returns_failure_result_if_view_does_not_exist([Frozen] IMapsLocation mapper,
                                                                           [Frozen] ITestForFileExistence fileTester,
                                                                           ViewFinder sut,
                                                                           string controllerName,
                                                                           string viewName)
        {
            var locationFormat1 = "Foo/{0}/{1}";
            var locationFormat2 = "Bar/{0}/{1}";
            var expectedAttempts = new []
            {
                $"Foo/{viewName}/{controllerName}",
                $"Bar/{viewName}/{controllerName}",
            };
            Mock.Get(fileTester).Setup(x => x.DoesFileExist(It.IsAny<string>())).Returns(false);
            Mock.Get(mapper).Setup(x => x.MapLocation(It.IsAny<string>())).Returns((string s) => s);
            
            Assert.That(() => sut.FindView(controllerName, viewName, new [] { locationFormat1, locationFormat2 }),
                        Has.Property(nameof(FindViewResult.Success)).False
                            .And.Property(nameof(FindViewResult.AttemptedLocations)).EqualTo(expectedAttempts));
        }
    }
}