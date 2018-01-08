using ImprovementOpportunityApp.AppCommons;
using ImprovementOpportunityApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ImprovementOpportunityApp.Controllers
{
    [Authorize(Roles = ApplicationRoles.EMPLOYEE + ", " + ApplicationRoles.DEPARTMENT_HEAD)]
    public class HomeController : Controller
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

        public async Task<ActionResult> Index()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            ViewBag.DepartmentId = user.DepartmentId;

            var forums = db.Forums.Include(f => f.Department).Include(f => f.Suggestion).Include(f => f.Topic).Include(f => f.ForumMessages);
            var forumList = new List<ForumViewModel>();

            foreach (var forum in forums)
                forumList.Add(new ForumViewModel
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
            
            return View(forumList);
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