using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace licenta.Models
{
    public class SendIncidentRequestViewModel
    {
        int ID { set; get; }
        string Category { set; get; }
        string Message { set; get; }
        bool NeedsApproval { set; get; }
        string Status { get; set; }
    }
}