using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class MenuDataAccess : DataAccess
    {
        public List<MenuItem> GetMenuDataAll()
        {
            using (WorkEntities context = GetContext())
            {
                var results = from u in context.MenuItems.Include("MenuType").Include("Url") orderby u.SortOrder ascending select u;
                return results.ToList();
            }
        }
    }
}
