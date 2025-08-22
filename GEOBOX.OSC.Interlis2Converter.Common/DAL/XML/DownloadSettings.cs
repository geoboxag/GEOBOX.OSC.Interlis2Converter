using System.Xml.Serialization;

namespace GEOBOX.OSC.Interlis2Converter.Common.DAL.XML
{
    [XmlRoot("DownloadSettings")]
    public class DownloadSettings
    {
        /// <summary>
        /// Dataset list containing infos the data (source and name)
        /// </summary>
        [XmlArray("FileDownloadSettings")]
        [XmlArrayItem("FileDownloadSetting")]
        public List<FileDownloadSetting> FileDownloadSettings { get; set; }


    }
}
