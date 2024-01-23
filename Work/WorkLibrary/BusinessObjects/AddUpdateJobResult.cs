using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects
{
    public class AddUpdateJobResult
    {
        public bool Success { get; set; }
        public int JobPostId { get; set; }
        public bool JobTooShort { get; set; }
    }
}
