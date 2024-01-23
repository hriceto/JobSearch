using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class LogManager
    {
        public void AddLog(string message, int userId, string variable1, string variable2)
        {
            Log log = WorkDal.Log.CreateLog(-1);
            log.CreatedDate = DateTime.Now;
            log.Page = HttpContext.Current.Request.Url.AbsoluteUri;
            log.Message = message;
            log.UserId = userId;
            log.Variable1 = variable1;
            log.Variable2 = variable2;

            LogDataAccess lda = new LogDataAccess();
            lda.AddLog(log);
        }
    }
}
