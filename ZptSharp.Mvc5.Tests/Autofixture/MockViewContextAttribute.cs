using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;

namespace ZptSharp.Mvc.Autofixture
{
    public class MockViewContextAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new MockViewContextCustomization();
    }

    public class MockViewContextCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ViewContext>(c => {
                return c
                    .FromFactory(() => {
                        var mock = new Mock<ViewContext>();
                        mock.SetupAllProperties();
                        return mock.Object;
                    });
            });

            fixture.Customize<HttpContextBase>(c => {
                return c
                    .FromFactory((HttpServerUtilityBase server, HttpApplicationStateBase app) => {
                        var mock = new Mock<HttpContextBase>();
                        mock.SetupAllProperties();
                        mock.SetupGet(x => x.Server).Returns(server);
                        mock.SetupGet(x => x.Application).Returns(app);
                        return mock.Object;
                    });
            });

            fixture.Customize<HttpServerUtilityBase>(c => {
                return c
                    .FromFactory((string s) => {
                        var mock = new Mock<HttpServerUtilityBase>();
                        mock.SetupAllProperties();
                        mock.Setup(x => x.MapPath(It.IsAny<string>())).Returns(s);
                        return mock.Object;
                    });
            });

            fixture.Customize<HttpApplicationStateBase>(c => {
                return c
                    .FromFactory(() => {
                        var mock = new Mock<HttpApplicationStateBase>();
                        mock.SetupAllProperties();
                        return mock.Object;
                    });
            });
        }
    }
}
