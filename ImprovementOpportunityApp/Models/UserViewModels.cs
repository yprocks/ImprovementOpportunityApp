using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImprovementOpportunityApp.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Department { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public string Email { get; set; }
        [Display(Name = "User Role")]
        public string UserRole { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }

    public class EditUserModel
    {
        public string Id { get; set; }

        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
        
    }

}