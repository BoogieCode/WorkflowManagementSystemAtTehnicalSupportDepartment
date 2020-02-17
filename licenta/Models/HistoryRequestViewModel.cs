using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace licenta.Models
{
    public class HistoryRequestViewModel
    {
        public HistoryRequestViewModel()
        {
            histories = new List<HistoryRequestModel>();
        }
        public string title { set; get; }
        public string type { set; get; }
        public string description { set; get; }
        public string priority { set; get; }
        public int? download { set; get; }

        public List<HistoryRequestModel> histories { set; get; }
       
    }
}