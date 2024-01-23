using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class CategoryDataAccess : DataAccess
    {
        private const string cacheKey = "Categories";

        public List<Category> GetCategories(bool refreshFromDatabase)
        {
            if (HttpContext.Current.Cache[cacheKey] != null && !refreshFromDatabase)
            {
                return (List<Category>)HttpContext.Current.Cache[cacheKey];
            }
            else
            {
                lock (cacheKey)
                {
                    if (HttpContext.Current.Cache[cacheKey] != null && !refreshFromDatabase)
                    {
                        return (List<Category>)HttpContext.Current.Cache[cacheKey];
                    }
                    else
                    {
                        using (WorkEntities context = GetContext())
                        {
                            var categoriesQuery = from c in context.Categories orderby c.Name ascending select c;
                            var categories = categoriesQuery.ToList();
                            HttpContext.Current.Cache.Insert(cacheKey, categories, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                            return categories;
                        }
                    }
                }
            }
        }

        public Category GetCategory(int categoryId)
        {
            using (WorkEntities context = GetContext())
            {
                var categoryQuery = from c in context.Categories where c.CategoryId == categoryId select c;
                return categoryQuery.FirstOrDefault();
            }
        }

        public bool UpdateCategory(Category category)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                if (category != null)
                {
                    context.Categories.Attach(category);
                    context.ObjectStateManager.ChangeObjectState(category, System.Data.EntityState.Modified);
                    result = (context.SaveChanges() == 1);
                }
            }
            return result;
        }
    }
}
