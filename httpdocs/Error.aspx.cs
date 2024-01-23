using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace HristoEvtimov.Websites.Work.Web
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = GetGlobalResourceObject("PageTitles", "strError").ToString();
            lblHeader2.Text = String.Format(GetLocalResourceObject("lblHeader2").ToString(), WebConfigurationManager.AppSettings["CUSTOMER_SERVICE_EMAIL"]);
        }
    }
}