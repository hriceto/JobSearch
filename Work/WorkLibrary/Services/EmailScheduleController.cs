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
    public class EmailScheduleController : ApiController
    {
        private List<int> FreeJobPostingAvailableReminderEmailIntervals = new List<int>() { 3, 10, 30, 60, 90, 150, 210, 330, 500 };

        [HttpGet]
        [BasicAuthentication(Users="schedulerunner")]
        public bool SendFreeJobPostingAvailableReminderEmails()
        {
            bool result = false;

            try
            {
                UrlManager urlManager = new UrlManager();
                string jobsPage = urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null);

                UserManager userManager = new UserManager();
                List<User> users = userManager.GetUsersWithUnusedFreeJobPostForEmailReminder();

                foreach (User user in users)
                {
                    DateTime? freeAdAvailableSince = user.Company.ReviewedDate;
                    DateTime? lastFreeJobPost = (from jp in user.Company.JobPosts 
                                          orderby jp.StartDate descending 
                                          select jp.StartDate).Take(1).SingleOrDefault();

                    int freeAdsRefreshInterval = Int32.Parse(WebConfigurationManager.AppSettings["FREE_ADS_REFRESH_INTERVAL_DAYS"]);
                    if (lastFreeJobPost.HasValue)
                    {
                        freeAdAvailableSince = ((DateTime)lastFreeJobPost).AddDays(freeAdsRefreshInterval);
                    }

                    if (freeAdAvailableSince.HasValue)
                    {
                        int freeAdAvailableSinceDays = DateTime.Now.Subtract((DateTime)freeAdAvailableSince).Days;
                        
                        if (freeAdAvailableSinceDays > 0)
                        {
                            //send email
                            if (FreeJobPostingAvailableReminderEmailIntervals.Contains(freeAdAvailableSinceDays))
                            {
                                Email email = new Email();
                                email.SendEmail(user.Email, Email.EmailTemplates.FreeJobPostingAvailableReminder,
                                    new Dictionary<string, string>() { 
                                        { "freeJobAvailableFor", freeAdAvailableSinceDays.ToString() },
                                        {"JobsPage", jobsPage},
                                        {"freeAdsRefreshInterval", freeAdsRefreshInterval.ToString()},
                                        {"unsubscribeUrl", 
                                            urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Unsubscribe, 
                                                new Dictionary<string,string>(){
                                                {"unsubscribeid", user.OkToEmailGuid.ToString()},
                                                {"email", user.Email}
                                                }) }
                                    }, user);

                                LogManager logManager = new LogManager();
                                logManager.AddLog("A reminder email was sent to " + user.Email, user.UserId, "days:" + freeAdAvailableSinceDays, "");
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }
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
