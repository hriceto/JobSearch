using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class ExceptionManager
    {
        public void AddException(System.Exception ex)
        {
            HristoEvtimov.Websites.Work.WorkDal.Exception exception = WorkDal.Exception.CreateException(-1);
            foreach(KeyValuePair<string, string> dataPiece in ex.Data)
            {
                exception.Data += "<" + dataPiece.Key + ">" + dataPiece.Value + "</" + dataPiece.Key + ">";
            }
            exception.HelpLink = ex.HelpLink;
            exception.Message = ex.Message;
            exception.Source = ex.Source;
            exception.StackTrace = ex.StackTrace;
            exception.CreatedDate = DateTime.Now;

            if (ex.InnerException != null)
            {
                if (ex.InnerException.Message != null)
                {
                    exception.Message += ex.InnerException.Message;
                }
                if (ex.InnerException.Source != null)
                {
                    exception.Source += ex.InnerException.Source;
                }
                if (ex.InnerException.StackTrace != null)
                {
                    exception.StackTrace +=  ex.InnerException.StackTrace;
                }
            }

            ExceptionDataAccess eda = new ExceptionDataAccess();
            eda.AddException(exception);
        }
    }
}
