using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class systemmessage : System.Web.UI.UserControl
    {
        private GeneralMasterPageBase.SystemMessageEventArgs SessionSystemMessage
        {
            get
            {
                if (Session["SessionSystemMessage"] != null)
                {
                    return (GeneralMasterPageBase.SystemMessageEventArgs)Session["SessionSystemMessage"];
                }
                return null;
            }
            set { Session["SessionSystemMessage"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            GeneralMasterPageBase masterPage = (GeneralMasterPageBase)Page.Master;
            if (masterPage != null)
            {
                masterPage.SystemMessage += new GeneralMasterPageBase.SystemMessageEventHandler(masterPage_SystemMessage);
            }
            pnlMessage.Visible = false;
            if (SessionSystemMessage != null)
            {
                DisplaySystemMessage(SessionSystemMessage.SystemMessage, SessionSystemMessage.SystemMessageType);
                SessionSystemMessage = null;
            }
        }

        protected void masterPage_SystemMessage(object sender, GeneralMasterPageBase.SystemMessageEventArgs e)
        {
            //add literal to current page
            if (e.SystemMessageDisplayTime == GeneralMasterPageBase.SystemMessageDisplayTimes.Now)
            {
                DisplaySystemMessage(e.SystemMessage, e.SystemMessageType);
            }
            else
            {
                //save message in session to be displyed on next page_load
                SessionSystemMessage = e;
            }
        }

        private void DisplaySystemMessage(string systemMessage, GeneralMasterPageBase.SystemMessageTypes displayType)
        {
            HtmlGenericControl divMessage = new HtmlGenericControl("div");
            string alertClass = "";
            if (displayType == GeneralMasterPageBase.SystemMessageTypes.Error)
            {
                alertClass = "alert-danger";
            }
            else if (displayType == GeneralMasterPageBase.SystemMessageTypes.OK)
            {
                alertClass = "alert-success";
            }
            else if (displayType == GeneralMasterPageBase.SystemMessageTypes.Warning)
            {
                alertClass = "alert-warning";
            }
            else if (displayType == GeneralMasterPageBase.SystemMessageTypes.Info)
            {
                alertClass = "alert-info";
            }
                        
            divMessage.Attributes.Add("class", "alert " + alertClass +" alert-dismissable");

            //set message in literal
            Label ltrSystemMessage = new Label();
            ltrSystemMessage.ID = "ltrSystemMessage";
            ltrSystemMessage.Text = "<strong>" + systemMessage + "</strong>";
            ltrSystemMessage.CssClass = displayType.ToString();

            Literal litCloseButton = new Literal();
            litCloseButton.Text = "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>";

            divMessage.Controls.Add(litCloseButton);
            divMessage.Controls.Add(ltrSystemMessage);

            pnlMessage.Visible = true;
            pnlMessage.Controls.Add(divMessage);
        }
    }
}