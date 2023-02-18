using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UniClubDataAPI.Validations;

namespace UniClubDataAPI.Models.Dto
{
    public class ClubDTO
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

        public ClubDTO() { }

        public ClubDTO(Club club)
        {
            Id = club.Id;
            Name = club.Name;
            Email = club.Email;
            UniversityId = club.UniversityId;
            Description = club.Description;
            LogoUrl = club.LogoUrl;
            TwitterUrl = club.TwitterUrl;
            FacebookUrl = club.FacebookUrl;
            InstagramUrl = club.InstagramUrl;
            WebsiteUrl = club.WebsiteUrl;
        }

        public async Task<Club> GetClubFromDTOAsync()
        {
            Club club = new Club()
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                UniversityId = this.UniversityId,
                Description = this.Description,
                LogoUrl = this.LogoUrl,
                TwitterUrl = this.TwitterUrl,
                FacebookUrl = this.FacebookUrl,
                InstagramUrl = this.InstagramUrl,
                WebsiteUrl = this.WebsiteUrl
            };
            return club;
        }

    }
}
