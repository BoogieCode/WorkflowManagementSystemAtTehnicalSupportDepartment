using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using licenta.DatabaseConnection;
using licenta.Models;

namespace licenta.Controllers
{
    public class IncidentRequestController : Controller
    {
        private TehnicalDepartmentDb db = new TehnicalDepartmentDb();

        // GET: IncidentRequest
        public async Task<ActionResult> Index()
        {
            var requests = db.Requests.Include(r => r.User);
            return View(await requests.ToListAsync());
        }

        // GET: IncidentRequest/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: IncidentRequest/Create
        public ActionResult Create()
        {
            IncidentRequestViewModel model = new IncidentRequestViewModel();
            using (TehnicalDepartmentDb db = new TehnicalDepartmentDb())
            {
                List<Department> departments = db.Departments.ToList();
                foreach (var d in departments)
                {
                    model.departmentsList.Add(new SelectListItem { Text = d.name, Value = d.name.ToString() });

                }

                return View(model);
            }
        }

        // POST: IncidentRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IncidentRequestViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                using (TehnicalDepartmentDb db = new TehnicalDepartmentDb())
                {
                    string CreatedByName = User.Identity.Name;
                    int CreatedById = db.Users.FirstOrDefault(u => u.username == CreatedByName).userId;
                    Request request = new Request
                    {
                        createdBy = CreatedById,
                        title = model.title,
                        description = model.description,
                        type = model.type == "0" ? false : true,
                        departmentAssigned = model.departmentAssigned,
                        employeeAssigned = null,
                        priority = model.priority
                    };
                    db.Requests.Add(request);
                    await db.SaveChangesAsync();
                    List<Request> newRequestList = db.Requests.Where(u => u.createdBy == CreatedById).ToList();
                    List<Request> newRequestListDescendingOrder = newRequestList.OrderByDescending(u => u.requestId).ToList();
                    int newRequestId = newRequestListDescendingOrder.First().requestId;
                    RequestHistory requestHistory = new RequestHistory
                    {
                        requestId = newRequestId,
                        from = CreatedByName,
                        to = model.departmentAssigned,
                        data = DateTime.Now,
                        status = "Pending"
                    };

                    db.RequestHistories.Add(requestHistory);
                    await db.SaveChangesAsync();





                    return RedirectToAction("Support");
                }
            }

            // ViewBag.createdBy = new SelectList(db.Users, "userId", "company", request.createdBy);
            return View(model);
        }

        // GET: IncidentRequest/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.createdBy = new SelectList(db.Users, "userId", "company", request.createdBy);
            return View(request);
        }

        // POST: IncidentRequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "requestId,createdBy,title,description,type,departmentAssigned,employeeAssigned,image,priority")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.createdBy = new SelectList(db.Users, "userId", "company", request.createdBy);
            return View(request);
        }

        // GET: IncidentRequest/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: IncidentRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Request request = await db.Requests.FindAsync(id);
            db.Requests.Remove(request);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Support()
        {

            using (TehnicalDepartmentDb db = new TehnicalDepartmentDb())
            {
                string CreatedByName = User.Identity.Name;
                string Company = Request.Cookies["companyCookie"].Value;
                int id = db.Users.Where(m => m.company == Company && m.username == CreatedByName).FirstOrDefault().userId;
                List<Request> requests = db.Requests.Where(m => m.createdBy == id).ToList();
                List<myIncidentRequestViewModel> myIncidentRequestViewModels = new List<myIncidentRequestViewModel>();

                foreach (var req in requests)
                {

                    List<RequestHistory> s = new List<RequestHistory>();
                    s = db.RequestHistories.Where(m => m.requestId == req.requestId).ToList();
                    string statusString = s.Last().status;

                    myIncidentRequestViewModels.Add(new myIncidentRequestViewModel
                    {
                        Id = req.requestId,
                        IncidentRequest = req.type == false ? "Incident" : "Request",
                        Title = req.title,
                        CreatedBy = db.Users.Where(m => m.userId == req.createdBy).FirstOrDefault().username,
                        DepartmentAssigned = req.departmentAssigned,
                        Priority = req.priority == 0 ? "Low" : req.priority == 1 ? "Medium" : "High",
                        Status = statusString

                    });
                }
                return View(myIncidentRequestViewModels);
            }
        }
    } 
}
