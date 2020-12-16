using System;
using System.ComponentModel.DataAnnotations;

namespace NFTIntegration.Models
{
    public class CreateProjectModel
    {
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Please enter project name")]
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
