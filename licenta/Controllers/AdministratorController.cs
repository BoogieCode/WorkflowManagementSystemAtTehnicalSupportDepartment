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
        private TehnicalDepartmentDb db = new TehnicalDepartmentDb();

        // GET: Administrator
        public ActionResult Index()
        {
            List<Request> requestsList = db.Requests.ToList();
            string company = Request.Cookies["companyCookie"].Value;
            List<User> companyUsers = db.Users.Where(m => m.company == company).ToList();
            requestsList = db.Requests.Where(m => db.Users.Any(l => m.createdBy == l.userId&&l.company==company)).ToList();
            List<myIncidentRequestViewModel> myIncidentRequestViewModel = new List<myIncidentRequestViewModel>();
            try
            {
            }
            catch
            {
                return View();
            }
            string statusString;
            foreach (var req in requestsList)
            {
                List<RequestHistory> s = new List<RequestHistory>();
                s = db.RequestHistories.Where(m => m.requestId == req.requestId).ToList();
                //var lasts = s.Last().status;
                statusString = s.Last().status;
                //switch (s.Last().status)
                //{
                //    case 1:
                //        statusString=
                //};
                myIncidentRequestViewModel.Add(new myIncidentRequestViewModel
                {
                    Id=req.requestId,
                    IncidentRequest = req.type == false ? "Incident" : "Request",
                    Title=req.title,
                    CreatedBy=db.Users.Where(m=>m.userId==req.createdBy).FirstOrDefault().username,
                    DepartmentAssigned=req.departmentAssigned,
                    Priority=req.priority==0?"Low":req.priority==1?"Medium":"High",
                    Status=statusString

            }) ;
            }
            // var users=db.Requests.Where(m=>m.createdBy==companyUsers)
            //var requests = db.Requests.Where()
            // return View(await requests.ToListAsync());
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
