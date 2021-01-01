using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.BulkRendering;
using ZptSharp.Config;

namespace ZptSharp
{
    /// <summary>
    /// Implementation of <see cref="IGetsBulkRenderingRequest" /> which transforms the
    /// CLI arguments into the proper type for bulk-rendering.
    /// </summary>
    public class BulkRenderingRequestFactory : IGetsBulkRenderingRequest
    {
        readonly ILoadsModel modelLoader;

        /// <summary>
        /// Gets the bulk rendering request.
        /// </summary>
        /// <param name="args">The command line args.</param>
        /// <param name="cancellationToken">A optional cancellation token.</param>
        /// <returns>A bulk rendering request.</returns>
        public Task<BulkRenderingRequest> GetRequestAsync(CliArguments args, CancellationToken cancellationToken = default)
        {
            if (args is null)
                throw new System.ArgumentNullException(nameof(args));
            if(args.RootPath?.Count != 1)
                throw new ArgumentException(Resources.ExceptionMessage.MustHavePreciselyOneRootPath, nameof(args));

            return GetRequestPrivateAsync(args, cancellationToken);
        }

        async Task<BulkRenderingRequest> GetRequestPrivateAsync(CliArguments args, CancellationToken cancellationToken)
        {
            return new BulkRenderingRequest
            {
                InputRootPath = args.RootPath.Single(),
                IncludedPaths = GetIncludedPaths(args),
                ExcludedPaths = SeparateSeparatedValues(args.CommaSeparatedExcludePatterns),
                OutputFileExtension = args.OutputFileExtension,
                OutputPath = args.OutputPath ?? Directory.GetCurrentDirectory(),
                RenderingConfig = GetRenderingConfig(args),
                Model = await GetModelAsync(args, cancellationToken).ConfigureAwait(false),
            };
        }

        static IList<string> GetIncludedPaths(CliArguments args)
        {
            var paths = SeparateSeparatedValues(args.CommaSeparatedIncludePatterns);
            if(paths.Any()) return paths;
            return new [] { "*.*" };
        }

        static IList<string> SeparateSeparatedValues(string source, char separator = ',')
        {
            if(source == null) return new string[0];

            var items = source.Split(separator).ToList();

            // Merge across any empty entries, which deals with double-commas (escaping a comma).
            for(int i = items.Count - 1; i >= 0; i--)
            {
                if(items[i].Length != 0 || i == items.Count - 1 || i == 0)
                    continue;
                
                items[i - 1] = String.Concat(items[i - 1], separator, items[i + 1]);
            }

            return items;
        }

        static RenderingConfig GetRenderingConfig(CliArguments args)
        {
            var config = RenderingConfig.CreateBuilder();
            
            if(args.EnableSourceAnnotation)
            {
                config.IncludeSourceAnnotation = true;
                config.SourceAnnotationBasePath = args.RootPath.Single();
            }

            if(!String.IsNullOrEmpty(args.CommaSeparatedKeywordOptions))
            {
                var keyValuePairs = SeparateSeparatedValues(args.CommaSeparatedKeywordOptions);
                foreach (var kvp in keyValuePairs)
                {
                    var parts = kvp.Split(new [] {'='}, 2);
                    if(parts.Length != 2) continue;
                    config.KeywordOptions.Add(parts[0], parts[1]);
                }
            }

            return config.GetConfig();
        }

        async Task<object> GetModelAsync(CliArguments args, CancellationToken cancellationToken)
        {
            if(String.IsNullOrEmpty(args.PathToModelJson)) return null;
            return await modelLoader.LoadModelAsync(args.PathToModelJson, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes 
        /// </summary>
        /// <param name="modelLoader"></param>
        public BulkRenderingRequestFactory(ILoadsModel modelLoader)
        {
            this.modelLoader = modelLoader ?? throw new ArgumentNullException(nameof(modelLoader));
        }
    }
}