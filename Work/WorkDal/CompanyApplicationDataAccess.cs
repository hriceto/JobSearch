using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class CompanyApplicationDataAccess : DataAccess
    {
        /// <summary>
        /// add a company. return companyid or -1 successful
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int AddCompanyApplication(CompanyApplication companyApplication)
        {
            int result = -1;
            using (WorkEntities context = GetContext())
            {
                context.CompanyApplications.AddObject(companyApplication);
                if (context.SaveChanges() == 1)
                {
                    result = companyApplication.CompanyApplicationId;
                }
            }
            return result;
        }

        public List<CompanyApplication> GetCompanyApplication(int companyId)
        {
            List<CompanyApplication> result = null;
            using (WorkEntities context = GetContext())
            {
                result = (from ca in context.CompanyApplications
                          where ca.CompanyId == companyId
                          select ca).ToList();
            }
            return result;
        }
    }
}
