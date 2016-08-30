using System;

namespace CSF.Zpt
{
  /// <summary>
  /// Log4net trace listener - taken form http://stackoverflow.com/a/515536/6221779
  /// </summary>
  public class Log4netTraceListener : System.Diagnostics.TraceListener
  {
    private readonly log4net.ILog _log;

    public Log4netTraceListener()
    {
      _log = log4net.LogManager.GetLogger("System.Diagnostics.Redirection");
    }

    public Log4netTraceListener(log4net.ILog log)
    {
      _log = log;
    }

    public override void Write(string message)
    {
      if (_log != null)
      {
        _log.Debug(message);
      }
    }

    public override void WriteLine(string message)
    {
      if (_log != null)
      {
        _log.Debug(message);
      }
    }
  }
}

