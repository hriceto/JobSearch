using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class checkoutconfirmation : GeneralControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["orderid"] != null)
            {
                Checkout checkout = new Checkout();
                User currentUser = GetUser();

                int orderId = Int32.Parse(Request.QueryString["orderid"].ToString());
                Order order = checkout.GetOrder(currentUser.UserId, orderId);

                if (order == null)
                {
                    return;
                }

                if (order.OrderComplete)
                {
                    litConfirmation.Text = String.Format(GetLocalResourceObject("strConfirmation").ToString(), order.OrderId);
                    if (order.Total > 0)
                    {
                        litConfirmationPaid.Text = String.Format(GetLocalResourceObject("strConfirmationPaid").ToString(), order.GWAuthorizationCode);
                    }
                    litConfirmationJobs.Text += GetLocalResourceObject("strConfirmationJobs").ToString();
                }
            }
        }
    }
}