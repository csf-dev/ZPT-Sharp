using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Metal;

namespace ZptSharp.Util
{
    public static class IntegrationTestDataProvider
    {
        public static MetalMacro GetPnomeTemplatePageMacro(IServiceProvider serviceProvider)
        {
            var templatePath = TestFiles.GetPath("ZptIntegrationTests", "SourceDocuments", "pnome_template.pt");
            return GetPageMacro(templatePath, serviceProvider);
        }

        public static MetalMacro GetAcmeTemplatePageMacro(IServiceProvider serviceProvider)
        {
            var templatePath = TestFiles.GetPath("ZptIntegrationTests", "SourceDocuments", "acme_template.html");
            return GetPageMacro(templatePath, serviceProvider);
        }

        public static object GetOptionsObject(IServiceProvider serviceProvider)
        {
            return new
            {
                content = new { args = "yes", },
                batch = GetBatchObject(),
                enumerableItems = new List<object>(GetItems()),
                getProducts = GetProducts(),
                laf = GetLafTemplate(serviceProvider),
            };
        }

        static IEnumerable<BatchItem> GetItems()
        {
            return new[] { "one", "two", "three", "four", "five" }
                .Select((item, idx) => new BatchItem { num = idx.ToString(), str = item });
        }

        static BatchObject GetBatchObject()
        {
            var batch = new BatchObject
            {
                previous_sequence = false,
                previous_sequence_start_item = "yes",
                next_sequence = true,
                next_sequence_start_item = "six",
                next_sequence_end_item = "ten",
            };
            batch.AddRange(GetItems());
            return batch;
        }

        static object GetProducts()
        {
            return new[]
            {
                new
                {
                    description = "This is the tee for those who LOVE Zope. Show your heart on your tee.",
                    image = "smlatee.jpg",
                    price = 12.99m,
                },
                new
                {
                    description = "This is the tee for Jim Fulton. He's the Zope Pope!",
                    image = "smpztee.jpg",
                    price = 11.99m,
                }
            };
        }

        static object GetLafTemplate(IServiceProvider serviceProvider)
        {
            var templatePath = TestFiles.GetPath("ZptIntegrationTests", "SourceDocuments", "teeshoplaf.html");
            return GetMetalDocumentAdapter(templatePath, serviceProvider);
        }

        static MetalMacro GetPageMacro(string templatePath, IServiceProvider serviceProvider)
        {
            var documentAdapter = GetMetalDocumentAdapter(templatePath, serviceProvider);
            return documentAdapter.GetMacros()["page"];
        }

        static MetalDocumentAdapter GetMetalDocumentAdapter(string templatePath, IServiceProvider serviceProvider)
        {
            using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var template = new HapDocumentProvider().GetDocumentAsync(stream, serviceProvider.GetRequiredService<RenderingConfig>()).Result;
                return new MetalDocumentAdapter(template,
                                                serviceProvider.GetRequiredService<ISearchesForAttributes>(),
                                                serviceProvider.GetRequiredService<IGetsMetalAttributeSpecs>());
            }
        }

        #region contained model objects

        public class BatchObject : List<object>
        {
            public object previous_sequence;
            public object previous_sequence_start_item;
            public object next_sequence;
            public object next_sequence_start_item;
            public object next_sequence_end_item;
        }

        public class BatchItem
        {
            public string num;
            public string str;

            public override string ToString() => str;
        }

        #endregion

    }
}
