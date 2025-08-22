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
        [Option('t', "type", HelpText = "Angabe des Types für die Konvertierung die Durchgeführt werden soll.", Required = true)]
        public string Type { get; set; }

        /// <summary>
        /// The root directory in which the files are to be created
        /// </summary>
        [Option('i', "inputDir", HelpText = "Angabe des Verzeichnispfades mit den Interlis (XTF) Dateien zum Zusammenführen.")]
        public string InputDir { get; set; }

        /// <summary>
        /// Name of the file without file extension
        /// </summary>
        [Option('o', "outputFile", HelpText = "Angabe des Datei-Namens (inkl. Dateiendung) für das Zusammengeführte Interlis (XTF-Datei).")]
        public string OutputFile { get; set; }
        
        /// <summary>
        /// Name of the folder
        /// </summary>
        [Option('d', "outputDir", HelpText = "Angabe des Datei-Pfades (Ordner) für das Zusammengeführte Interlis (XTF-Datei).", Required=true)]
        public string OutputDir { get; set; }
        
        /// <summary>
        /// Download config file name and path
        /// </summary>
        [Option('c', "downloadConfig", HelpText = "Angabe zur Konfigurationsdatei-Pfades für den Service-Daten-Download.")]
        public string DownloadConfig { get; set; }

        /// <summary>
        /// Full path and filename without file extension
        /// </summary>
        [Option('l', "logFile", HelpText = "Angabe des Pfades und des Datei-Namens (inkl. Dateiendung) für die Protokoll-Datei.")]
        public string LogFile { get; set; }
    }
}
