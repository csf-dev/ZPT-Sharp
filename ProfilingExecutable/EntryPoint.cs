using System;
using Test.CraigFowler.Web.ZPT.Tal;

namespace CraigFowler.Web.ZPT.Cli
{
	/// <summary>
	/// <para>Entry point class serves as the main class within this executable.</para>
	/// </summary>
	public static class EntryPoint
	{
		/// <summary>
		/// <para>Static entry point for this executable.</para>
		/// </summary>
		/// <param name="commandlineParams">
		/// A <see cref="System.String[]"/>
		/// </param>
		public static void Main(string[] commandlineParams)
		{
			TalIntegrationTests testClass = new TalIntegrationTests();
			testClass.TestBenchmarkTalDocument();
		}
	}
}

