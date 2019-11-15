using licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace licenta.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult _Layout()
        {
            SharedViewModel model = new SharedViewModel
            {
                companyName = "asdasd"
        };
            return PartialView("asdsa");
        }
    }
}