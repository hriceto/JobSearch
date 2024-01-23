using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects;
using HristoEvtimov.Websites.Work.WorkSearch;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class latestjobs : GeneralControlBase
    {
        private int _numberOfItems = 1;
        public int NumberOfItems
        {
            get {return _numberOfItems;}
            set {_numberOfItems = value;}
        }

        private int _categoryId = -1;
        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        UrlManager urlManager = new UrlManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadJobs();
        }

        private void LoadJobs()
        {
            JobSearch jobSearch = new JobSearch();
            int totalNumberOfResults = 0;
            rptrJobs.DataSource = jobSearch.Search("", _categoryId, 0, 0, 1, JobSearch.SortBy.StartDate, 1, _numberOfItems, out totalNumberOfResults);
            rptrJobs.DataBind();

            if (totalNumberOfResults == 0)
            {
                pnlEmpty.Visible = true;
                if (_categoryId > 0)
                {
                    lblEmptyList.Text = GetLocalResourceObject("strEmptyIndustryList").ToString();
                }
                else
                {
                    lblEmptyList.Text = GetLocalResourceObject("strEmptyList").ToString();
                }
            }
        }

        protected void rptrJobs_ItemDataBound(object sender, RepeaterItemEventArgs e)
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