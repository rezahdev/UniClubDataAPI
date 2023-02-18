using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UniClubDataAPI.Models.Dto
{
    public class UserDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        [DefaultValue(3)]
        public int AccessLevel { get; set; }

        public UserDTO() {  }

        public UserDTO(User user)
        {
            Name = user.Name;
            Email = user.Email;
            Password = user.Password;
            AccessLevel = user.AccessLevel;
        }
    }
}
