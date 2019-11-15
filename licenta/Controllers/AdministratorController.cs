using licenta.DatabaseConnection;
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
            string company = db.Users.Where(m => m.username == User.Identity.Name).FirstOrDefault().company;
            List<User> companyUsers = db.Users.Where(m => m.company == company).ToList();
            requestsList = db.Requests.Where(m => db.Users.Any(l => m.createdBy == l.userId&&l.company==company))
                           .ToList();
            // var users=db.Requests.Where(m=>m.createdBy==companyUsers)
            //var requests = db.Requests.Where()
            // return View(await requests.ToListAsync());
            return View(requestsList);
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
    }
}
