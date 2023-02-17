using UniClubDataAPI.Models.Dto;

namespace UniClubDataAPI.Data
{
    public class ClubData
    {
        public static List<ClubDTO> ClubList = new List<ClubDTO>()
        {
            new ClubDTO(1, "UWISU"),
            new ClubDTO(2, "UWPMS")
        };
    }
}
