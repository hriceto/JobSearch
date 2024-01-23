using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class JobApplicationManager
    {
        public int AddJobApplication(int jobPostId, User applicantUser, string firstName, string lastName,
            string email, string coverLetter, string resume, string phone)
        {
            if (jobPostId <= 0)
            {
                return -1;
            }

            JobManager jobManager = new JobManager();
            JobPost jobPost = jobManager.GetJobPostForDetail(jobPostId, false, false);
            if (jobPost == null)
            {
                return -1;
            }

            HtmlParser htmlParserAllowSomeHtml = new HtmlParser(HtmlParser.ParseRules.AllowSomeHtml);
            htmlParserAllowSomeHtml.RemoveAllowedTag("a");

            JobApplication jobApplication = JobApplication.CreateJobApplication(-1, jobPostId, firstName,
                lastName, email, DateTime.Now);

            jobApplication.Phone = phone;
            jobApplication.CoverLetter = htmlParserAllowSomeHtml.FilterHtml(coverLetter.Trim()).Trim();
            jobApplication.Resume = htmlParserAllowSomeHtml.FilterHtml(resume.Trim()).Trim();
            if (applicantUser != null)
            {
                jobApplication.ApplicantUserId = applicantUser.UserId;
            }

            JobApplicationDataAccess jada = new JobApplicationDataAccess();
            int result = jada.AddJobApplication(jobApplication);

            //send email to employer
            if (result > 0)
            {
                Email emailer = new Email();
                //add email to employer upon application.
                JobDataAccess jda = new JobDataAccess();
                User employerUser = jda.GetJobPostEmployerUser(jobPostId);
                if (employerUser != null)
                {
                    //find employer email address
                    string employerEmail = employerUser.Email;
                    if ((jobPost.IsAnonymousAd || jobPost.IsPaidAd) && !String.IsNullOrEmpty(jobPost.ReplyEmail))
                    {
                        employerEmail = jobPost.ReplyEmail;
                    }

                    UrlManager urlManager = new UrlManager();

                    emailer.SendEmail(employerEmail, Email.EmailTemplates.JobApplicationEmployer,
                        new Dictionary<string, string>() { 
                            {"FirstName", jobApplication.FirstName},
                            {"LastName", jobApplication.LastName},
                            {"Email", jobApplication.Email},
                            {"Phone", jobApplication.Phone},
                            {"CoverLetter", jobApplication.CoverLetter},
                            {"Resume", jobApplication.Resume},
                            {"DashboardLink", urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null)}
                        },
                        employerUser, jobPost);
                }

                //add email to job seeker upon application.
                emailer.SendEmail(email, Email.EmailTemplates.JobApplicationJobSeeker,
                    new Dictionary<string, string>() { { "FirstName", firstName }, { "LastName", lastName } },
                    jobPost);
            }
            return result;
        }

        public List<JobApplication> GetJobApplications(int jobPostId, int userId, int page, int pageSize, out int totalNumberOfResults)
        {
            totalNumberOfResults = 0;
            
            JobApplicationDataAccess jada = new JobApplicationDataAccess();
            List<JobApplication> result = jada.GetJobApplications(jobPostId, userId, page, pageSize, out totalNumberOfResults);

            return result;
        }
        
        /// <summary>
        /// Get job application for for user that owns the job posting
        /// </summary>
        /// <param name="jobApplicationId"></param>
        /// <returns></returns>
        public JobApplication GetJobApplication(int jobApplicationId, int userId)
        {
            JobApplicationDataAccess jada = new JobApplicationDataAccess();
            return jada.GetJobApplication(jobApplicationId, userId);
        }
    }
}
