using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.Web.Controls;

namespace HristoEvtimov.Websites.Work.Web.Admin.Controls
{
    public partial class couponadmin : GeneralControlBase, IFormValidation
    {
        UrlManager urlManager = null;
        CouponManager couponManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            couponManager = new CouponManager();
            hePaging.Paging += new _paging.PagingEventHandler(hePaging_Paging);

            urlManager = new UrlManager();
            hypAddNewCoupon.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/CouponEdit.aspx", null);

            if (!IsPostBack)
            {
                if (Request.QueryString["companyid"] != null)
                {
                    txtSearchByCompany.Text = Request.QueryString["companyid"].ToString();
                }
                LoadCoupons();
            }
        }

        public bool ValidateForm()
        {
            return Page.IsValid;
        }

        protected void btnSearch_Click(object sernder, EventArgs e)
        {
            hePaging.CurrentPage = 1;
            LoadCoupons();
        }

        protected void hePaging_Paging(object sender, _paging.PagingEventArgs e)
        {
            hePaging.CurrentPage = e.Page;
            LoadCoupons();
        }

        private void LoadCoupons()
        {
            Nullable<int> companyId = null;
            Nullable<int> userId = null;
            
            if (!String.IsNullOrEmpty(txtSearchByCompany.Text))
            {
                companyId = Int32.Parse(txtSearchByCompany.Text);
            }
            if (!String.IsNullOrEmpty(txtSearchByUser.Text))
            {
                userId = Int32.Parse(txtSearchByUser.Text);
            }
            
            int totalNumberOfResults = 0;
            rptrCoupons.DataSource = couponManager.GetCouponsForAdmin(companyId, userId, hePaging.CurrentPage, hePaging.PageSize, out totalNumberOfResults);
            rptrCoupons.DataBind();

            hePaging.TotalNumberOfItems = totalNumberOfResults;
            hePaging.BuildPaging();
        }

        protected void rptrCoupons_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Coupon coupon = (Coupon)e.Item.DataItem;

                Label lblCouponCode = e.Item.FindControl("lblCouponCode") as Label;
                if (lblCouponCode != null)
                {
                    lblCouponCode.Text = coupon.CouponCode;  
                }

                Label lblCompanyId = e.Item.FindControl("lblCompanyId") as Label;
                if (lblCompanyId != null && coupon.Company != null)
                {
                    lblCompanyId.Text = coupon.Company.Name;
                }

                Label lblUserId = e.Item.FindControl("lblUserId") as Label;
                if (lblUserId != null && coupon.User != null)
                {
                    lblUserId.Text = coupon.User.Email;
                }

                Label lblCouponDiscount = e.Item.FindControl("lblCouponDiscount") as Label;
                if (lblCouponDiscount != null)
                {
                    lblCouponDiscount.Text = couponManager.GetCouponDiscount(coupon);
                }

                Label lblCouponStatus = e.Item.FindControl("lblCouponStatus") as Label;
                if (lblCouponStatus != null)
                {
                    lblCouponStatus.Text = couponManager.GetCouponStatus(coupon).ToString();
                }

                HyperLink hypEditCoupon = e.Item.FindControl("hypEditCoupon") as HyperLink;
                if (hypEditCoupon != null)
                {
                    hypEditCoupon.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/CouponEdit.aspx", new Dictionary<string, string>() { {"couponid", coupon.CouponId.ToString()} });
                }
            }
        }
    }
}