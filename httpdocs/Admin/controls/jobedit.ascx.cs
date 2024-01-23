using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Admin.Controls
{
    public partial class jobedit : GeneralControlBase, IFormValidation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (JobPostId > 0)
                {
                    LoadJob(JobPostId);
                }
            }
        }

        public int JobPostId
        {
            get
            {
                if (Request.QueryString["jobpostid"] != null)
                {
                    return Int32.Parse(Request.QueryString["jobpostid"]);
                }
                return -1;
            }
        }

        private void LoadJob(int jobPostId)
        {
            pnlEditJob.Visible = true;

            JobManager jobManger = new JobManager();
            JobPost jobPost = jobManger.GetJobPostForAdmin(jobPostId);

            lblJobPostId.Text = jobPost.JobPostId.ToString();
            lblReviewRequired.Text = jobPost.ReviewRequired.ToString();
            lblStartDate.Text = jobPost.StartDate.ToString();
            lblEndDate.Text = jobPost.EndDate.ToString();
            lblIsFree.Text = jobPost.IsFreeAd.ToString();
            lblIsPaid.Text = jobPost.IsPaidAd.ToString();
            lblIsAnonymous.Text = jobPost.IsAnonymousAd.ToString();

            lblJobTitle.Text = HttpUtility.HtmlEncode(jobPost.Title);
            lblJobDescription.Text = HttpUtility.HtmlEncode(jobPost.Description);
            lblJobRequirements.Text = HttpUtility.HtmlEncode(jobPost.Requirements);
            lblJobBenefits.Text = HttpUtility.HtmlEncode(jobPost.Benefits);
            lblJobPosition.Text = HttpUtility.HtmlEncode(jobPost.Position);

            chkbJobSuspended.Checked = jobPost.Suspended;
            if (!jobPost.Suspended)
            {
                divSuspendJobPostReason.Attributes.Add("style", "display:none;");
            }

            UrlManager urlManager = new UrlManager();
            hypCompanyLink.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/CompanyEdit.aspx", new Dictionary<string, string>() { { "companyid", jobPost.CompanyId.ToString() } });
        }

        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                UserManager userManager = new UserManager();
                User currentUser = userManager.GetUser();

                int jobPostId = Int32.Parse(lblJobPostId.Text);
                JobManager jobManager = new JobManager();

                string suspendReason = "";
                if (chkbJobSuspended.Checked)
                {
                    suspendReason = rbtlSuspendJobPostReason.SelectedItem.Text;
                }

                bool result = jobManager.UpdateJobPostReviewed(jobPostId, currentUser.UserId, chkbJobSuspended.Checked, suspendReason);
                if (result)
                {
                    pnlEditJob.Visible = false;
                    AddSystemMessage(GetLocalResourceObject("strSuccess").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.OK,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                }
                else
                {
                    AddSystemMessage(GetLocalResourceObject("strFailure").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                }

                UrlManager urlManager = new UrlManager();
                Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Admin/JobAdmin.aspx", null));
            }
        }

        public bool ValidateForm()
        {
            if (chkbJobSuspended.Checked)
            {
                Page.Validate("SuspendJobPostReason");
            }

            return Page.IsValid;
        }
    }
}