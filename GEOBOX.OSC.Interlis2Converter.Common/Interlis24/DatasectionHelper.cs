using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GEOBOX.OSC.Interlis2Converter.Common.Interlis24
{
    internal class DatasectionHelper
    {
        #region Propertys and Attributs
        /// <summary>
        /// List with all datasections
        /// </summary>
        internal List<XElement> Datasections { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the datasection helper
        /// </summary>
        internal DatasectionHelper() 
        {
            Datasections = new List<XElement>();
        }
        #endregion

        #region Set Infos
        /// <summary>
        /// Add datasection
        /// </summary>
        /// <param name="datasectionContent">Content</param>
        internal void AddDatasection(XElement datasectionContent)
        {
            Datasections.Add(datasectionContent);
        }
        #endregion
    }
}