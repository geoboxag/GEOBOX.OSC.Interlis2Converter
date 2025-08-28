using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace GEOBOX.OSC.Interlis2Converter.Common.Interlis24
{
    internal class FileReader
    {
        internal InfosHelper InfosHelper { get; private set; }

        internal ModelsHelper ModelsHelper { get; private set; }

        internal NamespaceHelper NamespaceHelper { get; private set; }

        internal DatasectionHelper DatasectionHelper { get; private set; }

        internal bool SetModelAsXTFNamespace { get; set; } = false;

        /// <summary>
        /// Constructor init file reader
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        internal FileReader() 
        {
            InfosHelper = new InfosHelper();
            ModelsHelper = new ModelsHelper();
            NamespaceHelper = new NamespaceHelper();
            DatasectionHelper = new DatasectionHelper();
        }

        /// <summary>
        /// Read XTF File and collect infos for writing
        /// </summary>
        /// <param name="xtfFileToRead">Path to file and file name with extension e.g. C:\Temp\AVBB.xtf</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        internal void ReadXTF(string xtfFileToRead)
        {
            if (string.IsNullOrEmpty(xtfFileToRead))
            {
                // ToDo Logger
                throw new ArgumentNullException(xtfFileToRead);
            }
            if (!File.Exists(xtfFileToRead))
            {
                // ToDo Logger
                throw new FileNotFoundException();
            }

            try
            {
                using (var xmlReader = XmlReader.Create(xtfFileToRead))
                {
                    if (xmlReader == null)
                    {
                        // ToDo Logger
                        return;
                    }

                    XDocument xDocument = XDocument.Load(xmlReader);
                    
                    // Transfer and read namespaces
                    var transfers = xDocument.Descendants(XName.Get("transfer", "http://www.interlis.ch/xtf/2.4/INTERLIS"));
                    foreach (var transferAttribut in transfers.Attributes())
                    {
                        if (transferAttribut.Name.LocalName != "xmlns")
                        {
                            NamespaceHelper.AddNamespace(transferAttribut.Name.LocalName, transferAttribut.Value);
                        }
                    }

                    // Models
                    var models = xDocument.Descendants(XName.Get("model", "http://www.interlis.ch/xtf/2.4/INTERLIS"));
                    foreach (var model in models)
                    {
                        ModelsHelper.AddModel(model.Value);
                        if (SetModelAsXTFNamespace)
                        {
                            NamespaceHelper.AddStandardXTFNamespace(model.Value);
                        }
                    }

                    // Sender
                    var senders = xDocument.Descendants(XName.Get("sender", "http://www.interlis.ch/xtf/2.4/INTERLIS"));
                    foreach (var sender in senders)
                    {
                        if (String.IsNullOrEmpty(sender.Value)) continue;
                        // First value will set (first wins)
                        InfosHelper.SetSenderIsNotSet(sender.Value);
                    }

                    // Comment
                    var comments = xDocument.Descendants(XName.Get("comment", "http://www.interlis.ch/xtf/2.4/INTERLIS"));
                    foreach (var comment in comments)
                    {
                        if (String.IsNullOrEmpty(comment.Value)) continue;
                            InfosHelper.AppendComment(comment.Value);
                    }

                    // Datasection
                    var datasection = xDocument.Descendants(XName.Get("datasection", "http://www.interlis.ch/xtf/2.4/INTERLIS")).Where(node => !String.IsNullOrEmpty(node.Value));
                    // "Remove" top Node "datasection"
                    //var decendantAsNodes = datasection.First().Nodes();
                    foreach(XElement dataSectionNode in datasection.Elements())
                    {
                        DatasectionHelper.AddDatasection(dataSectionNode);
                    }

                } // End USING XMLReader
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
                // ToDo logger
                //logger.Warning(String.Format(Resources.loggerErrorWhileReadingFile, fileName));
            }
        }
    }
}