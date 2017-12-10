using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImprovementOpportunityApp.Models.Data
{
    [Table("Topic")]
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }

        public ICollection<Suggestion> Suggestions { get; set; }
        public ICollection<Forum> Forums { get; set; }

        public Topic()
        {
            IsActive = true;
            DateAdded = DateTime.Now;
            Forums = new HashSet<Forum>();
            Suggestions = new HashSet<Suggestion>();
        }
    }
}