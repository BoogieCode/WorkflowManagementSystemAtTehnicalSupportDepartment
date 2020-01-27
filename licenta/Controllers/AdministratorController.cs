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
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly TehnicalDepartmentDb db = new TehnicalDepartmentDb();

        // GET: Administrator
        public ActionResult Index()
        {
            var requestsList = db.Requests.ToList();
            string company = Request.Cookies["companyCookie"].Value;
            var companyUsers = db.Users.Where(m => m.company == company).ToList();
            requestsList = db.Requests.Where(m => db.Users.Any(l => m.createdBy == l.userId && l.company == company)).ToList();
            var myIncidentRequestViewModel = new List<myIncidentRequestViewModel>();
            try
            {
                //????
            }
            catch
            {
                return View();
            }

            string statusString = string.Empty;
            foreach (var req in requestsList)
            {

                var requestHistoryList = new List<RequestHistory>();
                requestHistoryList = db.RequestHistories.Where(m => m.requestId == req.requestId).ToList();

                statusString = requestHistoryList.Last()?.status;

                myIncidentRequestViewModel.Add(new myIncidentRequestViewModel
                {
                    Id = req.requestId,
                    IncidentRequest = !req.type ? "Incident" : "Request",
                    Title = req.title,
                    CreatedBy = db.Users.Where(m => m.userId == req.createdBy).FirstOrDefault()?.username,
                    DepartmentAssigned = req.departmentAssigned,
                    Priority = req.priority == 0 ? "Low" : req.priority == 1 ? "Medium" : "High",
                    Status = statusString

                });
            }
            return View(myIncidentRequestViewModel);
        }

        // GET: Administrator/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Administrator/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrator/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administrator/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Administrator/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Administrator/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Administrator/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [ChildActionOnly]
        public ActionResult _RequestIncidentList()
        {

            return PartialView("asd");
        }
    }
}
