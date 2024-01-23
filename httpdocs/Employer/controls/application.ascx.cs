using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class application : GeneralControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["jobapplicationid"] != null)
            {
                User currentUser = GetUser();

                int jobApplicationId = Int32.Parse(Request["jobapplicationid"].ToString());
                JobApplicationManager jobApllicationManager = new JobApplicationManager();
                JobApplication jobApplication = jobApllicationManager.GetJobApplication(jobApplicationId, currentUser.UserId);

                if (jobApplication != null)
                {
                    lblCoverLetter.Text = jobApplication.CoverLetter;
                    lblResume.Text = jobApplication.Resume;
                    lblJobTitle.Text = jobApplication.JobPost.Title;
                    lblJobApplicantName.Text = jobApplication.FirstName + " " + jobApplication.LastName;
                    lblJobApplicationDate.Text = jobApplication.DateCreated.ToString("MM/dd/yyyy hh:mm");
                    lblJobApplicantEmail.Text = jobApplication.Email;
                    lblJobApplicantPhone.Text = jobApplication.Phone;
                }
            }
        }
    }
}