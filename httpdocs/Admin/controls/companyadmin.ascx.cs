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
    public partial class companyadmin : GeneralControlBase, IFormValidation
    {
        UrlManager urlManager = new UrlManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            hePaging.Paging += new _paging.PagingEventHandler(hePaging_Paging);
            if (!IsPostBack)
            {
                BindCompanies();

                chkbSearchApproved.Checked = true;
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

        private void BindCompanies()
        {
            CompanyManager companyManager = new CompanyManager();
            rptrCompaniesPendingReview.DataSource = companyManager.GetCompaniesWaitingForReview();
            rptrCompaniesPendingReview.DataBind();
        }

        protected void rptrCompanies_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Company company = (Company)e.Item.DataItem;

                Literal litUsers = e.Item.FindControl("litUsers") as Literal;
                if (litUsers != null)
                {
                    foreach (User companyUser in company.Users)
                    {
                        litUsers.Text += companyUser.Email + "<BR />";
                    }
                }

                HyperLink hypReview = e.Item.FindControl("hypReview") as HyperLink;
                if (hypReview != null)
                {
                    hypReview.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/CompanyEdit.aspx", new Dictionary<string, string>() { { "companyid", company.CompanyId.ToString() } });
                }
            }
        }

        protected void btnSearchCompany_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                hePaging.CurrentPage = 1;
                SearchCompanies();
            }
        }

        private void SearchCompanies()
        {
            CompanyManager companyManager = new CompanyManager();
            int totalNumberOfResults = 0;
            rptrCompanySearch.DataSource = companyManager.AdminSearchForCompanies(txtSearchCompanyName.Text, 
                txtSearchEmailAddress.Text, chkbSearchApproved.Checked, txtSearchCreatedDateFrom.Text, 
                txtSearchCreatedDateTo.Text, hePaging.CurrentPage, hePaging.PageSize, out totalNumberOfResults);
            rptrCompanySearch.DataBind();

            hePaging.TotalNumberOfItems = totalNumberOfResults;
            hePaging.BuildPaging();
        }

        public bool ValidateForm()
        {
            StringValidation stringValidation = new StringValidation();
            txtSearchCompanyName.Text = stringValidation.SanitizeUserInputString(txtSearchCompanyName.Text.Trim(), StringValidation.SanitizeEntityNames.CompanyName).Trim();
            txtSearchEmailAddress.Text = stringValidation.SanitizeUserInputString(txtSearchEmailAddress.Text.Trim(), StringValidation.SanitizeEntityNames.Email).Trim();

            return Page.IsValid;
        }

        protected void hePaging_Paging(object sender, _paging.PagingEventArgs e)
        {
            hePaging.CurrentPage = e.Page;
            SearchCompanies();
        }
    }
}