using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace HristoEvtimov.Websites.Work.WorkLibrary.UserControls
{
    public class GeneralMasterPageBase : MasterPage
    {
        public enum SystemMessageDisplayTimes { Now, NextLoad };
        public enum SystemMessageTypes { Error, OK, Warning, Info };

        public delegate void SystemMessageEventHandler(object sender, SystemMessageEventArgs e);
        public event SystemMessageEventHandler SystemMessage;
        protected virtual void OnSystemMessage(SystemMessageEventArgs e)
        {
            if (SystemMessage != null)
                SystemMessage(this, e);
        }

        public void AddSystemMessage(string systemMessage, SystemMessageTypes systemMessageType, SystemMessageDisplayTimes systemMessageDisplayTime)
        {
            OnSystemMessage(new SystemMessageEventArgs(systemMessage, systemMessageType, systemMessageDisplayTime));
        }

        public class SystemMessageEventArgs
        {
            public SystemMessageEventArgs(string systemMessage, SystemMessageTypes systemMessageType, SystemMessageDisplayTimes systemMessageDisplayTime)
            {
                SystemMessage = systemMessage;
                SystemMessageType = systemMessageType;
                SystemMessageDisplayTime = systemMessageDisplayTime;
            }

            public string SystemMessage { get; set; }
            public SystemMessageTypes SystemMessageType { get; set; }
            public SystemMessageDisplayTimes SystemMessageDisplayTime { get; set; }
        }
    }
}
