using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class LogDataAccess : DataAccess
    {
        public void AddLog(Log log)
        {
            using (WorkEntities context = GetContext())
            {
                context.Logs.AddObject(log);
                context.SaveChanges();
            }
        }
    }
}
