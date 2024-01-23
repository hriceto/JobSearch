using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class _shoppingcart : System.Web.UI.UserControl
    {
        public delegate void DeleteJobEventHandler(object sender, DeleteJobEventArgs e);
        public event DeleteJobEventHandler DeleteJob;
        protected virtual void OnDeleteJob(DeleteJobEventArgs e)
        {
            if (DeleteJob != null)
                DeleteJob(this, e);
        }
        public class DeleteJobEventArgs
        {
            public int JobPostId { get; set; }

            public DeleteJobEventArgs(int jobPostId)
            {
                JobPostId = jobPostId;
            }
        }
        UrlManager urlManager = new UrlManager();
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadCart(List<JobPost> shoppingCartJobs, Decimal subtotal, Decimal tax, Decimal total, Decimal couponDiscount)
        {
            if (shoppingCartJobs.Count == 0)
            {
                rptrShoppingCart.Visible = false;
                return;
            }

            rptrShoppingCart.DataSource = shoppingCartJobs;
            rptrShoppingCart.DataBind();

            if (rptrShoppingCart.Controls != null)
            {
                if (rptrShoppingCart.Controls.Count > 0)
                {
                    if (rptrShoppingCart.Controls[rptrShoppingCart.Controls.Count - 1].Controls != null)
                    {
                        if (rptrShoppingCart.Controls[rptrShoppingCart.Controls.Count - 1].Controls.Count > 0)
                        {
                            Control footer = rptrShoppingCart.Controls[rptrShoppingCart.Controls.Count - 1].Controls[0];

                            Label lblSubtotal = footer.FindControl("lblSubtotal") as Label;
                            if (lblSubtotal != null)
                            {
                                lblSubtotal.Text = String.Format("{0:C}", subtotal);
                            }

                            Label lblTax = footer.FindControl("lblTax") as Label;
                            if (lblTax != null)
                            {
                                lblTax.Text = String.Format("{0:C}", tax);
                            }

                            Label lblTotal = footer.FindControl("lblTotal") as Label;
                            if (lblTotal != null)
                            {
                                lblTotal.Text = String.Format("{0:C}", total);
                            }

                            if (couponDiscount > 0)
                            {
                                HtmlGenericControl divCouponDiscount = footer.FindControl("divCouponDiscount") as HtmlGenericControl;
                                if (divCouponDiscount != null)
                                {
                                    divCouponDiscount.Visible = true;
                                }
                                Label lblCouponDiscount = footer.FindControl("lblCouponDiscount") as Label;
                                if (lblCouponDiscount != null)
                                {
                                    lblCouponDiscount.Text = String.Format("{0:C}", (couponDiscount * -1));
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void rptrShoppingCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                JobPost jobPost = (JobPost)e.Item.DataItem;

                HyperLink hypEditJob = e.Item.FindControl("hypEditJob") as HyperLink;
                if (hypEditJob != null)
                {
                    hypEditJob.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/AddEditJob.aspx", new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() } });
                }

                HyperLink hypRepublishJob = e.Item.FindControl("hypRepublishJob") as HyperLink;
                if (hypRepublishJob != null)
                {
                    hypRepublishJob.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/PublishJob.aspx", new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() } });
                }

                HyperLink hypPreviewJob = e.Item.FindControl("hypPreviewJob") as HyperLink;
                if (hypPreviewJob != null)
                {
                    //preview. no need for se url
                    hypPreviewJob.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.JobDetail, new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() }, {"ispreview", "1"} });
                }
            }
        }

        protected void lnkbDelete_Command(object sender, CommandEventArgs e)
        {
            int jobPostId = Int32.Parse(e.CommandArgument.ToString());
            OnDeleteJob(new DeleteJobEventArgs(jobPostId));
        }
    }
}