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
using Microsoft.AspNet.Identity.Owin;

namespace ImprovementOpportunityApp.Controllers
{
    public class ForumController : Controller
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

        // GET: Forum
        public async Task<ActionResult> Index()
        {
            var forums = db.Forums.Include(f => f.Department).Include(f => f.Suggestion).Include(f => f.Topic);
            return View(await forums.ToListAsync());
        }

        // GET: Forum/Details/5
        public async Task<ActionResult> Messages(int? id) 
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var forum = await db.Forums
                    .Include("Department")
                    .Include("Topic")
                    .Include("Suggestion")
                    .Include(x => x.ForumMessages.Select(y => y.User))
                    .FirstOrDefaultAsync(f => f.ForumId == id);

            if (forum == null) return HttpNotFound();

            var forumModel = new ForumViewModel
            {
                Department = forum.Department.Name,
                DownVotes = forum.DownVotes,
                UpVotes = forum.UpVotes,
                DateAdded = forum.DateAdded,
                LastUpdated = forum.LastUpdated,
                ForumId = forum.ForumId,
                Title = forum.Suggestion.Title,
                Topic = forum.Topic.Name,
                IsActive = forum.IsActive,
                Images = forum.Suggestion.Images,
                Links = forum.Suggestion.Links,
                CurrentUserName = User.Identity.Name,
                Messages = new List<MessageViewModel>()
            };

            foreach (var forumMessage in forum.ForumMessages)
            {
                forumModel.Messages.Add(new MessageViewModel
                {
                    ForumMessageId = forumMessage.ForumMessageId,
                    Message = forumMessage.Message,
                    DateAdded = forumMessage.DateAdded,
                    ReplyTo = forumMessage.ReplyMessageId,
                    LastUpdated = forumMessage.LastUpdated,
                    Links = forumMessage.Links,
                    Images = forumMessage.Images,
                    UserName = forumMessage.User.UserName,
                    UserId = forumMessage.UserId
                });
            }

            return View(forumModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(AddCommentModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);

                var message = new ForumMessage
                {
                    ForumId = model.ForumId,
                    ReplyMessageId = null,
                    Message = model.Comment,
                    UserId = user.Id
                };

                db.ForumMessages.Add(message);
                await db.SaveChangesAsync();
                return RedirectToAction("Messages", "Forum", new { id = model.ForumId });
            }

            //ViewBag.ForumId = new SelectList(db.Forums, "ForumId", "ForumId", forumMessage.ForumId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", forumMessage.UserId);
            ModelState.AddModelError("Error ", "Please enter a valid comment");
            return RedirectToAction("Messages", "Forum", new { id = model.ForumId });
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

        //// GET: Forum/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Forum forum = await db.Forums.FindAsync(id);
        //    if (forum == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", forum.DepartmentId);
        //    ViewBag.SuggestionId = new SelectList(db.Suggestions, "SuggestionId", "UserId", forum.SuggestionId);
        //    ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", forum.TopicId);
        //    return View(forum);
        //}

        //// POST: Forum/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ForumId,SuggestionId,DepartmentId,TopicId,DateAdded,LastUpdated,IsActive,UpVotes,DownVotes")] Forum forum)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(forum).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", forum.DepartmentId);
        //    ViewBag.SuggestionId = new SelectList(db.Suggestions, "SuggestionId", "UserId", forum.SuggestionId);
        //    ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", forum.TopicId);
        //    return View(forum);
        //}

        // GET: ForumMessages/Edit/5

        public async Task<ActionResult> EditMessage(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ForumMessage forumMessage = await db.ForumMessages.FindAsync(id);

            if (forumMessage == null)
                return HttpNotFound();

            ViewBag.ForumId = new SelectList(db.Forums, "ForumId", "ForumId", forumMessage.ForumId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", forumMessage.UserId);
            return View(forumMessage);
        }

        // POST: ForumMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMessage([Bind(Include = "ForumMessageId,UserId,ForumId,ReplyMessageId,Message,DateAdded,LastUpdated,Links,Images")] ForumMessage forumMessage)
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


        // GET: Forum/Delete/5
        public async Task<ActionResult> DeleteMessage(int? id)
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

        // POST: Forum/DeleteMessage/5
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

        //// GET: Forum/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Forum forum = await db.Forums.FindAsync(id);
        //    if (forum == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(forum);
        //}

        //// POST: Forum/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Forum forum = await db.Forums.FindAsync(id);
        //    db.Forums.Remove(forum);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}



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
