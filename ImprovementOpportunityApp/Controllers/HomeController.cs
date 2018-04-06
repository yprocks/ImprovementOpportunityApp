using ImprovementOpportunityApp.AppCommons;
using ImprovementOpportunityApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ImprovementOpportunityApp.Controllers
{
    
    [Authorize(Roles = ApplicationRoles.EMPLOYEE + ", " + ApplicationRoles.DEPARTMENT_HEAD)]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private string roleName;

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

        [HttpPost]
        public ActionResult SaveValues(FormCollection collection)
        {
            roleName = collection.Get("graph");

            return View();
        }

        public async Task<ActionResult> Index()
        {
            Panel graph = new Panel();
            
            var ab = this.roleName;
            var u = await UserManager.FindByNameAsync(User.Identity.Name);
            var users = db.Users.Include(a => a.Department);
            ViewBag.DepartmentId = u.DepartmentId;
            
            //
            var roleName = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())).FindById(u.Roles.First().RoleId).Name;
            //var roleName = role.FindById(u.Roles.First().RoleId).Name;
            //Control c = FindControl("graph");
            

            

            var forums = db.Forums.Include(f => f.Department).Include(f => f.Suggestion).Include(f => f.Topic).Include(f => f.ForumMessages);

            DeptViewModel model = new DeptViewModel
                {
                Users = new List<Users>(),
                    WeeklyIOs = new List<int>(),
                    IoDepartmentWise = new Dictionary<string, int>(),
                    ForumList = new List<ForumViewModel>()
                };
            ViewBag.role = roleName;
            //if (roleName == "employee")
            //{
                
            //    //c.Visible = false;
            //}
            //else
            //{
            //    ViewBag.role = roleName;
            //    //c.Visible = true;
            //}
            //

            foreach (var forum in forums)
                model.ForumList.Add(new ForumViewModel
                {
                    Department = forum.Department.Name,
                    DepartmentId = forum.DepartmentId,
                    DownVotes = forum.DownVotes,
                    UpVotes = forum.UpVotes,
                    DateAdded = forum.DateAdded,
                    LastUpdated = forum.LastUpdated,
                    ForumId = forum.ForumId,
                    Title = forum.Suggestion.Title,
                    Topic = forum.Topic.Name,
                    IsActive = forum.IsActive,
                    TotalMessages = forum.ForumMessages.Count
                });

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

        



        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

    }
}