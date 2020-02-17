using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace licenta.Models
{
    public class myFilteredIncidentRequestViewModel
    {
        public myFilteredIncidentRequestViewModel()
        {
            myIncidents = new List<myIncidentRequestViewModel>();
            fm = new FilterModel();
            if (orderedBy == null)
            {
                orderedBy = new Dictionary<string, int>();
                orderedBy.Add("From Latest to Earliest", 0);
                orderedBy.Add("From Earliest to Latest", 1);
            }
            incidentrequestCount = new Dictionary<string, int>();
            statusCount = new Dictionary<string, int>();
            depassignedCount = new Dictionary<string, int>();
        }
        public FilterModel fm { get; set; }
        public List<myIncidentRequestViewModel> myIncidents {get;set;}
        public Dictionary<string, int> orderedBy { get; set; }
        public string orderVal { get; set; }

        public Dictionary<string,int> incidentrequestCount;
        public Dictionary<string, int> statusCount;
        public Dictionary<string, int> depassignedCount;
    }
}