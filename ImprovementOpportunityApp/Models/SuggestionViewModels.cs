using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImprovementOpportunityApp.Models
{
    public class AddSuggestionModel
    {
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int TopicId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class EditSuggestionModel
    {
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int TopicId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int SuggestionId { get;  set; }
    }
}