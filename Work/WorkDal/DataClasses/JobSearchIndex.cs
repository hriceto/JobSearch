using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class JobSearchIndex
    {
        public JobSearchIndex()
        {
        }

        private JobPost _job;
        public JobPost Job
        {
            get { return _job; }
            set { _job = value; }
        }

        private ZipCode _zip;
        public ZipCode Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }

        private Company _jobCompany;
        public Company JobCompany
        {
            get { return _jobCompany; }
            set { _jobCompany = value; }
        }

        private EmploymentType _jobEmploymentType;
        public EmploymentType JobEmploymentType
        {
            get { return _jobEmploymentType; }
            set { _jobEmploymentType = value; }
        }

        private List<JobPostCategory> _jobPostCategories;
        public List<JobPostCategory> JobPostCategories
        {
            get { return _jobPostCategories; }
            set { _jobPostCategories = value; }
        }
    }
}
