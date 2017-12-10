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
    public class SuggestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Suggestions
        public async Task<ActionResult> Index()
        {
            var suggestions = db.Suggestions.Include(s => s.Department).Include(s => s.Topic).Include(s => s.User);
            return View(await suggestions.ToListAsync());
        }

        // GET: Suggestions/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: Suggestions/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Suggestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SuggestionId,UserId,DepartmentId,TopicId,Title,Description,Images,Links,DateAdded,LastUpdated,HasReviewed,HasConsidered")] Suggestion suggestion)
        {
            if (ModelState.IsValid)
            {
                db.Suggestions.Add(suggestion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", suggestion.DepartmentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", suggestion.TopicId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", suggestion.UserId);
            return View(suggestion);
        }

        // GET: Suggestions/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", suggestion.DepartmentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", suggestion.TopicId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", suggestion.UserId);
            return View(suggestion);
        }

        // POST: Suggestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SuggestionId,UserId,DepartmentId,TopicId,Title,Description,Images,Links,DateAdded,LastUpdated,HasReviewed,HasConsidered")] Suggestion suggestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suggestion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", suggestion.DepartmentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", suggestion.TopicId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", suggestion.UserId);
            return View(suggestion);
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
