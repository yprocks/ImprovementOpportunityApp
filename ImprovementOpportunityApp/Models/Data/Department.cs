using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImprovementOpportunityApp.Models.Data
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Suggestion> Suggestions { get; set; }
        public ICollection<Forum> Forums { get; set; }

        public Department()
        {
            IsActive = true;
            DateAdded = DateTime.Now;
            Users = new HashSet<ApplicationUser>();
            Suggestions = new HashSet<Suggestion>();
            Forums = new HashSet<Forum>();
        }
    }
}