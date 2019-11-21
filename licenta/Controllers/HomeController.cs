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
        [Authorize(Roles = "Administrator,Employee")]
        public ActionResult Unassigned()
        {
            List<myIncidentRequestViewModel> models = new List<myIncidentRequestViewModel>();
            List<Request> requestsList = new List<Request>();

            string company = Request.Cookies["companyCookie"].Value;
            int? departmentIds = db.Users.Where(m => m.username == User.Identity.Name&&m.company==company).FirstOrDefault().departmentId;

            string departmentName = db.Departments.Where(m => m.departmentId == departmentIds).First().name;
            //  List<Request> requests = db.Requests.Where(m => m.departmentAssigned == departmentName).ToList();


            requestsList = db.Requests.Where(m => db.Users.Any(l => l.company == company && departmentName == m.departmentAssigned && m.createdBy == l.userId)).ToList();

            foreach (var req in requestsList)
            {
                //  DE REFACUT CAND VIN APROVALURILE
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
        [Authorize(Roles = "Employee")]
        public ActionResult Assigned(int? id)
        {
            try {
                Request req = db.Requests.Where(m => m.requestId == id).FirstOrDefault();
                RequestHistory reqHistory = db.RequestHistories.Where(m => m.requestId == id).FirstOrDefault();

                req.employeeAssigned = User.Identity.Name;
                reqHistory.status = "In progress";
                db.SaveChanges();

                return RedirectToAction("Contact");             

            }
            catch {
                return View();

            }
        }
        [Authorize(Roles = "Employee")]
        public ActionResult Dashboard()
        {
            //HERE EMPLOYEES SHOULD SEE ALL THEIR ASSIGNED INCIDENTS OR REQUESTS!
            //THEY SHOULD HAVE THE OPTION TO SEND THEM TO OTHER DEP OR CHECK IT 
            //AS DONE WITH "EDIT STATE" BUTTON


            List<myIncidentRequestViewModel> models = new List<myIncidentRequestViewModel>();
            List<Request> requestsList = new List<Request>();

            string company = Request.Cookies["companyCookie"].Value;
            int? departmentIds = db.Users.Where(m => m.username == User.Identity.Name&& m.company==company).FirstOrDefault().departmentId;

            string departmentName = db.Departments.Where(m => m.departmentId == departmentIds).First().name;
            //  List<Request> requests = db.Requests.Where(m => m.departmentAssigned == departmentName).ToList();


            requestsList = db.Requests.Where(m => db.Users.Any(l => l.company == company && departmentName == m.departmentAssigned&&m.createdBy==l.userId)).ToList();

            //requestsList = db.Requests.Where(m => m.createdBy == db.Users.Where(l => l.userId == m.createdBy).FirstOrDefault().userId).ToList();

            foreach (var req in requestsList)
            {
                if (db.RequestHistories.Where(m => m.requestId == req.requestId).First().status.ToString() != "Pending")
                {

                    models.Add(new Models.myIncidentRequestViewModel
                    {
                        Id = req.requestId,
                        IncidentRequest = req.type == false ? "Incident" : "Request",
                        Title = req.title,
                        CreatedBy = db.Users.Where(m => m.userId == req.createdBy).FirstOrDefault().username,
                        DepartmentAssigned = req.departmentAssigned,
                        Priority = req.priority == 0 ? "Low" : req.priority == 1 ? "Medium" : "High",
                        Status = "In progress"
                    });
                }
            }




            return View(models);
        }

        public ActionResult ModalPartialSendView(int id)
        {
            return PartialView();
        }
    }
} 