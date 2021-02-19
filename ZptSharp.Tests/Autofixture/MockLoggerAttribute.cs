using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;

namespace ZptSharp.Autofixture
{
    public class MockLoggerAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (!typeof(ILogger).IsAssignableFrom(parameter.ParameterType))
                throw new ArgumentException($"The parameter must implement {nameof(ILogger)}.", nameof(parameter));

            if (parameter.ParameterType == typeof(ILogger))
                return new MockLoggerCustomization();

            if (parameter.ParameterType.IsGenericType
             && parameter.ParameterType.GetGenericTypeDefinition() == typeof(ILogger<>))
            {
                var categoryType = parameter.ParameterType.GetGenericArguments()[0];
                var customizationType = typeof(MockLoggerCustomization<>).MakeGenericType(categoryType);
                return (ICustomization)Activator.CreateInstance(customizationType);
            }

            throw new ArgumentException($"The parameter must either be of type {nameof(ILogger)} or its generic derived interface.", nameof(parameter));
        }

        class MockLoggerCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<ILogger>(c =>
                {
                    return c
                        .FromFactory(() => Mock.Of<ILogger>())
                        .Do(x =>
                        {
                            Mock.Get(x).Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
                        });
                });
            }
        }

        class MockLoggerCustomization<TCategory> : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<ILogger<TCategory>>(c =>
                {
                    return c
                        .FromFactory(() => Mock.Of<ILogger<TCategory>>())
                        .Do(x =>
                        {
                            Mock.Get(x).Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
                        });
                });
            }
        }
    }
}
