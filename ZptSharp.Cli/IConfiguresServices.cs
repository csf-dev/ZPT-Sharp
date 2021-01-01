namespace ZptSharp
{
    /// <summary>
    /// An object which pre-configures the services which make-up the CLI app.
    /// </summary>
    public interface IConfiguresServices
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        void ConfigureServices(CliArguments args);
    }
}