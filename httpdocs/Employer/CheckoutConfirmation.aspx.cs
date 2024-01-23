using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HristoEvtimov.Websites.Work.Web.Employer
{
    public partial class CheckoutConfirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = GetGlobalResourceObject("PageTitles", "strEmployerCheckoutConfirmation").ToString();
        }
    }
}