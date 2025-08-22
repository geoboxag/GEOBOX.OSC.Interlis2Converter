using System.Xml.Linq;

namespace GEOBOX.OSC.Interlis2Converter.Common.Interlis24
{
    internal class FileWriter
    {
        private XNamespace ili = "http://www.interlis.ch/xtf/2.4/INTERLIS";
        // ToDo Checker is ready for write in all Helpers
        internal InfosHelper InfosHelper { get; private set; }
        internal ModelsHelper ModelsHelper { get; private set; }
        internal NamespaceHelper NamespaceHelper { get; private set; }
        internal DatasectionHelper DatasectionHelper { get; private set; }

        private XDocument xmlWriter;

        public FileWriter(InfosHelper infosHelper, ModelsHelper modelsHelper, NamespaceHelper namespaceHelper, DatasectionHelper datasectionHelper) 
        {
            InfosHelper = infosHelper;
            ModelsHelper = modelsHelper;
            NamespaceHelper = namespaceHelper;
            DatasectionHelper = datasectionHelper;

        }

        public void WriteXTF(string xtfFileToWrite) 
        {

            if (string.IsNullOrEmpty(xtfFileToWrite))
            {
                // ToDo Logger
                throw new ArgumentNullException(xtfFileToWrite);
            }
            if (File.Exists(xtfFileToWrite))
            {
                // ToDo Logger overwrite file
                //throw new FileNotFoundException();
                // ToDo check is file writable
            }

            xmlWriter = new XDocument();

            // 1. Start Tag - Transfer
            // 2. Namespaces
            XElement tranfer = GetTransferElement();
            // 3. Headersection
            // >> Models > Model
            // >> Sender
            tranfer.Add(GetHeaderSection());
            // 4. Datasection
            tranfer.Add(GetDatasection());

            xmlWriter.Add(tranfer);
            xmlWriter.Save(xtfFileToWrite);
        }

        private XElement GetTransferElement()
        {
            XElement transfer = new XElement(ili + "transfer");
            transfer.Add(new XAttribute(XNamespace.Xmlns + "ili", ili));
            // Namespaces as Attributs
            foreach (KeyValuePair<string, string> aliasNamespace in NamespaceHelper.Namespaces)
            {
                transfer.Add(new XAttribute(XNamespace.Xmlns + aliasNamespace.Key, aliasNamespace.Value));
            }
            return transfer;
        }

        private XElement GetHeaderSection()
        {
            XElement headerSection = new XElement(ili + "headersection");

            // Add Models als Node
            XElement models = new XElement(ili + "models");
            foreach (string modelName in ModelsHelper.Models)
            {
                models.Add(new XElement(ili + "model", modelName));
            }
            headerSection.Add(models);

            // Sender
            XElement sender = new XElement(ili + "sender", InfosHelper.Sender);
            headerSection.Add(sender);

            // Comment (add only if there are any)
            XElement comment = new XElement(ili + "comment", InfosHelper.Comment);
            if(comment.HasElements)
                headerSection.Add(comment);

            return headerSection;
        }

        private XElement GetDatasection()
        {
            XElement dataSection = new XElement(ili + "datasection");

            // Add Datasections
            foreach (XElement datasection in DatasectionHelper.Datasections)
            {
                dataSection.Add(datasection);
            }

            return dataSection;
        }

    }
}