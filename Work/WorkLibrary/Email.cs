using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading.Tasks;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class Email
    {
        public enum EmailTemplates
        {
            VerifyEmail,
            ResetPasswordEmail,
            OrderConfirmation,
            AdminAccountUpdate,
            AdminJobSuspended,
            JobSuspended,
            AdminJobReviewed,
            UpdatePassword,
            EmailVerified,
            JobApplicationEmployer,
            JobApplicationJobSeeker,
            RunJobRefreshSuccess,
            RunJobRefreshFailure,
            ContactUs,
            AdminAlertEmployerRegistration,
            EmployerRoleAssignment,
            FreeJobPostingAvailableReminder,
            CouponCreation,
            AdminAlertJobPosted,
            ResumeHelp,
            AdminUnlimitedFreeJobsApproved,
        };

        public Email()
        {

        }

        public bool SendEmail(string sendTo, EmailTemplates emailTemplate, Dictionary<string, string> stringParameters, params object[] objectParameters)
        {
            bool result = false;
            string host = HttpContext.Current.Request.Url.Host;
            if (stringParameters == null)
            {
                stringParameters = new Dictionary<string, string>();
            }
            stringParameters.Add("CustomerServiceEmail", WebConfigurationManager.AppSettings["CUSTOMER_SERVICE_EMAIL"]);
            stringParameters.Add("CustomerServicePhone", WebConfigurationManager.AppSettings["CUSTOMER_SERVICE_PHONE"]);
            stringParameters.Add("Domain", host);

            //get email contents from xslt template
            XsltTemplating templating = new XsltTemplating();
            string emailText = templating.GetTransformedTemplate(XsltTemplating.TemplatePath.Email, emailTemplate.ToString(),
                stringParameters, objectParameters);
            if (!String.IsNullOrEmpty(emailText))
            {
                //send email in a separate task so this does not delay the rest of the program
                Task.Factory.StartNew(() => SendEmailAsync(sendTo, emailTemplate, emailText, host));
                result = true;
            }

            return result;
        }

        private void SendEmailAsync(string sendTo, EmailTemplates emailTemplate, string emailText, string host)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(sendTo);

            ResourceManager rm = new ResourceManager("Resources.EmailSubjects", System.Reflection.Assembly.Load("App_GlobalResources"));
            message.Subject = rm.GetString(emailTemplate.ToString());
            message.Subject = message.Subject.Replace("{host}", host);

            message.From = new System.Net.Mail.MailAddress(WebConfigurationManager.AppSettings["EMAIL_FROM"]);
            message.Body = emailText;
            message.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(WebConfigurationManager.AppSettings["SMTP_SERVER"]);
            if (!String.IsNullOrEmpty(WebConfigurationManager.AppSettings["SMTP_SERVER_USERNAME"]))
            {
                smtp.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["SMTP_SERVER_USERNAME"],
                    WebConfigurationManager.AppSettings["SMTP_SERVER_PASSWORD"]);
            }
            if (!String.IsNullOrEmpty(WebConfigurationManager.AppSettings["SMTP_SERVER_USE_SSL"]))
            {
                smtp.EnableSsl = Boolean.Parse(WebConfigurationManager.AppSettings["SMTP_SERVER_USE_SSL"]);
            }

            try
            {
                smtp.Send(message);
            }
            catch (System.Exception ex)
            {
                //save email if fail
                SaveEmailInBacklog(message);

                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
            }
        }

        private void SaveEmailInBacklog(System.Net.Mail.MailMessage message)
        {
            EmailBacklog backlogMessage = new EmailBacklog();
            backlogMessage.Body = message.Body;
            backlogMessage.CreatedDate = DateTime.Now;
            backlogMessage.EmailFrom = message.From.Address;
            foreach(var toAddress in message.To)
            {
                backlogMessage.EmailTo += toAddress.Address + ";";
            }
            backlogMessage.EmailTo = backlogMessage.EmailTo.Trim(new char[] { ';' });
            backlogMessage.IsHTML = message.IsBodyHtml;
            backlogMessage.Subject = message.Subject;

            EmailDataAccess eda = new EmailDataAccess();
            eda.SaveEmailInBacklog(backlogMessage);
        }

        public void Send_EmployerRoleAssignment_Email(User user, Company company)
        {
            UrlManager urlManager = new UrlManager();
            string jobsPage = urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null);
            string loginLink = urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null);

            if (company != null)
            {
                SendEmail(user.Email, Email.EmailTemplates.EmployerRoleAssignment,
                new Dictionary<string, string>() { { "LoginLink", loginLink }, { "JobsPage", jobsPage } },
                user, company);
            }
        }
    }
}
