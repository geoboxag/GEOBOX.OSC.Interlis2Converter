using GEOBOX.OSC.Common.Logging;
using GEOBOX.OSC.Interlis2Converter.Common.Properties;
using System.Diagnostics;
using System.IO.Compression;
namespace GEOBOX.OSC.Interlis2Converter.Common.IO
{
    /// <summary>
    /// Helper for ZIP Files (extract and compress)
    /// </summary>
    internal static class ZipHelper
    {
        /// <summary>
        /// Extracts a ZIP file to a specified destination directory and returns the path to the extracted files
        /// </summary>
        /// <param name="zipPath">Full path to the ZIP file</param>
        /// <param name="outputFolder">directory where files should be extracted to</param>
        /// <param name="xtfFileSaveName">new filename for xtf file</param>
        /// <param name="logger">logger for messages</param>
        /// <returns>count of extracted files</returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal static int ExtractXTFFileFromZip(string zipPath, string outputFolder, string xtfFileSaveName, ILogger logger)
        {
            // Verify the zip file exists
            if (!File.Exists(zipPath))
            {
                throw new FileNotFoundException(Resources.ZipHelperFileNotFoundMessage, zipPath);
            }
            // Verify the output folder exists
            if (!Directory.Exists(outputFolder))
            {
                throw new DirectoryNotFoundException();
            }
            // Verifiy the file name
            if (string.IsNullOrEmpty(xtfFileSaveName))
            {
                throw new ArgumentNullException(xtfFileSaveName);
            }

            try
            {
                int counter = 0;

                // Instead of using ZipFile.ExtractToDirectory, use ZipArchive to filter files
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        // Only extract files ending with .xtf
                        if (entry.Name.EndsWith(".xtf", StringComparison.OrdinalIgnoreCase))
                        {
                            string fileName = xtfFileSaveName;

                            if (counter >= 1) { 
                                string dateTime = DateTime.Now.ToString("yyyyMMdd-HHmmssfff");
                                fileName = $"{dateTime}_{xtfFileSaveName}"; 
                            }
                            
                            // Create full path for the extraction
                            string destinationPath = Path.Combine(outputFolder, fileName);

                            // Extract the file
                            entry.ExtractToFile(destinationPath, true);
                            logger.WriteInformation(string.Format(Resources.ZipExtractSuccesMessage, fileName, zipPath));

                            counter++;
                        }
                    }
                }
                return counter;
            }
            catch
            {
                // re-throw
                throw;
            }
        }

        /// <summary>
        /// Delete zip File in folder
        /// </summary>
        /// <param name="zipFilePath">Full path to ZIP file (e.g C:\Temp\interlis.zip)</param>
        /// <returns>true file are deleted</returns>
        /// <exception cref="ArgumentNullException">zipFilepath is null or empty</exception>
        /// <exception cref="FileNotFoundException">file does not exist</exception>
        internal static bool DeleteZipFile(string zipFilePath)
        {
            if (string.IsNullOrEmpty (zipFilePath)) 
            {
                throw new ArgumentNullException(zipFilePath);
            }

            // Verify the zip file exists
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException(Resources.ZipHelperFileNotFoundMessage, zipFilePath);
            }
            
            File.Delete(zipFilePath);
            return true;
        }
    }
}
