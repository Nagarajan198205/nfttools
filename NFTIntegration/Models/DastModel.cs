using NFTIntegration.Classes;
using System.ComponentModel.DataAnnotations;
using System.Management.Automation;

namespace NFTIntegration.Model
{
    public class DastModel
    {
        [Required]
        public string Url { get; set; }

        public string Description { get; set; }

        public int ProjectId { get; set; }
        protected Projects ProjectList { get; set; }
    }
}
