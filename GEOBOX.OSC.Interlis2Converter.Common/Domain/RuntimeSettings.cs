using GEOBOX.OSC.Common.Logging;
using GEOBOX.OSC.Interlis2Converter.Common.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOBOX.OSC.Interlis2Converter.Common.Domain
{
    /// <summary>
    /// Run Data object for console and ui
    /// </summary>
    public class RuntimeSettings
    {
        /// <summary>
        /// All Datas are ready for run
        /// </summary>
        private bool isInitOk = false;

        /// <summary>
        /// Command type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Input Path with all Files for Convert
        /// </summary>
        public string InputPath { get; private set; }

        /// <summary>
        /// Output Path with Filename
        /// </summary>
        public string OutputFile { get; private set; }

        /// <summary>
        /// Output File can be overwriten without user confirmation 
        /// </summary>
        public bool OutputOverwrite { get; private set; }

        /// <summary>
        /// File Path to Log-File
        /// </summary>
        public string LogFile { get; private set; }

        /// <summary>
        /// Log File can be overwriten without user confirmation 
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
            if (string.IsNullOrEmpty(inputPath))
            {
                throw new ArgumentNullException("inputpath");
            }
            if (!Directory.Exists(inputPath))
            {
                throw new DirectoryNotFoundException(String.Format(Resources.DirectoryNotFoundExceptionMessage, inputPath));
            }

            InputPath = inputPath;
        }

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
            if (string.IsNullOrEmpty(outputFile))
            {
                throw new ArgumentNullException("outputfile");
            }
            if (CheckIsFileReadOnly(outputFile))
            {
                throw new Exception(Resources.OutputFileReadOnlyMessage);
            }

            if (!File.Exists(outputFile))
            {
                OutputOverwrite = true;
            }
            else
            {
                OutputOverwrite = canOverwrite;
            }

            if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
            {
                throw new DirectoryNotFoundException(String.Format(Resources.DirectoryNotFoundExceptionMessage, Path.GetDirectoryName(outputFile)));
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
            if (string.IsNullOrEmpty(logFile))
            {
                throw new ArgumentNullException("logFile");
            }
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
                throw new DirectoryNotFoundException(String.Format(Resources.DirectoryNotFoundExceptionMessage, Path.GetDirectoryName(logFile)));
            }

            LogFile = logFile;
        }

        /// <summary>
        /// Check are all output-files confirmed for overwrite
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
        /// Check ar all settings ok, files exists and writable
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public bool CheckSettings(ILogger logger)
        {
            bool isInitOk = IsInitOk(logger);

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
                isInitOk = false;
                logger?.WriteWarning($"{nameof(InputPath)} enthält keine Angabe.");
                return isInitOk;
            }
            if (string.IsNullOrEmpty(OutputFile))
            {
                isInitOk = false;
                logger?.WriteWarning($"{nameof(OutputFile)} enthält keine Angabe.");
                return isInitOk;
            }
            if (string.IsNullOrEmpty(LogFile))
            {
                isInitOk = false;
                logger?.WriteWarning($"{nameof(LogFile)} enthält keine Angabe.");
                return isInitOk;
            }
            
            isInitOk = true;
            return isInitOk;
        }

        private bool CheckIsFileReadOnly(string path)
        {
            if (!File.Exists(path)) return false;

            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.IsReadOnly;
        }
    }
}