using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class CompanyManager
    {
        public int CreateCompany(string companyName, string companyAddress1, string companyAddress2,
            string companyCity, string companyState, string companyZip, string companyCountry,
            string companyPhone, string companyWebsite, bool isRecruiter, int companyCreatedByUserId,
            string email)
        {
            int companyId = -1;

            bool allowFreeJobPosts = true;
            
            WorkDal.Company company = WorkDal.Company.CreateCompany(-1, companyName, companyAddress1,
                companyCity, companyState, companyZip, companyCountry, isRecruiter, allowFreeJobPosts, DateTime.Now, companyCreatedByUserId, false);
            company.Address2 = companyAddress2;
            company.Phone = companyPhone;
            company.Website = companyWebsite;
            CompanyDataAccess cda = new CompanyDataAccess();

            companyId = cda.AddCompany(company);

            return companyId;
        }

        public bool UpdateCompany(int userId, string companyAddress1, string companyAddress2,
            string companyCity, string companyState, string companyZip, string companyCountry,
            string companyPhone, string companyWebsite)
        {
            bool success = false;
            Company company = GetCompany(userId);

            if (company != null)
            {
                company.Address1 = companyAddress1;
                company.Address2 = companyAddress2;
                company.City = companyCity;
                company.State = companyState;
                company.Zip = companyZip;
                company.Country = companyCountry;
                company.Phone = companyPhone;
                company.Website = companyWebsite;

                CompanyDataAccess cda = new CompanyDataAccess();
                success = cda.UpdateCompany(company);
            }
            return success;
        }

        public bool UpdateCompanyReviewed(int companyId, bool isRecruiter, bool allowFreeJobPosts, string companyDomain, string allowFreePostsReasonText, bool allowUnlimitedFreePosts)
        {
            bool result = false;

            UserManager userManager = new UserManager();
            User currentUser = userManager.GetUser();

            Company company = GetCompanyForReview(companyId);

            if (company != null)
            {
                company.IsRecruiter = isRecruiter;
                company.AllowFreeJobPosts = allowFreeJobPosts;
                company.CompanyDomain = companyDomain;

                company.Reviewed = true;
                company.ReviewedByUserId = currentUser.UserId;
                company.ReviewedDate = DateTime.Now;

                //send email
                if ((!company.AllowUnlimitedFreeJobPosts.HasValue || (company.AllowUnlimitedFreeJobPosts.HasValue && !company.AllowUnlimitedFreeJobPosts.Value))
                    && allowUnlimitedFreePosts)
                {
                    company.AllowUnlimitedFreeJobPosts = allowUnlimitedFreePosts;
                    Email email = new Email();
                    UrlManager urlManager = new UrlManager();
                    string jobsPage = urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null);
                    
                    foreach(User companyUser in company.Users)
                    {
                        email.SendEmail(companyUser.Email, Email.EmailTemplates.AdminUnlimitedFreeJobsApproved, 
                            new Dictionary<string, string>() { {"JobsPage", jobsPage} }, company, companyUser);
                    }
                }
                company.AllowUnlimitedFreeJobPosts = allowUnlimitedFreePosts;

                CompanyDataAccess cda = new CompanyDataAccess();
                result = cda.UpdateCompany(company);
            }

            return result;
        }

        public Company GetCompany(int userId)
        {
            CompanyDataAccess cda = new CompanyDataAccess();
            return cda.GetCompany(userId);
        }

        //get companies that are not reviewed for admin use.
        public List<Company> GetCompaniesWaitingForReview()
        {
            CompanyDataAccess cda = new CompanyDataAccess();
            return cda.GetCompaniesWaitingForReview();
        }

        //get company that is not reviewed by companyid for admin use
        public Company GetCompanyForReview(int companyId)
        {
            CompanyDataAccess cda = new CompanyDataAccess();
            return cda.GetCompanyForReview(companyId);
        }

        public Company GetCompanyByUserEmail(string email)
        {
            string[] emailparts = email.Split(new char[] { '@' });
            if (emailparts.Length == 2)
            {
                string domain = emailparts[1];
                CompanyDataAccess cda = new CompanyDataAccess();
                return cda.GetCompanyByDomain(domain);
            }
            return null;
        }

        public List<Company> AdminSearchForCompanies(string name, string email, bool approved, string sCreatedDateFrom, string sCreatedDateTo, int page, int pageSize, out int totalNumberOfResults)
        {
            CompanyDataAccess cda = new CompanyDataAccess();
            DateTime createdDateFrom = DateTime.MinValue;
            DateTime createdDateTo = DateTime.MaxValue;
            if (!String.IsNullOrEmpty(sCreatedDateFrom))
            {
                createdDateFrom = DateTime.Parse(sCreatedDateFrom);
            }
            if (!String.IsNullOrEmpty(sCreatedDateTo))
            {
                createdDateTo = DateTime.Parse(sCreatedDateTo);
            }
            return cda.AdminSearchForCompanies(name, email, approved, createdDateFrom, createdDateTo, page, pageSize, out totalNumberOfResults);
        }
    }
}
