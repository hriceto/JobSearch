using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class shoppingcartlink : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PreRender +=new EventHandler(shoppingcartlink_PreRender);
        }

        protected void shoppingcartlink_PreRender(object sender, EventArgs e)
        {
            this.Visible = true;

            JobManager jobManager = new JobManager();
            int count = jobManager.GetShoppingCartCountForDisplay();
            if (count >= 0)
            {
                hplnkShoppingCart.Text = String.Format(GetLocalResourceObject("strShoppingCart").ToString(), count);
                UrlManager urlManager = new UrlManager();
                hplnkShoppingCart.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/Checkout.aspx", null);
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}