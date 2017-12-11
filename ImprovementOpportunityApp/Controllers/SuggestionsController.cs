using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImprovementOpportunityApp.Models;
using ImprovementOpportunityApp.Models.Data;
using ImprovementOpportunityApp.AppCommons;
using Microsoft.AspNet.Identity.Owin;

namespace ImprovementOpportunityApp.Controllers
{
    [Authorize(Roles = ApplicationRoles.EMPLOYEE + ", " + ApplicationRoles.DEPARTMENT_HEAD)]
    public class SuggestionsController : Controller
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

        // GET: Suggestions
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole(ApplicationRoles.DEPARTMENT_HEAD))
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);
                var suggestions = db.Suggestions.Include(s => s.Department).Include(s => s.Topic).Include(s => s.User).Where(s => s.DepartmentId == user.DepartmentId);
                return View(await suggestions.ToListAsync());
            }
            else
            {
                var suggestions = db.Suggestions.Include(s => s.Department).Include(s => s.Topic).Include(s => s.User).Where(s => s.User.UserName == User.Identity.Name);
                return View(await suggestions.ToListAsync());
            }
        }

        // GET: Suggestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Suggestion suggestion = await db.Suggestions.Include(s => s.Department).Include(s => s.Topic).Include(s => s.User).FirstOrDefaultAsync(s => s.SuggestionId == id);

            if (suggestion == null)
                return HttpNotFound();

            if (!suggestion.HasReviewed)
            {
                if (User.IsInRole(ApplicationRoles.DEPARTMENT_HEAD))
                {
                    var user = await UserManager.FindByNameAsync(User.Identity.Name);
                    if (user.DepartmentId == suggestion.DepartmentId)
                    {
                        suggestion.HasReviewed = true;
                        db.Entry(suggestion).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
            }

            return View(suggestion);
        }

        // GET: Suggestions/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(d => d.IsActive), "DepartmentId", "Name");
            ViewBag.TopicId = new SelectList(db.Topics.Where(d => d.IsActive), "TopicId", "Name");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddSuggestionModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);

                var suggestion = new Suggestion
                {
                    DepartmentId = model.DepartmentId,
                    TopicId = model.TopicId,
                    Title = model.Title,
                    Description = model.Description,
                    UserId = user.Id
                };

                db.Suggestions.Add(suggestion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", model.DepartmentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", model.TopicId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", suggestion.UserId);
            return View(model);
        }

        // GET: Suggestions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Suggestion suggestion = await db.Suggestions.FindAsync(id);
            if (suggestion == null)
                return HttpNotFound();

            var model = new EditSuggestionModel
            {
                Description = suggestion.Description,
                Title = suggestion.Title,
                DepartmentId = suggestion.DepartmentId,
                TopicId = suggestion.TopicId,
                SuggestionId = suggestion.SuggestionId
            };

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", suggestion.DepartmentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", suggestion.TopicId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", suggestion.UserId);
            return View(suggestion);
        }

        // POST: Suggestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditSuggestionModel model)
        {
            if (ModelState.IsValid)
            {
                var suggestion = await db.Suggestions.FindAsync(model.SuggestionId);
                if (suggestion == null)
                    return HttpNotFound();
                suggestion.Title = model.Title;
                suggestion.Description = model.Description;
                suggestion.DepartmentId = model.DepartmentId;
                suggestion.TopicId = model.TopicId;
                suggestion.HasReviewed = false;
                suggestion.LastUpdated = DateTime.Now;

                db.Entry(suggestion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", model.DepartmentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", model.TopicId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", suggestion.UserId);
            return View(model);
        }

        // GET: Suggestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion suggestion = await db.Suggestions.FindAsync(id);
            if (suggestion == null)
            {
                return HttpNotFound();
            }
            return View(suggestion);
        }

        // POST: Suggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Suggestion suggestion = await db.Suggestions.FindAsync(id);
            db.Suggestions.Remove(suggestion);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Approve(int id)
        {
            Suggestion suggestion = await db.Suggestions.Include(s => s.Department).Include(s => s.Topic).Include(s => s.User).FirstOrDefaultAsync(s => s.SuggestionId == id);

            if (suggestion == null)
                return HttpNotFound();

            suggestion.HasReviewed = true;
            suggestion.HasConsidered = true;

            db.Entry(suggestion).State = EntityState.Modified;

            var forum = new Forum
            {
                SuggestionId = suggestion.SuggestionId,
                DepartmentId = suggestion.DepartmentId,
                TopicId = suggestion.TopicId
            };
            db.Forums.Add(forum);

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
