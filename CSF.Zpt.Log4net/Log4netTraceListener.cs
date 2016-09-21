using System;
using System.Diagnostics;
using System.Collections.Generic;
using log4net;

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

    private ILog _defaultLogger;
    private IDictionary<string, ILog> _loggers;
    private readonly object _syncRoot;

    #endregion

    #region methods

    /// <summary>
    /// Write the specified message.
    /// </summary>
    /// <param name="message">Message.</param>
    public override void Write(string message)
    {
      var log = GetLogger();

      if (log != null)
      {
        log.Debug(message);
      }
    }

    /// <summary>
    /// Writes a line of log output.
    /// </summary>
    /// <param name="message">Message.</param>
    public override void WriteLine(string message)
    {
      var log = GetLogger();

      if (log != null)
      {
        log.Debug(message);
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
      var logFunction = GetLogFunction(eventType);
      var logger = GetLogger(source);

      logFunction(logger, format, args);
    }

    /// <summary>
    /// Writes a log message corresponding to an event.
    /// </summary>
    /// <param name="eventCache">Event cache.</param>
    /// <param name="source">Source.</param>
    /// <param name="eventType">Event type.</param>
    /// <param name="id">Identifier.</param>
    /// <param name="message">Message.</param>
    public override void TraceEvent(TraceEventCache eventCache,
                                    string source,
                                    TraceEventType eventType,
                                    int id,
                                    string message)
    {
      this.TraceEvent(eventCache, source, eventType, id, message, new object[0]);
    }

    private Action<ILog,string,object[]> GetLogFunction(TraceEventType eventType)
    {
      Action<ILog, string,object[]> output;

      switch(eventType)
      {
      case TraceEventType.Error:
        output = (ILog logger, string format, object[] args) => logger.ErrorFormat(format, args);
        break;

      case TraceEventType.Information:
        output = (ILog logger, string format, object[] args) => logger.InfoFormat(format, args);
        break;

      case TraceEventType.Warning:
        output = (ILog logger, string format, object[] args) => logger.WarnFormat(format, args);
        break;

      case TraceEventType.Critical:
        output = (ILog logger, string format, object[] args) => logger.FatalFormat(format, args);
        break;

      case TraceEventType.Verbose:
        output = (ILog logger, string format, object[] args) => logger.DebugFormat(format, args);
        break;

      default:
        output = (ILog logger, string format, object[] args) => logger.DebugFormat(format, args);
        break;
      }

      return output;
    }

    private ILog GetLogger(string sourceName)
    {
      lock(_syncRoot)
      {
        if(!_loggers.ContainsKey(sourceName))
        {
          _loggers.Add(sourceName, LogManager.GetLogger(sourceName));
        }
      }

      return _loggers[sourceName];
    }

    private ILog GetLogger()
    {
      return _defaultLogger;
    }

    #endregion

    #region constructor

    public Log4netTraceListener() : this(null) {}

    public Log4netTraceListener(log4net.ILog log)
    {
      _syncRoot = new object();
      _loggers = new Dictionary<string, ILog>();
      _defaultLogger = log?? LogManager.GetLogger(ZptConstants.TraceSourceName);
    }

    #endregion
  }
}

