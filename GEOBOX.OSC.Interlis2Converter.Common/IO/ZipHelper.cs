using GEOBOX.OSC.Interlis2Converter.Common.Properties;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="extractPath">Optional directory where files should be extracted to</param>
        /// <returns>Full path to the extracted files</returns>
        internal static string ExtractZipFile(string zipPath, string extractPath = null)
        {
            // Verify the zip file exists
            if (!File.Exists(zipPath))
            {
                throw new FileNotFoundException(Resources.ZipHelperFileNotFoundMessage, zipPath);
            }

            // If no extract path is provided, create one in the same directory as the ZIP
            if (string.IsNullOrEmpty(extractPath))
            {
                string zipFileName = Path.GetFileNameWithoutExtension(zipPath);
                extractPath = Path.Combine(Path.GetDirectoryName(zipPath), zipFileName);
            }

            // Create the extraction directory if it doesn't exist
            Directory.CreateDirectory(extractPath);

            try
            {
                // Extract the ZIP file
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                File.Delete(zipPath);
                return extractPath;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(Resources.ZipHelperGeneralExceptionMessage, ex.Message));
            }
        }
    }
}
