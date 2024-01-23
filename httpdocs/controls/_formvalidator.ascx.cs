using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class _formvalidator : GeneralControlBase, IFormValidation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public bool ValidateForm()
        {
            bool isValid = Page.IsValid;

            //use one hidden and one absolute posiitoned element outside of the visible area of the page 
            //to verify against bots. Bots will most likely get tricked into entering information in those fields
            if (!String.IsNullOrEmpty(txtEmailVerification.Text) || !String.IsNullOrEmpty(txtEmailVerification2.Text))
            {
                //don't prevent bots. I mightbe having an issue with real users fillin out form.
                //isValid = false;
                try
                {
                    LogManager logManager = new LogManager();
                    logManager.AddLog("Possible submit of form by a bot.", 0, this.Parent.ID, "");
                }
                catch { }
            }
            return isValid;
        }
    }
}