using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace licenta.Models
{

    public class CompanyUsersRegisterViewModel
    {
     

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User")]
        public string user { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }



        public string userTypeId { get; set; }

        public string departmentName { get; set; }

        public List<SelectListItem> userTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> departmentsList { get; set; } = new List<SelectListItem>();



    }

 
}