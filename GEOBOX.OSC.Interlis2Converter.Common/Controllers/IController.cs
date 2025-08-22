using GEOBOX.OSC.Common.Logging;

namespace GEOBOX.OSC.Interlis2Converter.Common.Controllers
{
    /// <summary>
    /// Interface for all Commandline implementations
    /// </summary>
    public interface IController : IDisposable
    {
        /// <summary>
        /// Get the name (function type) - in Command-Arguments --type
        /// Key in List "AvailableControllers": must be unique
        /// </summary>
        /// <returns>command type</returns>
        static string CommandType { get; }

        /// <summary>
        /// Name of Controller for Display and Logging
        /// </summary>
        /// <returns>command type</returns>
        string DisplayName { get; }

        /// <summary>
        /// Execute Command
        /// </summary>
        /// <returns>true is runnig without errors</returns>
        bool Execute();

        bool CheckCommandlineOptions();

        /// <summary>
        /// Logger with all Messages from execute
        /// </summary>
        ILogger Logger { get; }
    }
}