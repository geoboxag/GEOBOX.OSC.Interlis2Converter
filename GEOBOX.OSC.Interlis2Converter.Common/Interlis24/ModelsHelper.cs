using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GEOBOX.OSC.Interlis2Converter.Common.Interlis24
{
    internal class ModelsHelper
    {
        #region Propertys and Attributs
        /// <summary>
        /// List with all models
        /// </summary>
        internal List<string> Models { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the model helper
        /// </summary>
        internal ModelsHelper()
        {
            Models = new List<string>();
        }
        #endregion

        #region Set Infos
        /// <summary>
        /// Add model name if not exists in list
        /// </summary>
        /// <param name="model">Model Name</param>
        internal void AddModel(string model)
        {
            if (Models.Contains(model)) return;
            Models.Add(model);
        }
        #endregion
    }
}