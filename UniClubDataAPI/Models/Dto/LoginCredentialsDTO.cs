using System.ComponentModel.DataAnnotations;

namespace UniClubDataAPI.Models.Dto
{
    public class LoginCredentialsDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
