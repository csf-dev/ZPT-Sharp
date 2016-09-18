using System;
using System.Diagnostics;

namespace CSF.Zpt
{
  /// <summary>
  /// Log4net trace listener
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is an improvement upon the type documented at http://stackoverflow.com/a/515536/6221779
  /// </para>
  /// </remarks>
  public class Log4netTraceListener : TraceListener
  {
    #region fields

    private static readonly string
      LOGGER_NAME = ZptConstants.TraceSourceName,
      MESSAGE_FORMAT = "{0}: {1}";
    private readonly log4net.ILog _log;

    #endregion

    #region methods

    /// <summary>
    /// Write the specified message.
    /// </summary>
    /// <param name="message">Message.</param>
    public override void Write(string message)
    {
      if (_log != null)
      {
        _log.Debug(message);
      }
    }

    /// <summary>
    /// Writes a line of log output.
    /// </summary>
    /// <param name="message">Message.</param>
    public override void WriteLine(string message)
    {
      if (_log != null)
      {
        _log.Debug(message);
      }
    }

    /// <summary>
    /// Writes a log message corresponding to an event.
    /// </summary>
    /// <param name="eventCache">Event cache.</param>
    /// <param name="source">Source.</param>
    /// <param name="eventType">Event type.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="format">Format.</param>
    /// <param name="args">Arguments.</param>
    public override void TraceEvent(TraceEventCache eventCache,
                                    string source,
                                    TraceEventType eventType,
                                    int id,
                                    string format,
                                    params object[] args)
    {
      if(_log != null)
      {
        var logFunction = GetLogFunction(eventType);
        logFunction(String.Format(MESSAGE_FORMAT, source, format), args);
      }
    }

    private Action<string,object[]> GetLogFunction(TraceEventType eventType)
    {
      Action<string,object[]> output;

      switch(eventType)
      {
      case TraceEventType.Error:
        output = _log.ErrorFormat;
        break;

      case TraceEventType.Information:
        output = _log.InfoFormat;
        break;

      case TraceEventType.Warning:
        output = _log.WarnFormat;
        break;

      case TraceEventType.Critical:
        output = _log.FatalFormat;
        break;

      case TraceEventType.Verbose:
        output = _log.DebugFormat;
        break;

      default:
        output = _log.DebugFormat;
        break;
      }

      return output;
    }

    #endregion

    #region constructor

    public Log4netTraceListener() : this(null) {}

    public Log4netTraceListener(log4net.ILog log)
    {
      _log = log?? log4net.LogManager.GetLogger(LOGGER_NAME);
    }

    #endregion
  }
}

