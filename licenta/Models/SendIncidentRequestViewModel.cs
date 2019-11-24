using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace licenta.Models
{
    public class SendIncidentRequestViewModel
    {

        public SendIncidentRequestViewModel()
        {
            Approval.Add(new SelectListItem { Text = "NO", Value = "0" });
            Approval.Add(new SelectListItem { Text = "YES", Value = "1" });

        }

       public int ID { set; get; }
       public string Category { set; get; }
       public string Message { set; get; }
       public string NeedsApproval { set; get; }
       public string Status { get; set; }
       public List<SelectListItem> departmentsList { get; set; } = new List<SelectListItem>();
       public List<SelectListItem> Approval { get; set; } = new List<SelectListItem>();

    }
}