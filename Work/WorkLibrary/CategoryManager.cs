using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class CategoryManager
    {
        public List<Category> GetCategories()
        {
            return GetCategories(false);
        }

        public List<Category> GetCategories(bool refreshFromDatabase)
        {
            CategoryDataAccess cda = new CategoryDataAccess();
            return cda.GetCategories(refreshFromDatabase);
        }

        public Category GetCategory(int categoryId)
        {
            CategoryDataAccess cda = new CategoryDataAccess();
            return cda.GetCategory(categoryId);
        }

        //each category holds the current number of jobs in that category
        public bool UpdateNumberOfJobsInCategories(List<JobSearchIndex> jobs)
        {
            CategoryDataAccess cda = new CategoryDataAccess();
            bool result = false;
            try
            {
                List<Category> categories = GetCategories();
                foreach (Category category in categories)
                {
                    int numberOfJobsInCategory = 0;
                    foreach (JobSearchIndex job in jobs)
                    {
                        foreach (JobPostCategory jobCategory in job.JobPostCategories)
                        {
                            if (jobCategory.CategoryId == category.CategoryId)
                            {
                                numberOfJobsInCategory += 1;
                            }
                        }
                    }

                    if (category.NumberOfActiveJobs != numberOfJobsInCategory)
                    {
                        //number of jobs in category has changed. Update it.
                        category.NumberOfActiveJobs = numberOfJobsInCategory;
                        cda.UpdateCategory(category);
                    }
                }
                result = true;
            }
            catch (System.Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
            }
            return result;
        }
    }
}
