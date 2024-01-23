using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class EmailDataAccess : DataAccess
    {
        public bool SaveEmailInBacklog(EmailBacklog message)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                context.EmailBacklogs.AddObject(message);
                if (context.SaveChanges() == 1)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
