using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace licenta.Models
{
    public class HistoryRequestModel
    {
        public string from { set; get; }
        public string to { set; get; }
        public string message { set; get; }
        public DateTime data { set; get; }
        public string status { set; get; }
        public string approvaltype { set; get; }

    }
}