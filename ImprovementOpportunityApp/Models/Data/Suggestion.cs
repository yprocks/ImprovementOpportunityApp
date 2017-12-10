using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImprovementOpportunityApp.Models.Data
{
    [Table("Suggestion")]
    public class Suggestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SuggestionId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int TopicId { get; set; }

        [Required]
        [StringLength(512)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Images { get; set; }

        public string Links { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime LastUpdated { get; set; }

        [Required]
        public bool HasReviewed { get; set; }

        [Required]
        public bool HasConsidered { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }

        [ForeignKey(nameof(TopicId))]
        public Topic Topic { get; set; }
        
        public ICollection<Forum> Forums { get; set; }

        public Suggestion()
        {
            DateAdded = DateTime.Now;
            LastUpdated = DateTime.Now;
            HasConsidered = false;
            HasReviewed = false;
            Forums = new HashSet<Forum>();
        }
    }
}