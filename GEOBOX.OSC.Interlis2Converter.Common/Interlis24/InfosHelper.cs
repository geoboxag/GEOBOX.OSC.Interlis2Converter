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
        /// Check is sender set
        /// </summary>
        private bool isSenderSet = false;
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
        internal InfosHelper()
        {
        }
        #endregion

        #region Set Infos
        /// <summary>
        /// set the sender
        /// first value will be set (first wins)
        /// </summary>
        /// <param name="sender">sender text from XTF</param>
        internal void SetSenderIsNotSet(string senderText)
        {
            if (isSenderSet) return;
            if (string.IsNullOrEmpty(senderText)) return;

            isSenderSet = true;
            sender = senderText;
        }

        /// <summary>
        /// Append comment
        /// </summary>
        /// <param name="commentToAppend"></param>
        internal void AppendComment(string commentToAppend)
        {
            if (string.IsNullOrEmpty(commentToAppend)) return;

            if (string.IsNullOrEmpty(comment))
            {
                comment = commentToAppend;
                return;
            }

            // seperated by semicolon (not with Environment.NewLine)
            comment = comment + "; " + commentToAppend;
        }
        #endregion
    }
}