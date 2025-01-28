using GEOBOX.OSC.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEOBOX.OSC.Interlis2Converter.Common.Interlis24
{
    internal class InfosHelper
    {
        #region Propertys and Attributs
        /// <summary>
        /// Sender for Headersections
        /// </summary>
        private string sender;
        /// <summary>
        /// Get the sender (is not set get Toolsname as default sender)
        /// </summary>
        internal string Sender {
            get
            {
                if (string.IsNullOrEmpty(sender)) return "GEOBOX - Interlis2 Konverter";
                return sender;
            }
        }

        /// <summary>
        /// Comment for Headersections
        /// </summary>
        private string comment;
        /// <summary>
        /// Get the comment
        /// </summary>
        internal string Comment
        {
            get
            {
                return comment;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for FileReder
        /// </summary>
        internal InfosHelper(string sender)
        {
            SetSenderIsNotSet(sender);
        }
        #endregion

        #region Set Infos
        /// <summary>
        /// set the sender
        /// the first sender will be set
        /// </summary>
        /// <param name="sender">sender text from XTF</param>
        internal void SetSenderIsNotSet(string senderText)
        {
            if (string.IsNullOrEmpty(senderText)) return;
            sender = senderText;
        }

        internal void AppendComment(string commentToAppend)
        {
            if (string.IsNullOrEmpty(commentToAppend)) return;

            if (string.IsNullOrEmpty(comment))
            {
                comment = commentToAppend;
                return;
            }

            comment = comment + Environment.NewLine + commentToAppend;
        }
        #endregion
    }
}