using ImprovementOpportunityApp.AppCommons;
using ImprovementOpportunityApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ImprovementOpportunityApp.Controllers
{
    [Authorize(Roles = ApplicationRoles.FIRM_ADMIN)]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

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

        // GET: Admin
        public ActionResult Index()
        {
            var users = db.Users.Include(a => a.Department);
            var model = new AdminViewModel
            {
                Users = new List<Users>(),
                WeeklyIOs = new List<int>(),
                IoDepartmentWise = new Dictionary<string, int>()
            };

            foreach (var user in users)
            {
                var userRole = UserManager.GetRoles(user.Id).FirstOrDefault();
                if (userRole == ApplicationRoles.FIRM_ADMIN) continue;
                var mUser = new Users
                {
                    DateAdded = user.DateAdded,
                    LastUpdated = user.LastUpdated,
                    Department = user.Department.Name,
                    Email = user.Email,
                    FullName = user.FullName,
                    Id = user.Id,
                    IsActive = user.IsActive,
                    UserName = user.UserName,
                    UserRole = userRole
                };
                model.Users.Add(mUser);
            }

            var date = DateTime.Now;
            const int range = 7;
            for (var i = range - 1; i >= 0; i--)
            {
                var thisDateTime = date.AddDays(-i);
                var suggestions = db.Suggestions.Count(c => DbFunctions.TruncateTime(c.DateAdded) == DbFunctions.TruncateTime(thisDateTime));
                model.WeeklyIOs.Add(suggestions);
            }

            var departments = db.Departments.ToList();

            foreach (var department in departments)
            {
                var suggestions = db.Suggestions.Count(c => c.DepartmentId == department.DepartmentId);
                model.IoDepartmentWise.Add(department.Name, suggestions);
            }

            return View(model);
        }

        public ActionResult EditUser(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.Users.Find(id);

            if (user == null)
                return HttpNotFound();

            var model = new EditUserModel
            {
                DepartmentId = user.DepartmentId,
                IsActive = user.IsActive,
                UserName = user.UserName,
                FullName = user.FullName,
                Name = UserManager.GetRoles(user.Id).FirstOrDefault()
            };

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", model.DepartmentId);
            ViewBag.Name = new SelectList(db.Roles.Where(f => f.Name != ApplicationRoles.FIRM_ADMIN), "Name", "Name", model.Name);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.Id);
                user.IsActive = model.IsActive;
                user.DepartmentId = model.DepartmentId;

                string[] roles = {
                    ApplicationRoles.DEPARTMENT_HEAD,
                    ApplicationRoles.EMPLOYEE
                };

                UserManager.RemoveFromRoles(model.Id, roles);
                UserManager.AddToRole(model.Id, model.Name);

                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", model.DepartmentId);
            ViewBag.RoleId = new SelectList(db.Roles.Where(f => f.Name != ApplicationRoles.FIRM_ADMIN), "RoleId", "Name", model.Name);
            return View(model);
        }

        public async Task<ActionResult> UpdateUserStatus(string id)
        {
            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            user.IsActive = !user.IsActive;

            db.Entry(user).State = EntityState.Modified;
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
    }
}