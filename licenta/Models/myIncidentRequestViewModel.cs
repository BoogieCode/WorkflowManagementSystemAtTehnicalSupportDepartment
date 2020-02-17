using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace licenta.Models
{
    public class myIncidentRequestViewModel
    {
     
        public int Id;
        public string IncidentRequest { set; get; }
        public string Title { set; get; }
        public string CreatedBy { get; set; }
        public string DepartmentAssigned { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }

        
    }
}