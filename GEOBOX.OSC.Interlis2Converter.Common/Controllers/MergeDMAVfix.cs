using GEOBOX.OSC.Common.Logging;
using GEOBOX.OSC.Interlis2Converter.Common.Interlis24;
using GEOBOX.OSC.Interlis2Converter.Common.Properties;
using GEOBOX.OSC.Interlis2Converter.Common.Settings;
using System.Diagnostics.CodeAnalysis;

namespace GEOBOX.OSC.Interlis2Converter.Common.Controllers
{
    public class MergeDMAVfix : IController
    {
        // DEBUG:
        // --type mergeDMAVfix --inputDir "C:\_daten\Interlis" --outputFile "C:\_daten\Interlis\DMAV_alles.xtf" --logFile "C:\_daten\Interlis\DMAV_alles.log"

        #region Propertys and Attributs
        /// <summary>
        /// Type is the key in availlable controller list
        /// </summary>
        private const string TYPE = "mergeDMAVfix";
        /// <summary>
        /// IController - Command Type name
        /// </summary>
        public static string CommandType => TYPE; 
        /// <summary>
        /// IController - Name for display and looger
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
        public MergeDMAVfix(RuntimeSettings data, ILogger logger)
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
            var filesToRead = GetDMAVFilesToReadInCorrectOrder(runtimeSettings.InputPath);
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
        /// File Names in correct order to read
        /// </summary>
        private List<string> GetDMAVFilesToReadInCorrectOrder(string sourcePath)
        {
            List<string> dmavFilesToReadInCorrectOrder = new List<string>()
            {
                "DMAV_Bodenbedeckung.xtf",
                "DMAV_DauerndeBodenverschiebungen.xtf",
                "DMAV_Dienstbarkeitsgrenzen.xtf",
                "DMAV_Einzelobjekte.xtf",
                "KGK_PFDS2.xtf",
                //"DMAV_FixpunkteAVKategorie2.xtf", removed from swisstopo in version 01.05.2025
                "DMAV_FixpunkteAVKategorie3.xtf",
                "FixpunkteLV_LFP.xtf", "FixpunkteLV_HFP.xtf",
                // "DMAV_FixpunkteLV.xtf", removed from swisstopo in version 01.05.2025
                "DMAV_Gebäudeadressen.xtf", "DMAV_Gebaeudeadressen.xtf",
                "DMAV_Grundstücke.xtf", "DMAV_Grundstuecke.xtf",
                "DMAV_HoheitsgrenzenAV.xtf",
                "HoheitsgrenzenLV.xtf",
                //"DMAV_HoheitsgrenzenLV.xtf", removed from swisstopo in version 01.05.2025
                "DMAV_Nomenklatur.xtf",
                "OrtschaftsverzeichnisPLZ.xtf",
                //"DMAV_PLZ_Ortschaft.xtf", removed from swisstopo in version 01.05.2025
                "DMAV_Rohrleitungen.xtf",
                "DMAV_Toleranzstufen.xtf",
                "DMAVSUP_UntereinheitGrundbuch.xtf"
            };

            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentNullException(sourcePath);
            }
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException(sourcePath);
            }

            var fileList = new List<string>();

            foreach (string fileName in dmavFilesToReadInCorrectOrder)
            {
                fileList.Add(Path.Combine(sourcePath, fileName));
            }

            return fileList;

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