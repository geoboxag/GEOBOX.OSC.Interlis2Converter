using GEOBOX.OSC.Common.Logging;
using GEOBOX.OSC.Interlis2Converter.Common.DAL.XML;
using GEOBOX.OSC.Interlis2Converter.Common.IO;
using GEOBOX.OSC.Interlis2Converter.Common.Properties;
using GEOBOX.OSC.Interlis2Converter.Common.Settings;

namespace GEOBOX.OSC.Interlis2Converter.Common.Controllers
{
    public class ServiceDownload : IController
    {
        // DEBUG:
        // --type serviceDownload --outputDir "C:\_daten\xtfDownloads"
        //--logFile "C:\_daten\Interlis\DMAV_alles.log" --configFile "C:\Path\To\Config\LFP2Config.xml"
        #region Propertys and Attributs
        /// <summary>
        /// Type is the key in availlable controller list
        /// </summary>
        private const string TYPE = "serviceDownload";
        /// <summary>
        /// IController - Command Type name
        /// </summary>
        public static string CommandType => TYPE;
        /// <summary>
        /// IController - Name for display and logger
        /// </summary>
        public string DisplayName => String.Format(Resources.ServiceDownloadDisplayName, TYPE);

        /// <summary>
        /// Data object with informations for running this controller
        /// </summary>
        private RuntimeSettings runtimeSettings;

        /// <summary>
        /// Logger instance with customer friendly logger
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for download
        /// </summary>
        /// <param name="data">runtime data with all settings</param>
        public ServiceDownload(RuntimeSettings data, ILogger logger)
        {
            runtimeSettings = data;
            Logger = logger;
        }
        #endregion

        public bool Execute()
        {
            // Check if output Dir exists
            if (!Directory.Exists(runtimeSettings.OutputDir))
            {
                Console.WriteLine(Resources.OutputPathNotFoundMessage);
                Logger.WriteError(Resources.OutputPathNotFoundMessage);
                return false;
            }

            // Download the Files from their URLs
            foreach (FileDownloadSetting fileDownloadSetting in runtimeSettings.DownloadSettings.FileDownloadSettings)
            {
                Console.WriteLine(string.Format(Resources.DownloadFileStartMessage, fileDownloadSetting.FileName));
                Logger.WriteInformation(string.Format(Resources.DownloadFileStartMessage, fileDownloadSetting.FileName));
                // Create Downloaders
                var downloader = new FileDownloader();
                //Add .zip for the download folder that gets deleted later, download needs to target a .zip file
                string tempZIPFilePath = Path.Combine(runtimeSettings.OutputDir, $"{fileDownloadSetting.FileName}.zip");

                // Check if temp zip file exists and delete this
                if (File.Exists(tempZIPFilePath))
                {
                    try
                    {
                        ZipHelper.DeleteZipFile(tempZIPFilePath);

                        Console.WriteLine(string.Format(Resources.ZipFileExistsWasDeletedMessage, tempZIPFilePath));
                        Logger.WriteInformation(string.Format(Resources.ZipFileExistsWasDeletedMessage, tempZIPFilePath));

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format(Resources.ZipFileDeletedErrorMessage, ex.Message));
                        Logger.WriteError(string.Format(Resources.ZipFileDeletedErrorMessage, ex.Message));
                        
                        continue;
                    }
                }

                downloader.DownloadProgressChanged += (sender, progress) =>
                {
                    Console.WriteLine(string.Format(Resources.DownloadProgressMessage, progress.ToString("F1")));
                };

                // Download the file
                FileInfo downloadedFile = null;
                try
                {
                    downloadedFile = downloader.DownloadFileAsync(tempZIPFilePath, fileDownloadSetting.SourceURL).Result;
                    Console.WriteLine(string.Format( Resources.DownloadSucessMessage, downloadedFile.FullName));
                    Logger.WriteInformation(string.Format(Resources.DownloadSucessMessage, downloadedFile.FullName));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(Resources.DownloadFailedMessage,ex.Message));
                    Logger.WriteError(string.Format(Resources.DownloadFailedMessage, ex.Message));

                    continue;
                }

                //unzip all XTF-Files from zip and save in output directory
                try
                {
                    int extractFileCounter = ZipHelper.ExtractXTFFileFromZip(downloadedFile.FullName, runtimeSettings.OutputDir, fileDownloadSetting.FileName, Logger);

                    if (extractFileCounter == 0)
                    {
                        Console.WriteLine(string.Format(Resources.ZipExtractCountNullCountMessage, downloadedFile.FullName));
                        Logger.WriteWarning(string.Format(Resources.ZipExtractCountNullCountMessage, downloadedFile.FullName));
                    }
                    else
                    {
                        Console.WriteLine(string.Format(Resources.ZipExtractCountSuccesMessage, downloadedFile.FullName, extractFileCounter));
                        Logger.WriteInformation(string.Format(Resources.ZipExtractCountSuccesMessage, downloadedFile.FullName, extractFileCounter));
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteError(string.Format(Resources.ZipGeneralExceptionMessage, ex.Message));
                    continue;
                }

                // delete zip file from download
                try
                {
                    ZipHelper.DeleteZipFile(tempZIPFilePath);

                    Console.WriteLine(string.Format(Resources.ZipFileDeleteMessage, tempZIPFilePath));
                    Logger.WriteInformation(string.Format(Resources.ZipFileDeleteMessage, tempZIPFilePath));
                    Logger.WriteLine(" ");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(Resources.ZipFileDeletedErrorMessage, ex.Message));
                    Logger.WriteError(string.Format(Resources.ZipFileDeletedErrorMessage, ex.Message));

                    continue;
                }
                
            }

            return true;
        }

        /// <summary>
        /// Check if all command line options are valid for this command
        /// </summary>
        /// <returns></returns>
        public bool CheckCommandlineOptions()
        {
            if(string.IsNullOrEmpty(runtimeSettings.OutputDir) || string.IsNullOrEmpty(runtimeSettings.DownloadConfigFilePath))
            {
                Console.WriteLine(Resources.DownloadMissingCMDOptionsMessage);
                return false;
            }
            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            //  uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}