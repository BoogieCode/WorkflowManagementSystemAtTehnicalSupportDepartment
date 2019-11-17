using licenta.DatabaseConnection;
using licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace licenta.Controllers
{
    public class HomeController : Controller
    {
        private TehnicalDepartmentDb db = new TehnicalDepartmentDb();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Unassigned()
        {
            List<myIncidentRequestViewModel> models = new List<myIncidentRequestViewModel>();
            List<Request> requestsList = new List<Request>();

            string company = db.Users.Where(m => m.username == User.Identity.Name).FirstOrDefault().company;
            int? departmentIds = db.Users.Where(m => m.username == User.Identity.Name).FirstOrDefault().departmentId;

            string departmentName = db.Departments.Where(m => m.departmentId == departmentIds).First().name;
            //  List<Request> requests = db.Requests.Where(m => m.departmentAssigned == departmentName).ToList();


            requestsList = db.Requests.Where(m => db.Users.Any(l => l.company == company && departmentName == m.departmentAssigned)).ToList();

            foreach (var req in requestsList)
            {
                if (db.RequestHistories.Where(m => m.requestId == req.requestId).First().status.ToString() == "Pending")
                {
                    models.Add(new Models.myIncidentRequestViewModel
                    {
                        Id = req.requestId,
                        IncidentRequest = req.type == false ? "Incident" : "Request",
                        Title = req.title,
                        CreatedBy = db.Users.Where(m => m.userId == req.createdBy).FirstOrDefault().username,
                        DepartmentAssigned = req.departmentAssigned,
                        Priority = req.priority == 0 ? "Low" : req.priority == 1 ? "Medium" : "High",
                        Status = "Pending"
                    });
                }
            }


            return View(models);
        }

        public ActionResult Assigned(int? id)
        {
            try {
                Request req = db.Requests.Where(m => m.requestId == id).FirstOrDefault();
                RequestHistory reqHistory = db.RequestHistories.Where(m => m.requestId == id).FirstOrDefault();

                req.employeeAssigned = User.Identity.Name;
                reqHistory.status = "In progress";
                db.SaveChanges();

                return RedirectToAction("Contact");                //DE CREAT POPULARE FIELDULUI EMPLOYEE ASSIGNED IN REQ

            }
            catch {
                return View();

            }
        }
    }
} 