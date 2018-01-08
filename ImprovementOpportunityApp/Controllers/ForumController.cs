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
using ImprovementOpportunityApp.AppCommons;

namespace ImprovementOpportunityApp.Controllers
{
    [Authorize(Roles = ApplicationRoles.EMPLOYEE + ", " + ApplicationRoles.DEPARTMENT_HEAD)]
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
        public ActionResult Index()
        {
            //var forums = db.Forums.Include(f => f.Department).Include(f => f.Suggestion).Include(f => f.Topic);
            //return View(await forums.ToListAsync());
            return RedirectToAction("Index", "Home");
        }

        // GET: Forum/Details/5
        public async Task<ActionResult> Messages(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            UserVote userVote = await db.UserVotes.FirstOrDefaultAsync(v => v.ForumId == id && v.UserId == user.Id);

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

            if (userVote != null)
            {
                forumModel.HasVoted = userVote.HasVoted;
                forumModel.HasUpVoted = userVote.HasUpVoted;
            }

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

        //// GET: Forum/Create
        //public ActionResult Create()
        //{
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
        //    ViewBag.SuggestionId = new SelectList(db.Suggestions, "SuggestionId", "UserId");
        //    ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name");
        //    return View();
        //}

        //// POST: Forum/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ForumId,SuggestionId,DepartmentId,TopicId,DateAdded,LastUpdated,IsActive,UpVotes,DownVotes")] Forum forum)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Forums.Add(forum);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", forum.DepartmentId);
        //    ViewBag.SuggestionId = new SelectList(db.Suggestions, "SuggestionId", "UserId", forum.SuggestionId);
        //    ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Name", forum.TopicId);
        //    return View(forum);
        //}

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

            var editComment = new EditCommentModel
            {
                ForumId = forumMessage.ForumId,
                ForumMessageId = forumMessage.ForumMessageId,
                Message = forumMessage.Message
            };

            //ViewBag.ForumId = new SelectList(db.Forums, "ForumId", "ForumId", forumMessage.ForumId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", forumMessage.UserId);
            return View(editComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMessage(EditCommentModel model)
        {
            if (ModelState.IsValid)
            {
                var forumMessage = await db.ForumMessages.FindAsync(model.ForumMessageId);

                forumMessage.Message = model.Message;
                forumMessage.LastUpdated = DateTime.Now;

                db.Entry(forumMessage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Messages", new { id = model.ForumId });
            }
            //ViewBag.ForumId = new SelectList(db.Forums, "ForumId", "ForumId", model.ForumId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", forumMessage.UserId);
            return View(model);
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

        public async Task<ActionResult> Update(int id)
        {
            Forum forum = await db.Forums.FindAsync(id);
            if (forum == null)
                return HttpNotFound();

            forum.IsActive = !forum.IsActive;
            forum.LastUpdated = DateTime.Now;

            db.Entry(forum).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Vote(int id, bool upVote)
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);

            UserVote userVote = await db.UserVotes.FirstOrDefaultAsync(v => v.ForumId == id && v.UserId == user.Id);

            if (userVote == null)
            {
                userVote = new UserVote
                {
                    HasUpVoted = upVote ? true : false,
                    HasVoted = true,
                    UserId = user.Id,
                    ForumId = id
                };
                db.UserVotes.Add(userVote);
                await db.SaveChangesAsync();
            }
            else
            {
                userVote.HasUpVoted = upVote ? true : false;
                db.Entry(userVote).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            int upVotes = db.UserVotes.Count(v => v.ForumId == id && v.HasVoted && v.HasUpVoted != null && (bool)v.HasUpVoted);
            int downVotes = db.UserVotes.Count(v => v.ForumId == id && v.HasVoted && v.HasUpVoted != null && (bool)!v.HasUpVoted);

            var forum = await db.Forums.FindAsync(id);
            forum.UpVotes = upVotes;
            forum.DownVotes = downVotes;

            db.Entry(forum).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Messages", new { id = id });
        }

        public async Task<ActionResult> Search(string query)
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            ViewBag.DepartmentId = user.DepartmentId;

            ViewBag.Query = query;

            var forums = db.Forums
                .Include(f => f.Department)
                .Include(f => f.Suggestion)
                .Include(f => f.Topic)
                .Include(f => f.ForumMessages)
                .Where(f => f.Suggestion.Title.ToLower().Contains(query.ToLower())
                    || f.Department.Name.ToLower().Contains(query.ToLower())
                    || f.Topic.Name.ToLower().Contains(query.ToLower())
                    || f.DateAdded.Month.ToString() == query
                    || f.DateAdded.Day.ToString() == query
                    || f.DateAdded.Year.ToString() == query);

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

        public async Task<ActionResult> Reply(ReplyModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);

                var message = new ForumMessage
                {
                    ForumId = model.Id,
                    ReplyMessageId = model.MessageId,
                    Message = model.Reply,
                    UserId = user.Id
                };

                db.ForumMessages.Add(message);
                await db.SaveChangesAsync();
                return RedirectToAction("Messages", new { id = model.Id });
            }
            ModelState.AddModelError("Error ", "Please enter a valid comment");
            return RedirectToAction("Messages", "Forum", new { id = model.Id });
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
