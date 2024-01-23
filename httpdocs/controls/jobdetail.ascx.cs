using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    /// <summary>
    /// This control will display the job details.
    /// </summary>
    public partial class jobdetail : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["jobpostid"] != null)
            {
                int jobPostId = 0;
                if (Int32.TryParse(Request.QueryString["jobpostid"].ToString(), out jobPostId))
                {
                    DisplayJobPost(jobPostId);
                }
            }
        }

        public void DisplayJobPost(int jobPostId)
        {
            bool isPreview = false;
            if (Request.QueryString["ispreview"] != null)
            {
                isPreview = true;
            }

            JobManager jobManager = new JobManager();
            JobPost jobPost = jobManager.GetJobPostForDetail(jobPostId, isPreview, true);

            if (jobPost != null)
            {
                if (!isPreview)
                {
                    string qsSeUrl = "";
                    if (Request.QueryString["seurl"] != null)
                    {
                        qsSeUrl = Request.QueryString["seurl"].ToString();
                    }

                    if (!String.IsNullOrEmpty(jobPost.SeUrl))
                    {
                        if (!jobPost.SeUrl.Equals(qsSeUrl))
                        {
                            UrlManager urlManager = new UrlManager();
                            Response.RedirectPermanent(JobManager.GetAbsoluteUrl(urlManager, jobPost), true);
                        }
                    }
                }

                if ((jobPost.IsPaidAd || jobPost.IsAnonymousAd) && !String.IsNullOrEmpty(jobPost.ReplyUrl))
                {
                    hypJobApplication.NavigateUrl = jobPost.ReplyUrl;
                }
                else
                {
                    UrlManager urlManager = new UrlManager();
                    hypJobApplication.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.JobApplication,
                        new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() } });
                }

                string jobHeading = jobManager.GetJobHeading(jobPost.Company.Name, jobPost.Position,
                    jobPost.Company.IsRecruiter, jobPost.IsAnonymousAd,
                    GetLocalResourceObject("strHiringText").ToString(), 
                    GetLocalResourceObject("strRecruitingText").ToString(),
                    GetLocalResourceObject("strHiringAnonymousText").ToString());
                lblHeading.Text = jobHeading;
                lblTitle.Text = jobPost.Title;
                lblDescription.Text = jobPost.Description;

                lblRequirements.Text = jobPost.Requirements;
                if (String.IsNullOrEmpty(jobPost.Requirements))
                {
                    pnlRequirements.Visible = false;
                }

                lblBenefits.Text = jobPost.Benefits;
                if (String.IsNullOrEmpty(jobPost.Benefits))
                {
                    pnlBenefits.Visible = false;
                }

                lblLocation.Text = jobPost.Location;
                lblEmploymentType.Text = jobPost.EmploymentType.Name;

                if (jobPost.StartDate != null)
                {
                    lblStartDate.Text = ((DateTime)jobPost.StartDate).ToString("MM/dd/yyyy");
                }

                this.Page.Title = Server.HtmlEncode(GetGlobalResourceObject("PageTitles", "strJobDetailPrefix").ToString() + " " + jobHeading);
                this.Page.MetaDescription = jobPost.SeDescription;
            }
            else
            {
                plhDetail.Visible = false;
                plhError.Visible = true;
                this.Page.Title = GetGlobalResourceObject("PageTitles", "strJobDetail").ToString();
                this.Page.MetaDescription = "";

                Response.TrySkipIisCustomErrors = true;
                Response.Status = "404 Not Found";
                Response.StatusCode = 404;            
            }
        }
    }
}