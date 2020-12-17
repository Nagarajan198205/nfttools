using System.ComponentModel.DataAnnotations;

namespace NFTIntegration.Models.Account
{
    public class AddUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The Password field must be a minimum of 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select a user role")]
        public int RoleId { get; set; }
    }
}