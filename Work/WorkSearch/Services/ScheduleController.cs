using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Services.Authentication;

namespace HristoEvtimov.Websites.Work.WorkSearch.Services
{
    public class ScheduleController : ApiController
    {
        [HttpGet]
        [BasicAuthentication(Users="schedulerunner")]
        public bool RunJobRefresh()
        {
            bool result = false;

            try
            {
                SettingActiveIndex settingActiveIndex = new SettingActiveIndex();
                JobSearch.pathToIndex path = settingActiveIndex.GetPathToActiveIndex();

                //invert path
                path = settingActiveIndex.InvertPath(path);

                JobSearch jobSearch = new JobSearch();
                JobManager jobManager = new JobManager();
                List<JobSearchIndex> jobs = jobManager.GetJobPostsForSearch();
                result = jobSearch.RecreateJobIndex(jobs, path);

                if (result)
                {
                    result = settingActiveIndex.UpdatePathToActiveIndex(path);
                }

                if (result)
                {
                    CategoryManager categoryManager = new CategoryManager();
                    result = categoryManager.UpdateNumberOfJobsInCategories(jobs);
                }

                if (!result)
                {
                    Email email = new Email();
                    email.SendEmail(WebConfigurationManager.AppSettings["EMAIL_TO"], Email.EmailTemplates.RunJobRefreshFailure, null);
                }
            }
            catch (System.Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
