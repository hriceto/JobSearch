using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class UrlDataAccess : DataAccess
    {
        private const string cacheKey = "Urls";

        public List<Url> GetUrls()
        {
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (List<Url>)HttpContext.Current.Cache[cacheKey];
            }
            else
            {
                lock (cacheKey)
                {
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        return (List<Url>)HttpContext.Current.Cache[cacheKey];
                    }
                    else
                    {
                        using (WorkEntities context = GetContext())
                        {
                            List<Url> results = (from u in context.Urls select u).ToList();
                            HttpContext.Current.Cache.Insert(cacheKey, results, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                            return results;
                        }
                    }
                }
            }
        }
    }
}
