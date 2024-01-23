using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class EmploymentTypeDataAccess : DataAccess
    {
        private const string cacheKey = "EmploymentTypes";

        public List<EmploymentType> GetEmploymentTypes()
        {
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (List<EmploymentType>)HttpContext.Current.Cache[cacheKey];
            }
            else
            {
                lock (cacheKey)
                {
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        return (List<EmploymentType>)HttpContext.Current.Cache[cacheKey];
                    }
                    else
                    {
                        using (WorkEntities context = GetContext())
                        {                            
                            var employmentTypesQuery = from et in context.EmploymentTypes select et;
                            var employmentTypes = employmentTypesQuery.ToList();
                            HttpContext.Current.Cache.Insert(cacheKey, employmentTypes, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                            return employmentTypes;
                        }
                    }
                }
            }
        }
    }
}
