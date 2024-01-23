using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class DataAccess
    {
        /// <summary>
        /// Gets context to entity model.
        /// </summary>
        /// <returns></returns>
        protected WorkEntities GetContext()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["WorkSQL"].ConnectionString;
            WorkEntities context = new WorkEntities("metadata=res://*/WorkModel.csdl|res://*/WorkModel.ssdl|res://*/WorkModel.msl;provider=System.Data.SqlClient;provider connection string=\"" + connectionString + ";MultipleActiveResultSets=True\"");
            return context;
        }
    }
}
