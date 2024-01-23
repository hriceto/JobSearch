using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects
{
    public class JobSearchResult
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private string _company;
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        private int _jobPostId;
        public int JobPostId
        {
            get { return _jobPostId; }
            set { _jobPostId = value; }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private string _seUrl;
        public string SeUrl
        {
            get { return _seUrl; }
            set { _seUrl = value; }
        }

        private string _seDescription;
        public string SeDescription
        {
            get { return _seDescription; }
            set { _seDescription = value; }
        }
    }
}
