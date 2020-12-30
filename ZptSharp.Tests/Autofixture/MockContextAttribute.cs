using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ZptSharp.Autofixture
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
                    })
                    .Without(x => x.ViewData);
            });

            fixture.Customize<HttpContext>(c => {
                return c
                    .FromFactory((HttpRequest request, HttpResponse response) => {
                        var mock = new Mock<HttpContext>();
                        mock.SetupAllProperties();
                        mock.SetupGet(x => x.Request).Returns(request);
                        mock.SetupGet(x => x.Response).Returns(response);
                        return mock.Object;
                    });
            });

            fixture.Customize<HttpRequest>(c => {
                return c
                    .FromFactory(() => {
                        var mock = new Mock<HttpRequest>();
                        mock.SetupAllProperties();
                        return mock.Object;
                    });
            });

            fixture.Customize<HttpResponse>(c => {
                return c
                    .FromFactory(() => {
                        var mock = new Mock<HttpResponse>();
                        mock.SetupAllProperties();
                        return mock.Object;
                    });
            });

            fixture.Customize<PathString>(c => c.FromFactory((string s) => new PathString($"/{s}")));

            fixture.Customize<QueryString>(c => c.FromFactory(() => new QueryString("?foo=bar")));
        }
    }
}
