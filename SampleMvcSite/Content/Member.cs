using System;
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Samples.Mvc.Content
{
  /// <summary>
  /// <para>Stub class for testing a member-like object.</para>
  /// </summary>
  public class Member
  {
    /// <summary>
    /// <para>Username</para>
    /// </summary>
    [TalesAlias("username")]
    public string Username
    {
      get;
      set;
    }
    
    /// <summary>
    /// <para>Age in years</para>
    /// </summary>
    [TalesAlias("age")]
    public int Age
    {
      get;
      set;
    }
  }
}

