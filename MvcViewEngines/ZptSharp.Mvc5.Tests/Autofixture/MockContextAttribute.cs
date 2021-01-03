using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;

namespace ZptSharp.Mvc5.Autofixture
{
    public class MockContextAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new MockContextCustomization();
    }

    public class MockContextCustomization : ICustomization
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

            fixture.Customize<ControllerContext>(c => {
                return c
                    .FromFactory(() => {
                        var mock = new Mock<ControllerContext>();
                        mock.SetupAllProperties();
                        return mock.Object;
                    })
                    .Without(x => x.DisplayMode);
            });

            fixture.Customize<HttpContextBase>(c => {
                return c
                    .FromFactory((HttpServerUtilityBase server, HttpApplicationStateBase app, HttpRequestBase request, HttpResponseBase response) => {
                        var mock = new Mock<HttpContextBase>();
                        mock.SetupAllProperties();
                        mock.SetupGet(x => x.Server).Returns(server);
                        mock.SetupGet(x => x.Application).Returns(app);
                        mock.SetupGet(x => x.Request).Returns(request);
                        mock.SetupGet(x => x.Response).Returns(response);
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

            fixture.Customize<HttpRequestBase>(c => {
                return c
                    .FromFactory(() => {
                        var mock = new Mock<HttpRequestBase>();
                        mock.SetupAllProperties();
                        return mock.Object;
                    })
                    .Without(x => x.RequestContext);
            });

            fixture.Customize<HttpResponseBase>(c => {
                return c
                    .FromFactory(() => {
                        var mock = new Mock<HttpResponseBase>();
                        mock.SetupAllProperties();
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
