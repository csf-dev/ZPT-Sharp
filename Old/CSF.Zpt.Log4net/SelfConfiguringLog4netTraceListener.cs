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
    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Zpt.SelfConfiguringLog4netTraceListener"/> class.
    /// </summary>
    public SelfConfiguringLog4netTraceListener() : base() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Zpt.SelfConfiguringLog4netTraceListener"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public SelfConfiguringLog4netTraceListener(log4net.ILog logger) : base(logger) {}

    /// <summary>
    /// Initializes the <see cref="T:CSF.Zpt.SelfConfiguringLog4netTraceListener"/> class.
    /// </summary>
    static SelfConfiguringLog4netTraceListener()
    {
      log4net.Config.XmlConfigurator.Configure();
    }
  }
}

