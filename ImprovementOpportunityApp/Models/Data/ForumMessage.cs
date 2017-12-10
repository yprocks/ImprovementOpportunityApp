using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImprovementOpportunityApp.Models.Data
{
    [Table("ForumMessage")]
    public class ForumMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForumMessageId { get; set; }
        
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        public int ForumId { get; set; }

        public int? ReplyMessageId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdated { get; set; }

        public string Links { get; set; }

        public string Images { get; set; }

        [ForeignKey(nameof(ForumId))]
        public Forum Forum { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public ForumMessage()
        {
            DateAdded = DateTime.Now;
            LastUpdated = DateTime.Now;
        }
    }
}