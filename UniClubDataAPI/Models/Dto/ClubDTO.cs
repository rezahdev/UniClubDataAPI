using System.ComponentModel.DataAnnotations;

namespace UniClubDataAPI.Models.Dto
{
    public class ClubDTO: Club
    {
        public ClubDTO(int id, string name)
        {
            base.Id = id;
            base.Name = name;
        }

        public int Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        [Required]
        [MaxLength(255)]
        public string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}
