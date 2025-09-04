using GEOBOX.OSC.Common.Logging;
using GEOBOX.OSC.Interlis2Converter.Common.DAL.XML;
using GEOBOX.OSC.Interlis2Converter.Common.Properties;
using System.Xml.Serialization;

namespace GEOBOX.OSC.Interlis2Converter.Common.Settings
{
    /// <summary>
    /// Run Data object for console and ui
    /// </summary>
    public class RuntimeSettings
    {
        /// <summary>
        /// All data is ready for running
        /// </summary>
        private bool isInitOk = false;

        /// <summary>
        /// Command type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Input path with all files for convert
        /// </summary>
        public string InputPath { get; private set; }

        /// <summary>
        /// Output filename with extension
        /// </summary>
        public string OutputFile { get; private set; }

        /// <summary>
        /// Output directory path
        /// </summary>
        public string OutputDir { get; private set; }

        /// <summary>
        /// Path to download config file
        /// </summary>
        public string DownloadConfigFilePath { get; private set; }
        /// <summary>
        /// Settings for Download from services
        /// </summary>
        public DownloadSettings DownloadSettings { get; private set; }

        /// <summary>
        /// Output file can be overwritten without user confirmation 
        /// </summary>
        public bool OutputOverwrite { get; private set; }

        /// <summary>
        /// File Path to Log-File
        /// </summary>
        public string LogFile { get; private set; }

        /// <summary>
        /// Log file can be overwritten without user confirmation 
        /// </summary>
        public bool LogFileOverwrite { get; private set; }

        /// <summary>
        /// Message with result from check for overwrite the output and log-file
        /// </summary>
        public string FileOverwriteMessage { get; private set; }

        /// <summary>
        /// Set command type
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type");
            }

            Type = type;
        }

        /// <summary>
        /// Set and check input folder
        /// </summary>
        /// <param name="inputPath">Path to directory</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void SetInputPath(string inputPath)
        {
            if (!string.IsNullOrEmpty(inputPath) && !Directory.Exists(inputPath))
            {
                throw new DirectoryNotFoundException(string.Format(Resources.DirectoryNotFoundExceptionMessage, inputPath));
            }

            InputPath = inputPath;
        }

        /// <summary>
        /// Set Output Directory
        /// </summary>
        /// <param name="outputDir"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void SetOutputDir(string outputDir)
        {
            if (!Directory.Exists(outputDir))
            {
                throw new DirectoryNotFoundException(string.Format(Resources.DirectoryNotFoundExceptionMessage, outputDir));
            }
            OutputDir = outputDir;
        }

        #region Download Config
        /// <summary>
        /// Set the Download Config file and read it
        /// </summary>
        /// <param name="downloadConfigFilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public void SetAndReadDownloadConfigFile(string downloadConfigFilePath)
        {
            if (!string.IsNullOrEmpty(downloadConfigFilePath))
            {
                if (!File.Exists(downloadConfigFilePath))
                {
                    throw new FileNotFoundException(string.Format(Resources.FileNotFoundExceptionMessage, downloadConfigFilePath));
                }
                else
                {
                    try
                    {
                        DownloadConfigFilePath = downloadConfigFilePath;
                        // read settings
                        XmlSerializer serializer = new(typeof(DownloadSettings));
                        using (FileStream fileStream = new(downloadConfigFilePath, FileMode.Open))
                        {
                            // Deserialize XML to ApplicationSettings object
                            DownloadSettings = (DownloadSettings)serializer.Deserialize(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Resources.GeneralExceptionMessage, ex.Message);
                        isInitOk = false;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Set and check output file
        /// </summary>
        /// <param name="outputFile">Path and File-Name</param>
        /// <param name="canOverwrite">if TRUE overwrite without user confirmation</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void SetOutput(string outputFile, bool canOverwrite)
        {
            if (!string.IsNullOrEmpty(outputFile))
            {
                //throw new ArgumentNullException("outputfile");

                if (CheckIsFileReadOnly(outputFile))
                {
                    throw new Exception(Resources.OutputFileReadOnlyMessage);
                }

                if (!File.Exists(Path.Combine(OutputDir, outputFile)))
                {
                    OutputOverwrite = true;
                }
                else
                {
                    OutputOverwrite = canOverwrite;
                }

            }
            OutputFile = outputFile;
        }

        /// <summary>
        /// Set and check log file
        /// </summary>
        /// <param name="logFile">Path and File-Name</param>
        /// <param name="canOverwrite">if TRUE overwrite without user confirmation</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void SetLogFile(string logFile, bool canOverwrite)
        {
            if (!string.IsNullOrEmpty(logFile))
            {

                if (CheckIsFileReadOnly(logFile))
                {
                    throw new Exception(Resources.LogFileReadOnlyMessage);
                }

                if (!File.Exists(logFile))
                {
                    LogFileOverwrite = true;
                }
                else
                {
                    LogFileOverwrite = canOverwrite;
                }

                if (!Directory.Exists(Path.GetDirectoryName(logFile)))
                {
                    throw new DirectoryNotFoundException(string.Format(Resources.DirectoryNotFoundExceptionMessage, Path.GetDirectoryName(logFile)));
                }

                LogFile = logFile;
            }
        }

        /// <summary>
        /// Check are all output-files confirmed to overwrite
        /// </summary>
        /// <returns></returns>
        public bool FileOverwriteIsConfirmed()
        {
            FileOverwriteMessage = Resources.OverwriteMessageOk;
            if (OutputOverwrite && LogFileOverwrite) return true;

            if (!OutputOverwrite && LogFileOverwrite)
            {
                FileOverwriteMessage = Resources.OverwriteMeassgeOutputFile;
            }
            else if (OutputOverwrite && !LogFileOverwrite)
            {
                FileOverwriteMessage = Resources.OverwriteMessageLogFile;
            }
            else
            {
                FileOverwriteMessage = Resources.OverwriteMessageAllFiles;
            }

            return false;
        }

        /// <summary>
        /// Check are all settings ok, files exists and writable
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public bool CheckSettings(ILogger logger)
        {
            isInitOk = IsInitOk(logger);

            if (!isInitOk)
            {
                logger?.WriteWarning($"{nameof(RuntimeSettings)} ist nicht initialisiert.");
            }
            return isInitOk;
        }

        private bool IsInitOk(ILogger logger)
        {
            if (string.IsNullOrEmpty(InputPath))
            {
                logger?.WriteWarning($"{nameof(InputPath)} enthält keine Angabe.");
                return false;
            }
            if (string.IsNullOrEmpty(OutputFile))
            {
                logger?.WriteWarning($"{nameof(OutputFile)} enthält keine Angabe.");
                return false;
            }
            if (string.IsNullOrEmpty(LogFile))
            {
                logger?.WriteWarning($"{nameof(LogFile)} enthält keine Angabe.");
                return false;
            }
            if (string.IsNullOrEmpty(OutputDir))
            {
                logger?.WriteWarning($"{nameof(OutputDir)} enthält keine Angabe.");
                return false;
            }

            if (!string.IsNullOrEmpty(DownloadConfigFilePath) && DownloadSettings == null)
            {
                logger?.WriteWarning($"{nameof(DownloadConfigFilePath)} ist ungültig. Datei konnte nicht gelesen werden.");
                return false;
            }

            return true;
        }

        private bool CheckIsFileReadOnly(string path)
        {
            if (!File.Exists(path)) return false;

            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.IsReadOnly;
        }
    }
}