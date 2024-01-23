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
    public partial class myjobs : GeneralControlBase
    {
        private UrlManager urlManager = new UrlManager();
        private JobManager jobManager = new JobManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            hePagingPublished.Paging += new _paging.PagingEventHandler(hePagingPublished_Paging);
            hePagingPublished.PageSize = 5;

            hePagingPublishedFuture.Paging += new _paging.PagingEventHandler(hePagingPublishedFuture_Paging);
            hePagingPublishedFuture.PageSize = 5;

            hePagingExpired.Paging += new _paging.PagingEventHandler(hePagingExpired_Paging);
            hePagingExpired.PageSize = 5;
            
            if (!IsPostBack)
            {
                User user = GetUser();

                BindJobs(user);
                                
                hypAddJob.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/AddEditJob.aspx", null);
            }
        }

        protected void hePagingPublished_Paging(object sender, _paging.PagingEventArgs e)
        {
            hePagingPublished.CurrentPage = e.Page;
            User user = GetUser();
            BindJobsPublished(user);
        }

        protected void hePagingPublishedFuture_Paging(object sender, _paging.PagingEventArgs e)
        {
            hePagingPublishedFuture.CurrentPage = e.Page;
            User user = GetUser();
            BindJobsPublishedFuture(user);
        }

        protected void hePagingExpired_Paging(object sender, _paging.PagingEventArgs e)
        {
            hePagingExpired.CurrentPage = e.Page;
            User user = GetUser();
            BindJobsExpired(user);
        }

        private void BindJobs(User user)
        {
            plhPublished.Visible = false;
            plhPublishedFuture.Visible = false;
            plhExpired.Visible = false;
            plhUnpublished.Visible = false;
            plhPendingReview.Visible = false;

            BindJobsPublished(user);
            BindJobsPublishedFuture(user);
            BindJobsUnpublished(user);
            BindJobsExpired(user);
            BindJobsPendingReview(user);

            bool noJobs = !(plhPublished.Visible || plhExpired.Visible || plhPendingReview.Visible || plhUnpublished.Visible || plhPublishedFuture.Visible);
            if (noJobs)
            {
                AddSystemMessage(GetLocalResourceObject("strNoJobs").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.Info,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/AddEditJob.aspx", null));
            }
        }

        private void BindJobsUnpublished(User user)
        {
            List<JobPost> jobsUnpublished = jobManager.GetJobPostsByUserUnpublished(user.UserId);
            if (jobsUnpublished != null)
            {
                if (jobsUnpublished.Count > 0)
                {
                    plhUnpublished.Visible = true;
                    rptrJobsUnpublished.DataSource = jobsUnpublished;
                    rptrJobsUnpublished.DataBind();
                    int maxJobsUnpublished = jobManager.MaxNumberOfUnpublishedJobs();
                    if (jobsUnpublished.Count >= maxJobsUnpublished)
                    {
                        hypAddJob.Enabled = false;
                        hypAddJob.CssClass = "btn btn-danger";
                        lblAddJobDisabled.Visible = true;
                        lblAddJobDisabled.Text = string.Format(lblAddJobDisabled.Text, maxJobsUnpublished);
                    }
                }
            }
        }

        private void BindJobsPublished(User user)
        {
            int totalNumberOfResults = -1;
            List<JobPost> jobsPublished = jobManager.GetJobPostsByUserPublished(user.UserId, hePagingPublished.CurrentPage, hePagingPublished.PageSize, out totalNumberOfResults);
            if (jobsPublished != null)
            {
                if (jobsPublished.Count > 0)
                {
                    plhPublished.Visible = true;
                    rptrJobsPublished.DataSource = jobsPublished;
                    rptrJobsPublished.DataBind();

                    hePagingPublished.TotalNumberOfItems = totalNumberOfResults;
                    hePagingPublished.BuildPaging();
                }
            }
        }

        private void BindJobsPublishedFuture(User user)
        {
            int totalNumberOfResults = -1;
            List<JobPost> jobsPublishedFuture = jobManager.GetJobPostsByUserPublishedFuture(user.UserId, hePagingPublishedFuture.CurrentPage, hePagingPublishedFuture.PageSize, out totalNumberOfResults);
            if (jobsPublishedFuture != null)
            {
                if (jobsPublishedFuture.Count > 0)
                {
                    plhPublishedFuture.Visible = true;
                    rptrJobsPublishedFuture.DataSource = jobsPublishedFuture;
                    rptrJobsPublishedFuture.DataBind();

                    hePagingPublishedFuture.TotalNumberOfItems = totalNumberOfResults;
                    hePagingPublishedFuture.BuildPaging();
                }
            }
        }

        private void BindJobsExpired(User user)
        {
            int totalNumberOfResults = 0;
            List<JobPost> jobsExpired = jobManager.GetJobPostsByUserPublishedExpired(user.UserId, hePagingExpired.CurrentPage, hePagingExpired.PageSize, out totalNumberOfResults);
            if (jobsExpired != null)
            {
                if (jobsExpired.Count > 0)
                {
                    plhExpired.Visible = true;
                    rptrJobsPublishedExpired.DataSource = jobsExpired;
                    rptrJobsPublishedExpired.DataBind();

                    hePagingExpired.TotalNumberOfItems = totalNumberOfResults;
                    hePagingExpired.BuildPaging();
                }
            }
        }

        private void BindJobsPendingReview(User user)
        {
            List<JobPost> jobsPendingReview = jobManager.GetJobPostsByUserPublishedPendingReview(user.UserId);
            if (jobsPendingReview != null)
            {
                if (jobsPendingReview.Count > 0)
                {
                    plhPendingReview.Visible = true;
                    rptrJobsPendingReview.DataSource = jobsPendingReview;
                    rptrJobsPendingReview.DataBind();
                }
            }
        }

        protected void rptrJobs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                JobPost jobPost = (JobPost)e.Item.DataItem;
                JobManager jobManager = new JobManager();

                if (e.Item.FindControl("hypEditJob") != null)
                {
                    HyperLink hypEditJob = (HyperLink)e.Item.FindControl("hypEditJob");
                    hypEditJob.Visible = false;
                    if (jobManager.UserCanEdit(jobPost))
                    {
                        hypEditJob.Visible = true;
                        hypEditJob.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/AddEditJob.aspx", new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() } });
                    }
                }

                if (e.Item.FindControl("hypPublishJob") != null)
                {
                    HyperLink hypPublishJob = (HyperLink)e.Item.FindControl("hypPublishJob");
                    if (jobManager.UserCanPublish(jobPost))
                    {
                        hypPublishJob.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/PublishJob.aspx", new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() } });
                        if (jobPost.AddedToCart)
                        {
                            hypPublishJob.Text = GetLocalResourceObject("strRePublishJob").ToString();
                            hypPublishJob.Attributes.Add("alt", GetLocalResourceObject("strRePublishJob.alt").ToString());
                            hypPublishJob.Attributes.Add("title", GetLocalResourceObject("strRePublishJob.title").ToString());
                        }
                        else
                        {
                            hypPublishJob.Text = GetLocalResourceObject("strPublishJob").ToString();
                            hypPublishJob.Attributes.Add("alt", GetLocalResourceObject("strPublishJob.alt").ToString());
                            hypPublishJob.Attributes.Add("title", GetLocalResourceObject("strPublishJob.title").ToString());
                        }
                    }
                }

                HyperLink hypViewApplications = e.Item.FindControl("hypViewApplications") as HyperLink;
                if (hypViewApplications != null)
                {
                    hypViewApplications.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/ViewJobApplications.aspx", new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() } });
                }

                HyperLink hypRepublish = e.Item.FindControl("hypRepublish") as HyperLink;
                if (hypRepublish != null)
                {
                    hypRepublish.Visible = hypAddJob.Visible;
                    hypRepublish.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/AddEditJob.aspx", new Dictionary<string, string>() { { "republishjobpostid", jobPost.JobPostId.ToString() } });
                }

                HyperLink hypPreviewJob = e.Item.FindControl("hypPreviewJob") as HyperLink;
                if (hypPreviewJob != null)
                {
                    //preview. no need for se url
                    hypPreviewJob.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.JobDetail, new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() }, { "ispreview", "1" } });
                }

                Button btnSuspend = e.Item.FindControl("btnSuspend") as Button;
                Panel pnlSuspend = e.Item.FindControl("pnlSuspend") as Panel;
                if (btnSuspend != null && pnlSuspend != null)
                {
                    pnlSuspend.Visible = false;
                    btnSuspend.Visible = false;
                    if (jobPost.IsPaidAd)
                    {
                        btnSuspend.Visible = true;
                        pnlSuspend.Visible = true;
                        btnSuspend.CommandArgument = jobPost.JobPostId.ToString();
                    }
                }                
            }
        }

        protected void btnSuspend_Command(object sender, CommandEventArgs e)
        {
            int jobPostId = Int32.Parse(e.CommandArgument.ToString());
            User currentUser = GetUser();
            bool result = jobManager.UpdateJobPostSuspend(jobPostId, currentUser, true);
            if (result)
            {
                AddSystemMessage(GetLocalResourceObject("strOKSuspend").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.OK,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                plhPublished.Visible = false;
                plhExpired.Visible = false;
                hePagingPublished.CurrentPage = 1;
                hePagingExpired.CurrentPage = 1;
                BindJobsPublished(currentUser);
                BindJobsExpired(currentUser);
            }
            else
            {
                AddSystemMessage(GetLocalResourceObject("strErrorSuspend").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }
    }
}