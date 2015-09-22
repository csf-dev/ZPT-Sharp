using System;
using NUnit.Framework;

namespace Test.CSF.Zpt
{
  [SetUpFixture]
  public class GlobalSetupFixture
  {
    [SetUp]
    public void Setup()
    {
      // Ensure that log4net is looking at the correct config file
      log4net.Config.XmlConfigurator.Configure();
    }
  }
}

