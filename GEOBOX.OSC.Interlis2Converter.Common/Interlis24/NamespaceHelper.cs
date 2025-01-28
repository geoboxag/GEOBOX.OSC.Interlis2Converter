using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GEOBOX.OSC.Interlis2Converter.Common.Interlis24
{
    internal class NamespaceHelper
    {
        #region Propertys and Attributs
        /// <summary>
        /// Alias is the LocalName in XML; Namespace is the Value in XML
        /// </summary>
        internal Dictionary<string /* Alias */, string /* Namespace */> Namespaces { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the namespace helper
        /// </summary>
        internal NamespaceHelper()
        {
            Namespaces = new Dictionary<string, string>();
        }
        #endregion

        #region Set or Get Infos
        /// <summary>
        /// Add namespace if alias not exists
        /// </summary>
        /// <param name="nsAlias">XML Namespace Alias</param>
        /// <param name="nsValue">XML Namespace</param>
        internal void AddNamespace(string nsAlias, string nsValue)
        {
            if (Namespaces.ContainsKey(nsAlias)) return;
            if (nsAlias == "ili") return;

            Namespaces[nsAlias] = nsValue;
        }
        #endregion
    }
}