using CommandLine;
using GEOBOX.OSC.Common.Logging;
using GEOBOX.OSC.Interlis2Converter.Common.Controllers;
using GEOBOX.OSC.Interlis2Converter.Common.Settings;
using GEOBOX.OSC.Interlis2Converter.ConsoleApp.Batch;
using GEOBOX.OSC.Interlis2Converter.ConsoleApp.Properties;

namespace GEOBOX.OSC.Interlis2Converter.ConsoleApp
{
    /// <summary>
    /// Programm (Main for Console)
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// All Available Controllers
        /// </summary>
        private static Dictionary<string, Type> availableControllers = AvailableControllers.Get();

        static ExitCode exitCode = ExitCode.Error;

        static int Main(string[] args)
        {
            WriteWelcomeMessage();
            if (args != null && args.Length > 0)
            // run as command line app
            {
                var commandLineOptions = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
#if DEBUG
                Console.ReadKey();
#endif          
            }
            return (int)exitCode;
        }

        /// <summary>
        /// Run Console Application with parsed Command Line Options
        /// </summary>
        /// <param name="commandLineOptions">Options from Command Line Input</param>
        static void RunOptions(CommandLineOptions commandLineOptions)
        {
            string logFilePath = $"{commandLineOptions.LogFile}";

            ILogger logger = new CustomerFriendlyLogger(FileLogger.Create(logFilePath), true);
            ((CustomerFriendlyLogger)logger).WriteHeader(Resources.genModulName, Resources.loggerComment);

            // Check is type available
            if (!availableControllers.ContainsKey(commandLineOptions.Type))
            {
                var message = Resources.CMDCallTypeNotFound;
                logger?.WriteWarning(message);
                Console.WriteLine(message);
                exitCode = ExitCode.Error;
                return;
            }

            try
            {
                // Create and set values to runtime settings given vaalues (not empty) get checked
                var runtimeSettings = new RuntimeSettings();
                runtimeSettings.SetType(commandLineOptions.Type);
                runtimeSettings.SetInputPath(commandLineOptions.InputDir);
                runtimeSettings.SetOutputDir(commandLineOptions.OutputDir);
                runtimeSettings.SetOutput(commandLineOptions.OutputFile, true);
                runtimeSettings.SetAndReadDownloadConfigFile(commandLineOptions.DownloadConfig);
                runtimeSettings.SetLogFile(commandLineOptions.LogFile, true);

                using (IController controller = (IController)Activator.CreateInstance(availableControllers[commandLineOptions.Type], new object[] { runtimeSettings, logger }))
                {
                    logger?.WriteInformation(string.Format(Resources.MessageStartFunction, controller.DisplayName));
                    //Check if needed arguments are present in the commandline
                    if (!controller.CheckCommandlineOptions())
                    {
                        exitCode = ExitCode.Error;
                        return;
                    }
                    if (controller.Execute())
                    {
                        // Success
                        var message = string.Format(Resources.MessageEndFunctionSuccess, controller.DisplayName);
                        logger?.WriteInformation(message);
                        Console.WriteLine(message);
                        exitCode = ExitCode.Success;
                    }
                    else
                    {
                        // Error
                        var message = string.Format(Resources.MessageEndFunctionError, controller.DisplayName);
                        logger?.WriteError(message);
                        Console.WriteLine(message);
                        exitCode = ExitCode.Error;
                    }

                    logger?.Dispose();
                    return;
                }
            }
            catch (Exception ex)
            {
                var message = "Hopperla, da ging etwas nicht so wie es sollte....";
                logger?.WriteError(message);
                Console.WriteLine(message);
                logger?.WriteError(ex.Message);
                Console.WriteLine(ex.Message);
                exitCode = ExitCode.Error;
                logger?.Dispose();
                return;
            }
        }

        /// <summary>
        /// Run Error Case
        /// </summary>
        /// <param name="errors"></param>
        static void HandleParseError(IEnumerable<Error> errors)
        {
            Console.WriteLine(Resources.CMDCallWithError);
            exitCode = ExitCode.Error;
        }

        /// <summary>
        /// Write Welcome Message to console
        /// </summary>
        static void WriteWelcomeMessage()
        {
            Console.WriteLine("========================================================");
            Console.WriteLine(Resources.genModulName);
            Console.WriteLine("Tool zur Konvertierung von Interlis (XTF) Dateien.");
            Console.WriteLine("========================================================");
        }
    }
}