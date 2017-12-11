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
    public class ForumMessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ForumMessages
        public async Task<ActionResult> Index()
        {
            var forumMessages = db.ForumMessages.Include(f => f.Forum).Include(f => f.User);
            return View(await forumMessages.ToListAsync());
        }

        // GET: ForumMessages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumMessage forumMessage = await db.ForumMessages.FindAsync(id);
            if (forumMessage == null)
            {
                return HttpNotFound();
            }
            return View(forumMessage);
        }

        // GET: ForumMessages/Create
        public ActionResult Create()
        {
            ViewBag.ForumId = new SelectList(db.Forums, "ForumId", "ForumId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: ForumMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ForumMessageId,UserId,ForumId,ReplyMessageId,Message,DateAdded,LastUpdated,Links,Images")] ForumMessage forumMessage)
        {
            if (ModelState.IsValid)
            {
                db.ForumMessages.Add(forumMessage);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ForumId = new SelectList(db.Forums, "ForumId", "ForumId", forumMessage.ForumId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", forumMessage.UserId);
            return View(forumMessage);
        }

        // GET: ForumMessages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumMessage forumMessage = await db.ForumMessages.FindAsync(id);
            if (forumMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ForumId = new SelectList(db.Forums, "ForumId", "ForumId", forumMessage.ForumId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", forumMessage.UserId);
            return View(forumMessage);
        }

        // POST: ForumMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ForumMessageId,ForumId,Message")] ForumMessage forumMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forumMessage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ForumId = new SelectList(db.Forums, "ForumId", "ForumId", forumMessage.ForumId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", forumMessage.UserId);
            return View(forumMessage);
        }

        // GET: ForumMessages/Delete/5
        public async Task<ActionResult> DeleteMessage(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            ForumMessage forumMessage = await db.ForumMessages.FindAsync(id);
            if (forumMessage == null)
                return HttpNotFound();
            
            return View(forumMessage);
        }

        // POST: ForumMessages/Delete/5
        [HttpPost, ActionName("DeleteMessage")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ForumMessage forumMessage = await db.ForumMessages.FindAsync(id);
            if (forumMessage == null)
                return HttpNotFound();
            int forumId = forumMessage.ForumId;
            db.ForumMessages.Remove(forumMessage);
            await db.SaveChangesAsync();
            return RedirectToAction("Messages", new { id = forumId });
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
