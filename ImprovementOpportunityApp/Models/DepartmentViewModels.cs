using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImprovementOpportunityApp.Models
{
    public class EditDepartmentModel
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }

    public class AddDepartmentModel
    {
        [Required]
        public string Name { get; set; }

    }
}