using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class mycoupons : GeneralControlBase
    {
        User currentUser = null;
        CouponManager couponManager = null;
            
        protected void Page_Load(object sender, EventArgs e)
        {
            couponManager = new CouponManager();

            if (!IsPostBack)
            {
                currentUser = GetUser();
                LoadCoupons();
            }
        }

        private void LoadCoupons()
        {
            if (currentUser.CompanyId.HasValue)
            {
                var myCoupons = couponManager.GetMyCoupons(currentUser.CompanyId.Value, currentUser.UserId);
                var siteCoupons = couponManager.GetSiteCoupons();

                rptrMyCoupons.DataSource = myCoupons;
                rptrMyCoupons.DataBind();

                rptrSiteCoupons.DataSource = siteCoupons;
                rptrSiteCoupons.DataBind();

                plhMyCoupons.Visible = (myCoupons.Count > 0);
                plhSiteCoupons.Visible = (siteCoupons.Count > 0);

                int totalCoupons = (myCoupons.Count + siteCoupons.Count);
                lblHeadingCouponsYes.Visible = (totalCoupons > 0);
                lblHeadingCouponsNo.Visible = (totalCoupons <= 0);
                lblHeadingCouponsYes.Text = String.Format(GetLocalResourceObject("strHeadingCouponsYes").ToString(), currentUser.FirstName);
                lblHeadingCouponsNo.Text = String.Format(GetLocalResourceObject("strHeadingCouponsNo").ToString(), WebConfigurationManager.AppSettings["CUSTOMER_SERVICE_EMAIL"]);
            }
        }

        protected void rptrCoupons_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Coupon coupon = (Coupon)e.Item.DataItem;
                CouponManager.CouponStatus couponStatus = couponManager.GetCouponStatus(coupon);

                Label lblCouponCode = e.Item.FindControl("lblCouponCode") as Label;
                if (lblCouponCode != null)
                {
                    lblCouponCode.Text = coupon.CouponCode;
                }

                Label lblNumberOfUsesLeft = e.Item.FindControl("lblNumberOfUsesLeft") as Label;
                if (lblNumberOfUsesLeft != null)
                {
                    lblNumberOfUsesLeft.Text = (coupon.NumberOfUsesLimit - coupon.NumberOfUses).ToString();
                }

                Label lblCouponDiscount = e.Item.FindControl("lblCouponDiscount") as Label;
                if (lblCouponDiscount != null)
                {
                    lblCouponDiscount.Text = couponManager.GetCouponDiscount(coupon);
                }

                Label lblCouponStatus = e.Item.FindControl("lblCouponStatus") as Label;
                if (lblCouponStatus != null)
                {
                    lblCouponStatus.Text = couponStatus.ToString(); 
                }

                Label lblCouponExpiration = e.Item.FindControl("lblCouponExpiration") as Label;
                if (lblCouponExpiration != null)
                {
                    if (coupon.EndDate.HasValue)
                    {
                        lblCouponExpiration.Text = coupon.EndDate.Value.ToString("MM/dd/yyyy");
                    }
                }

                Button btnUseCoupon = e.Item.FindControl("btnUseCoupon") as Button;
                if (btnUseCoupon != null)
                {
                    btnUseCoupon.Visible = (couponStatus == CouponManager.CouponStatus.Active);
                    btnUseCoupon.CommandArgument = coupon.CouponId.ToString();
                    if (currentUser.CouponId.HasValue)
                    {
                        if (coupon.CouponId == currentUser.CouponId.Value)
                        {
                            btnUseCoupon.CommandArgument = "-1";
                            btnUseCoupon.Enabled = false;
                            btnUseCoupon.Text = GetLocalResourceObject("btnUseCouponInactive").ToString();
                        }
                    }
                }
            }
        }

        protected void btnUseCoupon_Command(object sender, CommandEventArgs e)
        {
            UserManager userManager = new UserManager();
            currentUser = userManager.GetUser();

            if (currentUser == null)
            {
                RedirectToHomeAndError("strUserNotLoggedIn");
            }

            bool updateSuccess = userManager.UpdateUserCoupon(currentUser, Int32.Parse(e.CommandArgument.ToString()));
            if (updateSuccess)
            {
                AddSystemMessage(GetLocalResourceObject("strUseCouponOk").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.OK,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                LoadCoupons();
            }
            else
            {
                AddSystemMessage(GetLocalResourceObject("strUseCouponError").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }
    }
}