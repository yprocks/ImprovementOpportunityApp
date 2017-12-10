using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImprovementOpportunityApp.Models.Data
{
    [Table("Forum")]
    public class Forum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForumId { get; set; }
        
        [Required]
        public int SuggestionId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int TopicId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime LastUpdated { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }

        [ForeignKey(nameof(TopicId))]
        public Topic Topic { get; set; }

        [ForeignKey(nameof(SuggestionId))]
        public Suggestion Suggestion { get; set; }
        
        public int UpVotes { get; set; }

        public int DownVotes { get; set; }

        public ICollection<ForumMessage> ForumMessages { get; set; }
        public ICollection<UserVote> UserVotes { get; set; }

        public Forum()
        {
            IsActive = true;
            DateAdded = DateTime.Now;
            LastUpdated = DateTime.Now;
            ForumMessages = new HashSet<ForumMessage>();
            UserVotes = new HashSet<UserVote>();
            UpVotes = 0;
            DownVotes = 0;
        }
    }
}