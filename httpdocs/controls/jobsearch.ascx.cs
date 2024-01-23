using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects;
using HristoEvtimov.Websites.Work.WorkSearch;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class jobsearch : System.Web.UI.UserControl
    {
        UrlManager urlManager = new UrlManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            hePaging.Paging += new _paging.PagingEventHandler(hePaging_Paging);

            if (!IsPostBack)
            {
                CategoryManager categoryManager = new CategoryManager();
                ddlCategory.DataSource = categoryManager.GetCategories();
                ddlCategory.DataBind();

                if (WebConfigurationManager.AppSettings["JOB_SEARCH_RESULTS_PAGE_SIZE"] != null)
                {
                    hePaging.PageSize = Int32.Parse(WebConfigurationManager.AppSettings["JOB_SEARCH_RESULTS_PAGE_SIZE"].ToString());
                }
                if (Request.QueryString["term"] != null)
                {
                    txtSearch.Text = Server.UrlDecode(Request.QueryString["term"].ToString());
                }
                RunSearch();
            }
        }

        protected void hePaging_Paging(object sender, _paging.PagingEventArgs e)
        {
            hePaging.CurrentPage = e.Page;
            RunSearch();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hePaging.CurrentPage = 1;
            RunSearch();
        }

        protected void ddlOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            RunSearch();
        }

        protected void RunSearch()
        {
            JobSearch jobSearch = new JobSearch();

            double lattitude = 0;
            double longitude = 0;
            if (!String.IsNullOrEmpty(txtMilesZip.Text))
            {
                string milesZipCode = txtMilesZip.Text.Trim();
                if (milesZipCode.Length > 5)
                {
                    milesZipCode = milesZipCode.Substring(0, 5);
                }
                int iMilesZipCode = 0;
                if (Int32.TryParse(milesZipCode, out iMilesZipCode))
                {
                    CountryManager countryManager = new CountryManager();
                    ZipCode oMilesZipCode = countryManager.GetZipCode(iMilesZipCode);
                    if (oMilesZipCode != null)
                    {
                        lattitude = (double)oMilesZipCode.Lattitude;
                        longitude = (double)oMilesZipCode.Longitude;
                    }
                }
            }

            //get sorting
            JobSearch.SortBy sort = JobSearch.SortBy.Relevance;
            if (ddlOrderBy.SelectedValue == "StartDate")
            {
                sort = JobSearch.SortBy.StartDate;
            }

            int categoryId = Int32.Parse(ddlCategory.SelectedValue);

            int totalNumberOfResults = -1;
            rptrJobResults.DataSource = jobSearch.Search(txtSearch.Text, categoryId, lattitude, longitude, Double.Parse(ddlMilesDistance.SelectedValue), sort, hePaging.CurrentPage, hePaging.PageSize, out totalNumberOfResults);
            rptrJobResults.DataBind();
            
            hePaging.TotalNumberOfItems = totalNumberOfResults;
            hePaging.BuildPaging();

            if (totalNumberOfResults > 0)
            {
                rptrJobResults.Visible = true;
                lblNumberOfSearchResults.Text = String.Format(GetLocalResourceObject("strNumberOfSearchResults").ToString(), totalNumberOfResults);
            }
            else
            {
                rptrJobResults.Visible = false;
                lblNumberOfSearchResults.Text = GetLocalResourceObject("strNoSearchResults").ToString();
            }
        }

        protected void rptrJobResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                JobSearchResult jobSearchResult = (JobSearchResult)e.Item.DataItem;
                if (e.Item.FindControl("hypJobDetail") != null)
                {
                    HyperLink hypJobDetail = (HyperLink)e.Item.FindControl("hypJobDetail");
                    hypJobDetail.NavigateUrl = JobManager.GetAbsoluteUrl(urlManager, jobSearchResult);
                    hypJobDetail.Text = jobSearchResult.Title;
                }
            }
        }
    }
}