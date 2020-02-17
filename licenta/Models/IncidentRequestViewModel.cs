using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace licenta.Models
{
    public class IncidentRequestViewModel
    {
        public IncidentRequestViewModel()
        {
            IncidentRequest.Add(new SelectListItem { Text = "Incident", Value = "0" });
            IncidentRequest.Add(new SelectListItem { Text = "Request", Value = "1" });

            priorityList.Add(new SelectListItem { Text = "Low", Value = "0" });
            priorityList.Add(new SelectListItem { Text = "Medium", Value = "1" });
            priorityList.Add(new SelectListItem { Text = "High", Value = "2" });
        }
        public string type { set; get; }
        public string createdBy { set; get; }
        public string title { set; get; }
        public string description { set; get; }
        public string departmentAssigned { set; get; }
        public string employeeAssigned { set; get; }
        public HttpPostedFileBase file { set; get; }
        public int priority { set; get; }



        public List<SelectListItem> IncidentRequest { get; set; } = new List<SelectListItem>();//done from constructor
        public List<SelectListItem> departmentsList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> employeeList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> priorityList { get; set; } = new List<SelectListItem>();//done from constructor
    }
}