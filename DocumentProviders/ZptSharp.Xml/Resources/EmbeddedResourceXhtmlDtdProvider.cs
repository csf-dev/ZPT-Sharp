using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ZptSharp.Dom.Resources
{
    /// <summary>
    /// Implementation of <see cref="IGetsXhtmlDtds"/> which uses embedded resources.
    /// The purpose of this mechanism is to get XHTML DTDs (and supporting content)
    /// from a trusted source without any network requests.
    /// </summary>
    public class EmbeddedResourceXhtmlDtdProvider : IGetsXhtmlDtds
    {
        const string modulePattern = @"^(xhtml-[a-zA-Z]+)\.mod$";
        static readonly Regex module = new Regex(modulePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        static Type ThisType => typeof(EmbeddedResourceXhtmlDtdProvider);

        static Assembly ThisAssembly => ThisType.Assembly;

        /// <summary>
        /// Gets a stream containing the XHTML DTD, for the specified Uri.
        /// </summary>
        /// <returns>The XHTML DTD stream.</returns>
        /// <param name="uri">URI.</param>
        public Stream GetXhtmlDtdStream(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (!XhtmlOnlyXmlUrlResolver.W3BaseUri.IsBaseOf(uri))
                throw new ArgumentException(ExceptionMessages.InvalidUri, nameof(uri));

            var resourceName = GetResourceName(uri);
            return ThisAssembly.GetManifestResourceStream(ThisType, resourceName);
        }

        static string GetResourceName(Uri uri)
        {
            var name = uri.AbsolutePath.Split('/').Last();
            if (!module.IsMatch(name)) return name;

            return module.Replace(name, @"$1-1.mod");
        }
    }
}
