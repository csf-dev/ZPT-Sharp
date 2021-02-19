using CSF.Cli;

namespace ZptSharp.Cli
{
    /// <summary>
    /// A utility which uses <see cref="ParameterParserBuilder{TArgs}" /> to parse
    /// a set of source arguments into an instance of <see cref="CliArguments" />.
    /// </summary>
    public static class CliArgumentsParser
    {
        /// <summary>
        /// Parses the source arguments and returns a <see cref="CliArguments" />.
        /// </summary>
        /// <param name="args">The source command line args.</param>
        /// <returns>The parsed arguments.</returns>
        public static CliArguments Parse(string[] args)
        {
            var builder = new ParameterParserBuilder<CliArguments>();

            builder.RemainingArguments(x => x.RootPath);
            builder.AddValue(x => x.CommaSeparatedIncludePatterns, "i", "include");
            builder.AddValue(x => x.CommaSeparatedExcludePatterns, "x", "exclude");
            builder.AddValue(x => x.PathToModelJson, "m", "model");
            builder.AddValue(x => x.CommaSeparatedKeywordOptions, "k", "keywords");
            builder.AddValue(x => x.OutputFileExtension, "e", "extension");
            builder.AddValue(x => x.OutputPath, "o", "output");
            builder.AddFlag(x => x.UseAngleSharp, "s", "anglesharp");
            builder.AddFlag(x => x.EnableSourceAnnotation, "a", "annotate");

            var parser = builder.Build();
            return parser.Parse(args);
        }
    }
}