using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using licenta.DatabaseConnection;
using licenta.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;

namespace licenta.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersManagementController : Controller
    {

        //DE FACUT O CLASA CARE FACE ASTA 
        //CODUL EXISTA SI IN ACCOUNT CONTROLLER

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UsersManagementController()
        {
        }

        public UsersManagementController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }








        private TehnicalDepartmentDb db = new TehnicalDepartmentDb();
        static string CompanyName;
        // GET: UsersManagement
        public ActionResult Index()
        {

            string Company = null;
            Company = db.Users.FirstOrDefault(u => u.username == User.Identity.Name).company.ToString();
            CompanyName = Company;
            var users = db.Users.Include(u => u.Department).Where(u=>u.type!=0 && u.company==Company);
            return View(users.ToList());
        }

        // GET: UsersManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: UsersManagement/Create
        public ActionResult Create()
        {
            CompanyUsersRegisterViewModel model = new CompanyUsersRegisterViewModel();
            using (TehnicalDepartmentDb db = new TehnicalDepartmentDb())
            {

                List<Department> departments = db.Departments.ToList();
                foreach (var d in departments) {
                    model.departmentsList.Add(new SelectListItem { Text = d.name, Value = d.departmentId.ToString() });
                    
                }

                model.userTypes.Add(new SelectListItem { Text = "Employee", Value = 1.ToString() });
                model.userTypes.Add(new SelectListItem { Text = "Customer", Value = 2.ToString() });

                return View(model);
            }
        }

        // POST: UsersManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CompanyUsersRegisterViewModel model)
        {

            
            if (ModelState.IsValid)
            {

                var user1 = new ApplicationUser { UserName = model.user, Email = model.email };
                var result = await UserManager.CreateAsync(user1, model.password);
                if (result.Succeeded)
                {
                    User newUser = new User
                    {
                        company = CompanyName,
                        username = model.user,
                        password = model.password,
                        email = model.email,
                        type = Int32.Parse(model.userTypeId),
                        departmentId = Int32.Parse(model.departmentName)

                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // GET: UsersManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.departmentId = new SelectList(db.Departments, "departmentId", "name", user.departmentId);
            return View(user);
        }

        // POST: UsersManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,company,type,username,password,email,departmentId,role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departmentId = new SelectList(db.Departments, "departmentId", "name", user.departmentId);
            return View(user);
        }

        // GET: UsersManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: UsersManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
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
    }
}
