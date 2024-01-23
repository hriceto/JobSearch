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

namespace HristoEvtimov.Websites.Work.Web.Admin.Controls
{
    public partial class couponedit : GeneralControlBase, IFormValidation
    {
        public int CouponId
        {
            get
            {
                if (Request.QueryString["couponid"] != null)
                {
                    return Int32.Parse(Request.QueryString["couponid"].ToString());
                }
                return -1;
            }
        }

        public bool IsEditCoupon
        {
            get { return CouponId > 0; }
        }

        UrlManager urlManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            urlManager = new UrlManager();
            hypCouponSearch.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/CouponAdmin.aspx", null);

            if (!IsPostBack)
            {
                if (Request.QueryString["companyid"] != null)
                {
                    txtCouponCompanyId.Text = Request.QueryString["companyid"].ToString();
                }
                LoadCoupon();    
            }
        }

        public void LoadCoupon()
        {
            if (IsEditCoupon)
            {
                CouponManager couponManager = new CouponManager();
                Coupon coupon = couponManager.GetCouponForAdmin(CouponId);

                if (coupon == null)
                {
                    Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Admin/CouponEdit.aspx", null));
                }

                lblCouponId.Text = coupon.CouponId.ToString();
                txtCouponUserId.Text = coupon.UserId.HasValue ? coupon.UserId.Value.ToString() : "";
                txtCouponCompanyId.Text = coupon.CompanyId.HasValue ? coupon.CompanyId.Value.ToString() : "";
                txtCouponCode.Text = coupon.CouponCode;
                lblNumberOfUses.Text = coupon.NumberOfUses.ToString();
                txtNumberOfUsesLimit.Text = coupon.NumberOfUsesLimit.ToString();
                txtDiscountPercentage.Text = coupon.DiscountPercentage.HasValue ? coupon.DiscountPercentage.Value.ToString() : "";
                txtDiscountAmount.Text = coupon.DiscountAmount.HasValue ? coupon.DiscountAmount.Value.ToString() : "";
                txtStartDate.Text = coupon.StartDate.HasValue ? coupon.StartDate.Value.ToString("MM/dd/yyyy") : "";
                txtEndDate.Text = coupon.EndDate.HasValue ? coupon.EndDate.Value.ToString("MM/dd/yyyy") : "";
                btnSave.Text = GetLocalResourceObject("strBtnSaveUpdate").ToString();
                btnDeactivate.Visible = true;
            }
            else
            {
                btnSave.Text = GetLocalResourceObject("strBtnSaveAdd").ToString();
                btnDeactivate.Visible = false;
            }
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                User currentUser = GetUser();
                int result = -1;
                CouponManager couponManager = new CouponManager();
                if (IsEditCoupon)
                {
                    result = couponManager.UpdateCoupon(CouponId, txtCouponUserId.Text, txtCouponCompanyId.Text,
                       txtCouponCode.Text, Int32.Parse(txtNumberOfUsesLimit.Text), txtDiscountPercentage.Text,
                       txtDiscountAmount.Text, txtStartDate.Text, txtEndDate.Text, currentUser.UserId);
                }
                else
                {
                    result = couponManager.AddCoupon(txtCouponUserId.Text, txtCouponCompanyId.Text,
                        txtCouponCode.Text, Int32.Parse(txtNumberOfUsesLimit.Text), txtDiscountPercentage.Text,
                        txtDiscountAmount.Text, txtStartDate.Text, txtEndDate.Text, currentUser.UserId);
                }

                if (result > 0)
                {
                    AddSystemMessage(GetLocalResourceObject("strSaveOK").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.OK,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                    Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Admin/CouponEdit.aspx", new Dictionary<string, string> { {"couponid", result.ToString()} }));
                }
                else
                {
                    AddSystemMessage(GetLocalResourceObject("strSaveError").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
            }
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (IsEditCoupon)
            {
                User currentUser = GetUser();
                CouponManager couponManager = new CouponManager();
                if (couponManager.DeactivateCoupon(CouponId, currentUser.UserId))
                {
                    AddSystemMessage(GetLocalResourceObject("strDeactivateOK").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.OK,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
                else
                {
                    AddSystemMessage(GetLocalResourceObject("strDeactivateError").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
            }
        }

        public bool ValidateForm()
        {
            return Page.IsValid;
        }
    }
}