using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace licenta.Models
{
    public class FilterModel
    {
        public FilterModel()
        {
            isCheckedType = new List<bool> { true, true };
            isCheckedStatus = new List<bool> { true, true, true,true };
            isCheckedCategory = new List<bool> { true, true, true };

        }
        //types
        public List<bool> isCheckedType { set; get; }
        //status
        public List<bool> isCheckedStatus { set; get; }

        //category
        public List<bool> isCheckedCategory { set; get; }

    }
}