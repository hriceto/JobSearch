using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class CompanyDataAccess : DataAccess
    {
        /// <summary>
        /// add a company. return companyid or -1 successful
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int AddCompany(Company company)
        {
            int result = -1;
            using (WorkEntities context = GetContext())
            {
                context.Companies.AddObject(company);
                if (context.SaveChanges() == 1)
                {
                    result = company.CompanyId;
                }
            }
            return result;
        }

        public bool UpdateCompany(Company company)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                if (company != null)
                {
                    context.Companies.Attach(company);
                    context.ObjectStateManager.ChangeObjectState(company, System.Data.EntityState.Modified);
                    result = (context.SaveChanges() == 1);
                }
            }
            return result;
        }

        public Company GetCompany(int userId)
        {
            Company result = null;
            using (WorkEntities context = GetContext())
            {
                result = (from u in context.Users
                          where u.UserId == userId && u.CompanyId > 0
                          select u.Company).FirstOrDefault();
            }
            return result;
        }

        public List<Company> GetCompaniesWaitingForReview()
        {
            List<Company> result = null;
            using (WorkEntities context = GetContext())
            {
                List<int> companyIds = (from u in context.Users
                                        join aspnet in context.aspnet_Membership on u.Email equals aspnet.Email
                                        where u.IsCompanyAdmin == true &&
                                        aspnet.IsApproved == true &&
                                        u.Company.Reviewed == false
                                        select u.Company.CompanyId).Take(10).ToList();


                result = (from c in context.Companies.Include("Users")
                          where companyIds.Contains(c.CompanyId)
                          select c).ToList();
            }
            return result;
        }

        public Company GetCompanyForReview(int companyId)
        {
            Company result = null;
            using (WorkEntities context = GetContext())
            {
                result = (from c in context.Companies.Include("Users").Include("CompanyApplications")
                          where c.CompanyId == companyId
                          select c).FirstOrDefault();
            }
            return result;
        }

        public Company GetCompanyByDomain(string domain)
        {
            Company result = null;
            using (WorkEntities context = GetContext())
            {
                result = (from c in context.Companies
                          where c.CompanyDomain == domain
                          select c).FirstOrDefault();
            }
            return result;
        }

        public List<Company> AdminSearchForCompanies(string name, string email, bool approved, DateTime createdDateFrom, DateTime createdDateTo, int page, int pageSize, out int totalNumberOfResults)
        {
            List<Company> result = null;
            using (WorkEntities context = GetContext())
            {
                var companyIds = (from u in context.Users
                               join aspnet in context.aspnet_Membership on u.Email equals aspnet.Email
                               where u.Company.Name.Contains(name) &&
                               u.Email.Contains(email) &&
                               u.IsCompanyAdmin == true &&
                               u.Company.CreatedDate >= createdDateFrom &&
                               u.Company.CreatedDate <= createdDateTo &&
                               aspnet.IsApproved == approved 
                               orderby u.Company.CreatedDate descending
                               select u.Company.CompanyId);

                var companies = (from c in context.Companies.Include("Users")
                                 where companyIds.Contains(c.CompanyId)
                          orderby c.CreatedDate descending
                                 select c);

                totalNumberOfResults = companies.Count();
                result = companies.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                
            }
            return result;
        }
    }
}
