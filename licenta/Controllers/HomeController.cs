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
            int? departmentIds = db.Users.Where(m => m.username == User.Identity.Name && m.company == company).FirstOrDefault().departmentId;

            string departmentName = db.Departments.Where(m => m.departmentId == departmentIds).First().name;
            //  List<Request> requests = db.Requests.Where(m => m.departmentAssigned == departmentName).ToList();


            requestsList = db.Requests.Where(m => db.Users.Any(l => l.company == company && departmentName == m.departmentAssigned && m.createdBy == l.userId)).ToList();

            foreach (var req in requestsList)
            {
                //  DE REFACUT CAND VIN APROVALURILE
                if (db.RequestHistories.Where(m => m.requestId == req.requestId).First().status.ToString() != "Done"
                    && db.Requests.FirstOrDefault(m => m.requestId == req.requestId).employeeAssigned == null)
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
            try
            {
                Request req = db.Requests.Where(m => m.requestId == id).FirstOrDefault();
                RequestHistory reqHistory = db.RequestHistories.Where(m => m.requestId == id).FirstOrDefault();

                req.employeeAssigned = User.Identity.Name;
                reqHistory.status = "In Progress";
                db.SaveChanges();

                return RedirectToAction("Contact");

            }
            catch
            {
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
            List<Request> allrequestsList = new List<Request>();
            List<RequestHistory> allrequestsHistoryList = new List<RequestHistory>();

            string company = Request.Cookies["companyCookie"].Value;
            int? departmentIds = db.Users.Where(m => m.username == User.Identity.Name && m.company == company).FirstOrDefault().departmentId;

            string departmentName = db.Departments.Where(m => m.departmentId == departmentIds).First().name;
            //  List<Request> requests = db.Requests.Where(m => m.departmentAssigned == departmentName).ToList();


            allrequestsList = db.Requests.ToList();
            requestsList = allrequestsList.Where(m => db.Users.Any(l => l.company == company && departmentName == m.departmentAssigned && m.createdBy == l.userId)).ToList();
            allrequestsList = allrequestsList.OrderByDescending(c => c.requestId).ToList();
            //requestsList = db.Requests.Where(m => m.createdBy == db.Users.Where(l => l.userId == m.createdBy).FirstOrDefault().userId).ToList();

            //requestsList = requestsList.OrderByDescending(c => c.requestId).ToList();




            foreach (var req in requestsList)
            {

                allrequestsHistoryList = db.RequestHistories.Where(l => l.requestId == req.requestId).ToList();
                allrequestsHistoryList = allrequestsHistoryList.OrderByDescending(l => l.requestHistoryId).ToList();

                if (allrequestsHistoryList.First().to == req.departmentAssigned)
                {
                    string status = db.RequestHistories.Where(m => m.requestId == req.requestId).First().status.ToString();
                    if ((status != "Pending" || status != "Done")
                        && db.Requests.FirstOrDefault(m => m.requestId == req.requestId).employeeAssigned == User.Identity.Name)
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
            }




            return View(models);
        }

        [HttpGet]
        public ActionResult ModalPartialSendView(int? userid)
        {
            //WHY YOU DON T GET THE REAL VALUE
            SendIncidentRequestViewModel model = new SendIncidentRequestViewModel();
            if (userid != null)
                model.ID = Int32.Parse(userid.ToString());
            List<Department> departments = db.Departments.ToList();
            foreach (var d in departments)
            {
                model.departmentsList.Add(new SelectListItem { Text = d.name, Value = d.name.ToString() });

            }

            return PartialView(model);

        }

        [HttpPost]
        public ActionResult ModalPartialSendView(SendIncidentRequestViewModel model)
        {
            RequestHistory requestHistory = db.RequestHistories.Where(m => m.requestId == model.ID).FirstOrDefault();
            Request request = db.Requests.FirstOrDefault(m => m.requestId == requestHistory.requestId);

            request.employeeAssigned = null;
            request.departmentAssigned = model.Category;
            db.SaveChanges();


            string statusState;
            if (model.NeedsApproval == "1")
            {
                statusState = "Waiting for approval";
            }
            else
            {
                statusState = "In Progress";
            }

            RequestHistory newrequestHistory = new RequestHistory
            {
                requestId = model.ID,
                from = requestHistory.to,
                to = model.Category,
                data = DateTime.Now,
                message = model.Message,
                approval = model.NeedsApproval.ToString(),
                status = statusState

            };

            db.RequestHistories.Add(newrequestHistory);
            db.SaveChanges();




            return RedirectToAction("Dashboard");
        }



        [HttpGet]
        public ActionResult History(int id)
        {
            Request request = db.Requests.FirstOrDefault(m => m.requestId == id);
            List<RequestHistory> requestHistory = db.RequestHistories.Where(m => m.requestId == id).ToList();
            requestHistory=requestHistory.OrderByDescending(d => d.data).ToList();
            HistoryRequestViewModel model = new HistoryRequestViewModel();

            model.title = request.title;

            if (request.type == false)
                model.type = "Incident";
            else
                model.type = "Request";

            model.description = request.description;

            switch (model.priority)
            {
                case "0": model.priority = "Low";
                    break;
                case "1":
                    model.priority = "Medium";
                    break;
                case "2":
                    model.priority = "High";
                    break;

            }

            foreach(var req in requestHistory)
            {
                model.histories.Add(new HistoryRequestModel
                {
                    from = req.from,
                    to = req.to,
                    status = req.status,
                    data = req.data,
                    message=req.message

                }); 
            }

            return View(model);
        }
    }
}