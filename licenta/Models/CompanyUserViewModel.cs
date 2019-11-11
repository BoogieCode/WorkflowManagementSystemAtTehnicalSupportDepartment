using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace licenta.Models
{
    public class CompanyUserViewModel
    {
        public int userId;
        [Display(Name = "username")]
        public string username{ set; get; }

        [Display(Name = "email")]
        public string email{ set; get; }

        [Display(Name = "type")]
        public string type{ set; get; }

        [Display(Name = "department")]
        public string department{ set; get; }

        [Display(Name = "role")]
        public string role{ set; get; }
    }
}