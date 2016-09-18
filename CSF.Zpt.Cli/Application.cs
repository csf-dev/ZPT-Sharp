using System;
using CSF.Cli;
using CSF.Zpt.Rendering;
using CSF.Zpt.BatchRendering;
using System.Text;
using System.Diagnostics;
using CSF.Zpt.Cli.Exceptions;
using CSF.Zpt.Cli.Resources;

namespace CSF.Zpt.Cli
{
  public class Application
  {
    #region fields

    private readonly ICommandLineOptionsFactory _commandlineOptionsFactory;
    private readonly IBatchRenderingOptionsFactory _batchOptionsFactory;
    private readonly IRenderingOptionsFactory _renderingOptionsFactory;
    private readonly IBatchRenderer _renderer;
    private readonly IVersionNumberInspector _versionInspector;
    private readonly IApplicationTerminator _terminator;

    #endregion

    #region methods

    /// <summary>
    /// Perform the work of the application.
    /// </summary>
    public void Begin(string[] args)
    {
      if(args == null)
      {
        throw new ArgumentNullException(nameof(args));
      }

      var commandlineOptions = GetCommandlineOptions(args);
      if(commandlineOptions == null)
      {
        return;
      }

      Begin(commandlineOptions);
    }

    public void Begin(CommandLineOptions options)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      if(HandleInformationalActions(options))
      {
        return;
      }

      var batchOptions = GetBatchOptions(options);
      var renderingOptions = GetRenderingOptions(options);
      var renderingMode = GetRenderingMode(options);

      if(batchOptions == null
         || renderingOptions == null)
      {
        return;
      }

      var response = Render(renderingOptions, batchOptions, renderingMode);
      WriteResponseInfo(response, options);
    }

    private IBatchRenderingResponse Render(IRenderingOptions options,
                                           IBatchRenderingOptions batchOptions,
                                           RenderingMode? renderingMode)
    {
      IBatchRenderingResponse output = null;

      try
      {
        output = _renderer.Render(options, batchOptions, renderingMode);
      }
      catch(InvalidBatchRenderingOptionsException ex)
      {
        switch(ex.FatalError.Value)
        {
        case BatchRenderingFatalErrorType.NoInputsSpecified:
          WriteErrorAndTerminate(ex, OutputMessages.NoInputs);
          break;

        case BatchRenderingFatalErrorType.InputCannotBeBothStreamAndPaths:
          WriteErrorAndTerminate(ex, OutputMessages.CannotInputFromStdInAndPaths);
          break;

        default:
          string message = String.Format(OutputMessages.UnexpectedErrorFormat, ex.ToString());
          WriteUnexpectedErrorAndTerminate(ex, message);
          break;
        }
      }
      catch(Exception ex)
      {
        string message = String.Format(OutputMessages.UnexpectedErrorFormat, ex.ToString());
        WriteUnexpectedErrorAndTerminate(ex, message);
      }

      return output;
    }

    private void WriteResponseInfo(IBatchRenderingResponse response,
                                   CommandLineOptions options)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      if(response != null)
      {
        // TODO: Do something with the response
      }
    }

    private CommandLineOptions GetCommandlineOptions(string[] args)
    {
      return SafeGetValue<CommandLineOptions>(() => _commandlineOptionsFactory.GetOptions(args));
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
      catch(RenderingModeDeterminationException ex)
      {
        WriteErrorAndTerminate(ex, OutputMessages.HtmlAndXmlModesMutuallyExclusiveError);
      }
      catch(InvalidInputPathException ex)
      {
        string message = String.Format(OutputMessages.InvalidInputPathFormat, ex.Path);
        WriteErrorAndTerminate(ex, message);
      }
      catch(InvalidOutputPathException ex)
      {
        WriteErrorAndTerminate(ex, OutputMessages.InvalidOutputPath);
      }
      catch(InvalidKeywordOptionsException ex)
      {
        string message = String.Format(OutputMessages.InvalidKeywordOptionFormat, ex.InvalidOption);
        WriteErrorAndTerminate(ex, message);
      }
      catch(InvalidOutputEncodingException ex)
      {
        WriteErrorAndTerminate(ex, OutputMessages.InvalidEncoding);
      }
      catch(OptionsParsingException ex)
      {
        WriteErrorAndTerminate(ex, OutputMessages.CannotParseOptionsError);
      }
      catch(CouldNotCreateContextVisitorException ex)
      {
        string message = String.Format(OutputMessages.CouldNotCreateContextVisitorFormat, ex.InvalidClassname);
        WriteErrorAndTerminate(ex, message);
      }
      catch(CouldNotCreateRenderingContextFactoryException ex)
      {
        WriteErrorAndTerminate(ex, OutputMessages.CouldNotCreateRenderingContextFactory);
      }
      catch(Exception ex)
      {
        string message = String.Format(OutputMessages.UnexpectedErrorFormat, ex.ToString());
        WriteUnexpectedErrorAndTerminate(ex, message);
      }

      return output;
    }

    private bool HandleInformationalActions(CommandLineOptions options)
    {
      bool informationalActionPerformed = false;

      if(options.ShowUsageStatement)
      {
        Console.Write(OutputMessages.UsageStatement);
        _terminator.Terminate();
        informationalActionPerformed = true;
      }
      else if (options.ShowVersionInfo)
      {
        Console.WriteLine(OutputMessages.VersionFormat, _versionInspector.GetZptBuilderVersion());
        _terminator.Terminate();
        informationalActionPerformed = true;
      }

      return informationalActionPerformed;
    }

    private void WriteErrorAndTerminate(Exception exception, string message)
    {
      ZptConstants.TraceSource.TraceEvent(TraceEventType.Critical, 1, exception.ToString());
      Console.Error.WriteLine(message);
      _terminator.Terminate(ApplicationTerminator.ExpectedErrorExitCode);

    }

    private void WriteUnexpectedErrorAndTerminate(Exception exception, string message)
    {
      ZptConstants.TraceSource.TraceEvent(TraceEventType.Critical, 2, exception.ToString());
      Console.Error.WriteLine(message);
      _terminator.Terminate(ApplicationTerminator.UnexpectedErrorExitCode);
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
    public Application(ICommandLineOptionsFactory commandlineOptionsFactory = null,
                       IBatchRenderingOptionsFactory batchOptionsFactory = null,
                       IRenderingOptionsFactory renderingOptionsFactory = null,
                       IBatchRenderer renderer = null,
                       IVersionNumberInspector versionInspector = null,
                       IApplicationTerminator terminator = null)
    {
      _commandlineOptionsFactory = commandlineOptionsFactory?? new CommandLineOptionsFactory();
      _batchOptionsFactory = batchOptionsFactory?? new BatchRenderingOptionsFactory();
      _renderingOptionsFactory = renderingOptionsFactory?? new RenderingOptionsFactory();
      _renderer = renderer?? new BatchRenderer();
      _versionInspector = versionInspector?? new VersionNumberInspector();
      _terminator = terminator?? new ApplicationTerminator();
    }

    #endregion

    #region static methods

    /// <summary>
    /// The entry point of the program, where the program control starts and ends.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
      var app = new Application();
      app.Begin(args);
    }

    #endregion
  }
}

