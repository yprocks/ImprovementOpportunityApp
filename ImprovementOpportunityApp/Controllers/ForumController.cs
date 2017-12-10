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

namespace ImprovementOpportunityApp.Controllers
{
    public class ForumController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Forum
        public async Task<ActionResult> Index()
        {
            var forums = db.Forums.Include(f => f.Department).Include(f => f.Suggestion).Include(f => f.Topic);
            return View(await forums.ToListAsync());
        }

        // GET: Forum/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum forum = await db.Forums.FindAsync(id);
            if (forum == null)
            {
                return HttpNotFound();
            }
            return View(forum);
        }

        // GET: Forum/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "SuggestionId", "UserId");
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name");
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ForumId,SuggestionId,DepartmentId,TopicId,DateAdded,LastUpdated,IsActive,UpVotes,DownVotes")] Forum forum)
        {
            if (ModelState.IsValid)
            {
                db.Forums.Add(forum);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", forum.DepartmentId);
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "SuggestionId", "UserId", forum.SuggestionId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", forum.TopicId);
            return View(forum);
        }

        // GET: Forum/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum forum = await db.Forums.FindAsync(id);
            if (forum == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", forum.DepartmentId);
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "SuggestionId", "UserId", forum.SuggestionId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", forum.TopicId);
            return View(forum);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ForumId,SuggestionId,DepartmentId,TopicId,DateAdded,LastUpdated,IsActive,UpVotes,DownVotes")] Forum forum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forum).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", forum.DepartmentId);
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "SuggestionId", "UserId", forum.SuggestionId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", forum.TopicId);
            return View(forum);
        }

        // GET: Forum/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum forum = await db.Forums.FindAsync(id);
            if (forum == null)
            {
                return HttpNotFound();
            }
            return View(forum);
        }

        // POST: Forum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Forum forum = await db.Forums.FindAsync(id);
            db.Forums.Remove(forum);
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
