using System;
using CSF.Cli;
using CSF.Zpt.Rendering;
using CSF.Zpt.BatchRendering;
using System.Text;
using CSF.Zpt.Cli.BatchRendering;
using CSF.Zpt.Cli.Rendering;
using System.Diagnostics;

namespace CSF.Zpt.Cli
{
  public class Application
  {
    #region fields

    private readonly string[] _args;
    private readonly ICommandLineOptionsFactory _commandlineOptionsFactory;
    private readonly IBatchRenderingOptionsFactory _batchOptionsFactory;
    private readonly IRenderingOptionsFactory _renderingOptionsFactory;
    private readonly IBatchRenderer _renderer;
    private readonly IVersionNumberInspector _versionInspector;

    #endregion

    #region methods

    /// <summary>
    /// Perform the work of the application.
    /// </summary>
    public void Begin()
    {
      var commandlineOptions = GetCommandlineOptions();

      HandleInformationalActions(commandlineOptions);

      var batchOptions = GetBatchOptions(commandlineOptions);
      var renderingOptions = GetRenderingOptions(commandlineOptions);
      var renderingMode = GetRenderingMode(commandlineOptions);

      _renderer.Render(renderingOptions, batchOptions, renderingMode);
    }

    private CommandLineOptions GetCommandlineOptions()
    {
      return SafeGetValue<CommandLineOptions>(() => _commandlineOptionsFactory.GetOptions(_args));
    }

    private IBatchRenderingOptions GetBatchOptions(CommandLineOptions options)
    {
      return SafeGetValue<IBatchRenderingOptions>(() => _batchOptionsFactory.GetBatchOptions(options));
    }

    private IRenderingOptions GetRenderingOptions(CommandLineOptions options)
    {
      return SafeGetValue<IRenderingOptions>(() => _renderingOptionsFactory.GetOptions(options));
    }

    private RenderingMode? GetRenderingMode(CommandLineOptions options)
    {
      return SafeGetValue<RenderingMode?>(() => options.GetRenderingMode());
    }

    private TOutput SafeGetValue<TOutput>(Func<TOutput> getter)
    {
      TOutput output = default(TOutput);

      try
      {
        output = getter();
      }
      catch(OptionsParsingException ex)
      {
        Trace.TraceError(ex.ToString());
        Console.Error.WriteLine(Resources.Messages.CannotParseOptionsError);
        Environment.Exit(1);
      }
      catch(Exception ex)
      {
        Trace.TraceError(ex.ToString());
        Console.Error.WriteLine(Resources.Messages.UnexpectedError);
        Environment.Exit(2);
      }

      return output;
    }

    private void HandleInformationalActions(CommandLineOptions options)
    {
      if(options.ShowUsageStatement)
      {
        Console.Write(Resources.Messages.UsageStatement);
        Environment.Exit(0);
      }
      else if (options.ShowVersionInfo)
      {
        Console.WriteLine(Resources.Messages.VersionFormat, _versionInspector.GetZptBuilderVersion());
        Environment.Exit(0);
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Cli.EntryPoint"/> class.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <param name="parameterParser">A parameter parser.</param>
    /// <param name="contextVisitorFactory">A context visitor factory.</param>
    /// <param name="contextFactoryFactory">A rendering context factory factory.</param>
    public Application(string[] args,
                       ICommandLineOptionsFactory commandlineOptionsFactory = null,
                       IBatchRenderingOptionsFactory batchOptionsFactory = null,
                       IRenderingOptionsFactory renderingOptionsFactory = null,
                       IBatchRenderer renderer = null,
                       IVersionNumberInspector versionInspector = null)
    {
      if(args == null)
      {
        throw new ArgumentNullException(nameof(args));
      }

      _args = args;

      _commandlineOptionsFactory = commandlineOptionsFactory?? new CommandLineOptionsFactory();
      _batchOptionsFactory = batchOptionsFactory?? new BatchRenderingOptionsFactory();
      _renderingOptionsFactory = renderingOptionsFactory?? new RenderingOptionsFactory();
      _renderer = renderer?? new BatchRenderer();
      _versionInspector = versionInspector?? new VersionNumberInspector();
    }

    #endregion
  }
}

