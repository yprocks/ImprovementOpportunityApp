using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImprovementOpportunityApp.Models.Data
{
    [Table("UserVote")]
    public class UserVote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserVoteId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        public int ForumId { get; set; }

        public bool? HasUpVoted { get; set; }

        public bool HasVoted { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }

        [ForeignKey(nameof(ForumId))]
        public Forum Forum { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public UserVote()
        {
            DateAdded = DateTime.Now;
            HasVoted = false;
            HasUpVoted = null;
        }
    }
}