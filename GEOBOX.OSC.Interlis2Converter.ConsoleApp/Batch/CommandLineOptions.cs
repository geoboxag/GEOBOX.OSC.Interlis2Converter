using CommandLine;

namespace GEOBOX.OSC.Interlis2Converter.ConsoleApp.Batch
{
    /// <summary>
    /// Contains the properties of the command line arguments
    /// </summary>
    public sealed class CommandLineOptions
    {
        /// <summary>
        /// Type of function to be executed
        /// </summary>
        [Option("type", HelpText = "Angabe des Types für die Konvertierung die Durchgeführt werden soll.", Required = true)]
        public string Type { get; set; }

        /// <summary>
        /// The root directory in which the files are to be created
        /// </summary>
        [Option("inputDir", HelpText = "Angabe des Verzeichnispfades mit den Interlis (XTF) Dateien zum Zusammenführen.", Required = true)]
        public string InputDir { get; set; }

        /// <summary>
        /// Name of the file without file extension
        /// </summary>
        [Option("outputFile", HelpText = "Angabe des Pfades und des Datei-Namens (inkl. Dateiendung) für das Zusammengeführte Interlis (XTF-Datei).", Required = true)]
        public string OutputFile { get; set; }

        /// <summary>
        /// Full path and filename without file extension
        /// </summary>
        [Option("logFile", HelpText = "Angabe des Pfades und des Datei-Namens (inkl. Dateiendung) für die Protokoll-Datei.", Required = true)]
        public string LogFile { get; set; }
    }
}
