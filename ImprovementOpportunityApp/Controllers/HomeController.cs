using ImprovementOpportunityApp.AppCommons;
using ImprovementOpportunityApp.Models;
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

        public async Task<ActionResult> Index()
        {
            var forums = await db.Forums.Include(f => f.Department).Include(f => f.Suggestion).Include(f => f.Topic).ToListAsync();
            var forumList = new List<ForumViewModel>();
            foreach (var forum in forums)
            {
                forumList.Add(new ForumViewModel
                {
                    Department = forum.Department.Name,
                    DownVotes = forum.DownVotes,
                    UpVotes = forum.UpVotes,
                    DateAdded = forum.DateAdded,
                    LastUpdated = forum.LastUpdated,
                    ForumId = forum.ForumId,
                    Title = forum.Suggestion.Title,
                    Topic = forum.Topic.Name,
                    IsActive = forum.IsActive
                });
            }
            return View(forumList);
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
    }
}