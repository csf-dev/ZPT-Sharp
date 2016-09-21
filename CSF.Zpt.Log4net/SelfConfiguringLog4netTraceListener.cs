using System;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="Log4netTraceListener"/> which self-configures from the XML config.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type calls <c>log4net.Config.XmlConfigurator.Configure();</c> in its static constructor.
  /// </para>
  /// </remarks>
  public class SelfConfiguringLog4netTraceListener : Log4netTraceListener
  {
    public SelfConfiguringLog4netTraceListener() : base() {}

    public SelfConfiguringLog4netTraceListener(log4net.ILog logger) : base(logger) {}

    static SelfConfiguringLog4netTraceListener()
    {
      log4net.Config.XmlConfigurator.Configure();
    }
  }
}

