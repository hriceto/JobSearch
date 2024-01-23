using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Admin.Controls
{
    public partial class companyedit : GeneralControlBase, IFormValidation
    {
        UrlManager urlManager = new UrlManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CompanyId > 0)
                {
                    LoadCompany(CompanyId);
                }
            }
        }

        public int CompanyId
        {
            get
            {
                if (Request.QueryString["companyid"] != null)
                {
                    return Int32.Parse(Request.QueryString["companyid"]);
                }
                return -1;
            }
        }

        private void LoadCompany(int companyId)
        {
            pnlReviewCompany.Visible = true;

            CompanyManager companyManager = new CompanyManager();
            Company company = companyManager.GetCompanyForReview(companyId);
            if (company != null)
            {
                lblCompanyId.Text = company.CompanyId.ToString();
                lblCompanyName.Text = company.Name;
                lblCompanyAddress.Text = company.Address1 + " " + company.Address2 + " " + company.City + " " +
                    company.State + " " + company.Zip + " " + company.Country;
                lblCompanyPhone.Text = company.Phone;
                lblCompanyWebsite.Text = company.Website;
                lblCompanyCreatedDate.Text = company.CreatedDate.ToString();
                lblCompanyUsers.Text = "";
                foreach (User user in company.Users)
                {
                    if (user.OkToEmail)
                    {
                        lblCompanyUsers.Text += String.Format(GetLocalResourceObject("strUserOkToEmail").ToString(), user.Email, user.UserId.ToString());
                    }
                    else
                    {
                        lblCompanyUsers.Text += String.Format(GetLocalResourceObject("strUserNotOkToEmail").ToString(), user.Email, user.UserId.ToString());
                    }
                }
                chkbCompanyIsRecruiter.Checked = company.IsRecruiter;
                chkbCompanyAllowFreePosts.Checked = company.AllowFreeJobPosts;
                if (company.AllowFreeJobPosts)
                {
                    divDeclineFreePostsReason.Attributes.Add("style", "display:none;");
                }
                txtCompanyDomain.Text = company.CompanyDomain;

                hypCompanyJobs.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/JobAdmin.aspx", 
                    new Dictionary<string, string>() { { "companyid", company.CompanyId.ToString() } });
                hypAddCompanyCoupon.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/CouponEdit.aspx",
                    new Dictionary<string, string>() { { "companyid", company.CompanyId.ToString() } });
                hypViewCompanyCoupons.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/CouponAdmin.aspx",
                    new Dictionary<string, string>() { { "companyid", company.CompanyId.ToString() } });

                if (company.CompanyApplications != null && company.CompanyApplications.Count > 0)
                {
                    var companyApplication = company.CompanyApplications.FirstOrDefault();

                    pnlCompanyApplication.Visible = true;
                    lblCompanyApplicationNumberOfEmployees.Text = companyApplication.NumberOfEmployees;
                    lblCompanyApplicationNumberOfPostsPerYear.Text = companyApplication.NumberOfPostsPerYear;
                    chkbCompanyApplicationApprove.Checked = companyApplication.Company.AllowUnlimitedFreeJobPosts.HasValue ?
                        companyApplication.Company.AllowUnlimitedFreeJobPosts.Value : false;

                }
            }
        }

        protected void btnReviewCompany_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                int companyId = Int32.Parse(lblCompanyId.Text);
                string reasonText = "";
                if (!chkbCompanyAllowFreePosts.Checked)
                {
                    reasonText = rbtlDeclineFreePostsReason.SelectedItem.Text;
                }

                CompanyManager companyManager = new CompanyManager();
                bool result = companyManager.UpdateCompanyReviewed(companyId, chkbCompanyIsRecruiter.Checked,
                    chkbCompanyAllowFreePosts.Checked, txtCompanyDomain.Text, reasonText, chkbCompanyApplicationApprove.Checked);
                if (result)
                {
                    pnlReviewCompany.Visible = false;
                    AddSystemMessage(GetLocalResourceObject("strSuccessUpdateCompany").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.OK,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                }
                else
                {
                    AddSystemMessage(GetLocalResourceObject("strFailureUpdateCompany").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                }

                Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Admin/CompanyAdmin.aspx", null));
            }
        }

        public bool ValidateForm()
        {
            if (!chkbCompanyAllowFreePosts.Checked)
            {
                Page.Validate("DeclineFreePostsReason");
            }

            return Page.IsValid;
        }

    }
}