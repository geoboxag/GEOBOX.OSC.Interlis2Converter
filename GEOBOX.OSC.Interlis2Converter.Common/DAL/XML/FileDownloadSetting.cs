using System.Xml.Serialization;

namespace GEOBOX.OSC.Interlis2Converter.Common.DAL.XML
{
    [XmlRoot("FileDownloadSetting")]
    public class FileDownloadSetting
    {
        /// <summary>
        /// URL to the zip file containing the .xtf data
        /// </summary>
        [XmlElement("SourceURL")]
        public string SourceURL { get; set; }

        /// <summary>
        /// Name of the data file in the output directory
        /// </summary>
        [XmlElement("FileName")]
        public string FileName { get; set; }
    }
}
