using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using ZptSharp.Dom;
using ZptSharp.Metal;

namespace ZptSharp.Autofixture
{
    public class MetalDocAdapterAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new MetalDocAdapterCustomization();

        public class MetalDocAdapterCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<IGetsMetalDocumentAdapter>(x => {
                    return x.FromFactory<ISearchesForAttributes, IGetsMetalAttributeSpecs>(GetAdapterFactory);
                });
            }

            IGetsMetalDocumentAdapter GetAdapterFactory(ISearchesForAttributes attributeSearcher,
                                                        IGetsMetalAttributeSpecs specProvider)
            {
                var factory = new Mock<IGetsMetalDocumentAdapter>();
                factory
                    .Setup(x => x.GetMetalDocumentAdapter(It.IsAny<IDocument>()))
                    .Returns((IDocument doc) => new MetalDocumentAdapter(doc, attributeSearcher, specProvider));
                return factory.Object;
            }
        }
    }
}
