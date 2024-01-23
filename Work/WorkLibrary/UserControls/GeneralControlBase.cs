using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary.UserControls
{
    public class GeneralControlBase : UserControl 
    {
        protected void AddSystemMessage(string systemMessage, GeneralMasterPageBase.SystemMessageTypes systemMessageType,
            GeneralMasterPageBase.SystemMessageDisplayTimes systemMessageDisplayTime)
        {
            //try to find the message display control on the master page
            //and set its display text right there.
            GeneralMasterPageBase masterPage = (GeneralMasterPageBase)Page.Master;
            if (masterPage != null)
            {
                masterPage.AddSystemMessage(systemMessage, systemMessageType, systemMessageDisplayTime);
            }
        }

        public User GetUser()
        {
            UserManager userManager = new UserManager();
            return userManager.GetUser();
        }

        public void RedirectToHomeAndError(string globalResourceKey)
        {
            if (!String.IsNullOrEmpty(globalResourceKey))
            {
                this.AddSystemMessage(GetGlobalResourceObject("GlobalResources", globalResourceKey).ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
            }
            Response.Redirect("/");
        }
    }
}
