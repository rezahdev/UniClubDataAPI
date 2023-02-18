using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniClubDataAPI.Validations;

namespace UniClubDataAPI.Models
{
    public class Club
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [UniqueNameValidation("UniversityId")]
        public string Name { get; set; }

        [Required]
        public int UniversityId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public string? LogoUrl { get; set; }

        public string? WebsiteUrl { get; set; }

        public string? InstagramUrl { get; set; }

        public string? FacebookUrl { get; set; }

        public string? TwitterUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
