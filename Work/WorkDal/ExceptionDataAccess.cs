using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class ExceptionDataAccess : DataAccess
    {
        public void AddException(HristoEvtimov.Websites.Work.WorkDal.Exception exception)
        {
            using (WorkEntities context = GetContext())
            {
                context.Exceptions.AddObject(exception);
                context.SaveChanges();
            }
        }
    }
}
