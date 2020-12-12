using System;
using System.ComponentModel.DataAnnotations;

namespace NFTIntegration.Model
{
    public class DastModel
    {
        [Required]
        public string Url { get; set; }

        public string Description { get; set; }
    }
}
