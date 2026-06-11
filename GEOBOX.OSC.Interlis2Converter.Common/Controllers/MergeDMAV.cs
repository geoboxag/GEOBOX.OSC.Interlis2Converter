using GEOBOX.OSC.Common.Logging;
using GEOBOX.OSC.Interlis2Converter.Common.Interlis24;
using GEOBOX.OSC.Interlis2Converter.Common.Properties;
using GEOBOX.OSC.Interlis2Converter.Common.Settings;
using System.Diagnostics.CodeAnalysis;

namespace GEOBOX.OSC.Interlis2Converter.Common.Controllers
{
    public class MergeDMAV : IController
    {
        // DEBUG:
        // --type mergeDMAV --inputDir "C:\_daten\Interlis" --outputFile "C:\_daten\Interlis\DMAV_alles.xtf" --logFile "C:\_daten\Interlis\DMAV_alles.log"

        #region Propertys and Attributs
        /// <summary>
        /// Type is the key in available controller list
        /// </summary>
        private const string TYPE = "mergeDMAV";
        /// <summary>
        /// IController - Command Type name
        /// </summary>
        public static string CommandType => TYPE; 
        /// <summary>
        /// IController - Name for display and logger
        /// </summary>
        public string DisplayName => String.Format(Resources.MergeDMAVDisplayName, TYPE);

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
        /// Constructor for convert
        /// </summary>
        /// <param name="data">runtime data with all settings</param>
        public MergeDMAV(RuntimeSettings data, ILogger logger)
        {
            runtimeSettings = data;
            Logger = logger;
        }
        #endregion

        #region Execute
        /// <summary>
        /// Run Command
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            if (!runtimeSettings.CheckSettings(Logger))
            {
                return false;
            }

            // 1. Read the XTF files
            var filesToRead = GetDMAVFilesToReadByModelInCorrectOrder(runtimeSettings.InputPath);
            // ToDo add Logger to file reader and log message during read files

            // 2. Read Namespaces, Models, Datasection from Interlis an Collect the data
            FileReader fileReader = new FileReader();
            fileReader.SetModelAsXTFNamespace = true;
            foreach(string fileToRead in filesToRead)
            {
                try
                {
                    fileReader.ReadXTF(fileToRead);
                }
                catch
                {
                    continue;
                }
            }

            // 3. Write new XTF
            FileWriter fileWriter = new FileWriter(fileReader.InfosHelper, fileReader.ModelsHelper, fileReader.NamespaceHelper, fileReader.DatasectionHelper);
            fileWriter.WriteXTF(Path.Combine(runtimeSettings.OutputDir, runtimeSettings.OutputFile));

            return true;
        }
        #endregion

        public bool CheckCommandlineOptions()
        {
            if (string.IsNullOrEmpty(runtimeSettings.InputPath)|| string.IsNullOrEmpty(runtimeSettings.OutputFile)|| string.IsNullOrEmpty(runtimeSettings.OutputDir))
            {
                Console.WriteLine(Resources.DMAVMergeMissingCMDOptionsMessage);
                return false;
            }
            return true;
        }

        #region Create and Get Files for Read
        /// <summary>
        /// Exising File Names in correct order, selected by allowed model list
        /// </summary>
        private List<string> GetDMAVFilesToReadByModelInCorrectOrder(string sourcePath)
        {
            List<string> allowedModelNamesInCorrectOrder = new List<string>() 
            {
                "DMAV_Bodenbedeckung_V1_1",
                "DMAV_DauerndeBodenverschiebungen_V1_1",
                "DMAV_Dienstbarkeitsgrenzen_V1_1",
                "DMAV_Einzelobjekte_V1_1",
                "DMAV_FixpunkteAVKategorie3_V1_1",
                "DMAV_Gebaeudeadressen_V1_1",
                "DMAV_Grundstuecke_V1_1",
                "DMAV_HoheitsgrenzenAV_V1_0",
                "DMAV_Nomenklatur_V1_1",
                "DMAV_Rohrleitungen_V1_1",
                "DMAV_Toleranzstufen_V1_1",
                "DMAVSUP_UntereinheitGrundbuch_V1_1",
                "FixpunkteLV_V1_0",
                "KGKCGC_FPDS2_V1_1",
                "HoheitsgrenzenLV_V1_0",
                "OfficialIndexOfLocalities_V1_0"
            };

            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentNullException(sourcePath);
            }
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException(sourcePath);
            }

            var fileReader = new FileReader();

            return fileReader.FindXtfFilesByModelinCorrectOrder(sourcePath, allowedModelNamesInCorrectOrder);
        }

        #endregion

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