using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class EmploymentTypeManager
    {
        public List<EmploymentType> GetEmploymentTypes()
        {
            EmploymentTypeDataAccess etda = new EmploymentTypeDataAccess();
            return etda.GetEmploymentTypes();
        }
    }
}
