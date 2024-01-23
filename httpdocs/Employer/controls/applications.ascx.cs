using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.Web.Controls;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class applications : GeneralControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hePaging.Paging += new _paging.PagingEventHandler(hePaging_Paging);

            if (!IsPostBack)
            {
                BindJobApplications();
            }
        }

        protected void BindJobApplications()
        {
            lblEmptyMessage.Visible = false;
            if (Request.QueryString["jobpostid"] != null)
            {
                int jobPostId = -1;
                if (Int32.TryParse(Request.QueryString["jobpostid"].ToString(), out jobPostId))
                {
                    User currentUser = GetUser();

                    //set job post info
                    JobManager jobManager = new JobManager();
                    JobPost jobPost = jobManager.GetJobPostForEmployerAdmin(jobPostId, currentUser.UserId);

                    if (jobPost != null)
                    {
                        lblTitle.Text = jobPost.Title;
                        lblJobApplications.Text = String.Format(GetLocalResourceObject("lblJobApplications").ToString(), 
                            jobPost.Title);
                        if (jobPost.StartDate != null)
                        {
                            lblBeginDate.Text = ((DateTime)jobPost.StartDate).ToString("MM/dd/yyyy");
                        }
                        if (jobPost.EndDate != null)
                        {
                            lblEndDate.Text = ((DateTime)jobPost.EndDate).ToString("MM/dd/yyyy");
                        }
                        lblLocation.Text = jobPost.Location;
                        lblNumberOfViews.Text = jobPost.NumberOfViews.ToString();

                        //set job applications
                        JobApplicationManager jobApplicationManager = new JobApplicationManager();

                        int totalNumberOfResults = -1;
                        rptrJobApplications.DataSource = jobApplicationManager.GetJobApplications(jobPostId, currentUser.UserId, hePaging.CurrentPage, hePaging.PageSize, out totalNumberOfResults);
                        rptrJobApplications.DataBind();

                        hePaging.TotalNumberOfItems = totalNumberOfResults;
                        hePaging.BuildPaging();

                        lblNumberOfApplications.Text = totalNumberOfResults.ToString();

                        rptrJobApplications.Visible = (totalNumberOfResults > 0);
                        lblEmptyMessage.Visible = (totalNumberOfResults == 0);
                    }
                }
            }
        }

        protected void hePaging_Paging(object sender, _paging.PagingEventArgs e)
        {
            hePaging.CurrentPage = e.Page;
            BindJobApplications();
        }

        protected void rptrJobApplications_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                JobApplication jobApplication = (JobApplication)e.Item.DataItem;
                if (e.Item.FindControl("hypApplicationDetails") != null)
                {
                    HyperLink hypApplicationDetails = (HyperLink)e.Item.FindControl("hypApplicationDetails");
                    UrlManager urlManager = new UrlManager();
                    hypApplicationDetails.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/ViewJobApplication.aspx", new Dictionary<string, string>() { { "jobapplicationid", jobApplication.JobApplicationId.ToString() } });
                }
            }
        }
    }
}