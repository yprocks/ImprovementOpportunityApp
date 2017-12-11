using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImprovementOpportunityApp.Models
{
    public class EditTopicModel
    {
        [Required]
        public int TopicId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }

    public class AddTopicModel
    {  
        [Required]
        public string Name { get; set; }
        
    }
}