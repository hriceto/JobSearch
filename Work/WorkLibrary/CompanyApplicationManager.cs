using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class CompanyApplicationManager
    {
        public int CreateCompanyApplication(int companyId, string numberOfEmployees, string numberOfPostsPerYear)
        {
            CompanyApplication companyApplication = CompanyApplication.CreateCompanyApplication(-1, companyId);
            companyApplication.NumberOfEmployees = numberOfEmployees;
            companyApplication.NumberOfPostsPerYear = numberOfPostsPerYear;
            CompanyApplicationDataAccess cada = new CompanyApplicationDataAccess();

            int companyApplicationId = cada.AddCompanyApplication(companyApplication);

            return companyApplicationId;
        }

        public List<CompanyApplication> GetCompanyApplication(int companyId)
        {
            CompanyApplicationDataAccess cada = new CompanyApplicationDataAccess();
            return cada.GetCompanyApplication(companyId);
        }
    }
}
