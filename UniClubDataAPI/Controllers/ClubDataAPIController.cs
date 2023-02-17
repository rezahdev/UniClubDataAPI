using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UniClubDataAPI.Data;
using UniClubDataAPI.Models;
using UniClubDataAPI.Models.Dto;

namespace UniClubDataAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/ClubDataAPI")]
    [ApiController]
    public class ClubDataAPIController: ControllerBase
    {
        private readonly ILogger _logger;

        public ClubDataAPIController(ILogger<ClubDataAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ClubDTO>> GetClubs()
        {
            _logger.LogInformation("Updating");
            return Ok(ClubData.ClubList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClubDTO> GetClub(int id)
        {
            if(id == 0)
            {
                return BadRequest("Invalid Id");
            }
            ClubDTO club = ClubData.ClubList.Find(c => c.Id == id);
            return club == null ? NotFound() : Ok(club);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClubDTO> CreateClub([FromBody]ClubDTO club)
        {
            if(club == null)
            {
                return BadRequest();
            }

            if(ClubData.ClubList.FirstOrDefault(c => c.Name.ToLower() == club.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Unique", "name is not unique.");
                return BadRequest(ModelState);
            }

            club.Id = ClubData.ClubList.OrderByDescending(c => c.Id).First().Id + 1;
            ClubData.ClubList.Add(club);

            return Ok(club);
        }

        [HttpDelete("{id:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteClub(int id)
        {
            ClubDTO club = ClubData.ClubList.Find(c => c.Id == id);

            if(club == null)
            {
                return BadRequest();
            }
            else
            {
                ClubData.ClubList.Remove(club);
                return NoContent();
            }
        }

        [HttpPut("{id:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClubDTO> UpdateClub(int id, [FromBody]ClubDTO newClubData)
        {
            if(newClubData == null || newClubData.Id != id)
            {
                return BadRequest();
            }

            ClubDTO club = ClubData.ClubList.Find(c => c.Id == id);

            if(club == null)
            {
                return NotFound();
            }

            club.Name = newClubData.Name;
            return Ok(club);
        }

        [HttpPatch("{id:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClubDTO> UpdatePartialClub(int id, JsonPatchDocument<ClubDTO> newClubData)
        {
            if (newClubData == null)
            {
                return BadRequest();
            }

            ClubDTO club = ClubData.ClubList.Find(c => c.Id == id);

            if (club == null)
            {
                return NotFound();
            }

            newClubData.ApplyTo(club, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(club);
        }
    }
}
