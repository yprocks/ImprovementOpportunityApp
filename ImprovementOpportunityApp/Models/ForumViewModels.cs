using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImprovementOpportunityApp.Models
{
    public class ForumViewModel
    {
        public int ForumId { get; set; }

        public string Title { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }

        public string Department { get; set; }

        public string Topic { get; set; }

        [Display(Name = "Up Votes")]
        public int UpVotes { get; set; }

        [Display(Name = "Down Votes")]
        public int DownVotes { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }
        public bool? HasUpVoted { get; set; } = null;
        public bool HasVoted { get; set; }

        // Fields used for message view 
        public string Links { get; set; }

        public string Images { get; set; }

        public string CurrentUserName { get; set; }

        public IList<MessageViewModel> Messages { get; set; }
      
    }

    public class MessageViewModel
    {
        public int ForumMessageId { get; set; }

        public string Message { get; set; }

        public string UserName { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Links { get; set; }

        public string Images { get; set; }

        public int? ReplyTo { get; set; }

        public string UserId { get; set; }

    }

    public class AddCommentModel
    {
        [Required]
        public int ForumId { get; set; }
        [Required]
        public string Comment { get; set; }
    }

    public class EditCommentModel
    {
        [Required]
        public int ForumId { get; set; }

        [Required]
        public int ForumMessageId { get; set; }

        [Required]
        [StringLength(512)]
        public string Message { get; set; }

        //public string Links { get; set; }
        //public string Images { get; set; }
    }
}