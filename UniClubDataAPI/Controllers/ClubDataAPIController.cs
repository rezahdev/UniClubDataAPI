using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UniClubDataAPI.Data;
using UniClubDataAPI.Models;

namespace UniClubDataAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/ClubDataAPI")]
    [ApiController]
    public class ClubDataAPIController: ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDBContext _db;

        public ClubDataAPIController(ILogger<ClubDataAPIController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Club>> GetClubs()
        {
            _logger.LogInformation("Updating");
            return Ok(_db.Clubs.ToList());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Club> GetClub(int id)
        {
            if(id == 0)
            {
                return BadRequest("Invalid Id");
            }
            Club club = _db.Clubs.Find(id);
            return club == null ? NotFound() : Ok(club);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Club> CreateClub([FromBody]Club club)
        {
            if(club == null)
            {
                return BadRequest();
            }

            if(_db.Clubs.FirstOrDefault(c => c.Name.ToLower() == club.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Unique", "name is not unique.");
                return BadRequest(ModelState);
            }

            _db.Clubs.Add(club);
            _db.SaveChanges();

            return Ok(club);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteClub(int id)
        {
            Club club = _db.Clubs.Find(id);

            if(club == null)
            {
                return BadRequest();
            }
            else
            {
                _db.Clubs.Remove(club);
                _db.SaveChanges();
                return NoContent();
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Club> UpdateClub(int id, [FromBody]Club newClubData)
        {
            if(newClubData == null || newClubData.Id != id)
            {
                return BadRequest();
            }

            Club club = _db.Clubs.Find(id);

            if(club == null)
            {
                return NotFound();
            }

            _db.Clubs.Update(club);
            _db.SaveChanges();
            return Ok(club);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Club> UpdatePartialClub(int id, JsonPatchDocument<Club> newClubData)
        {
            if (newClubData == null)
            {
                return BadRequest();
            }

            Club club = _db.Clubs.Find(id);

            if (club == null)
            {
                return NotFound();
            }

            newClubData.ApplyTo(club, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            _db.Clubs.Update(club);
            _db.SaveChanges();

            return Ok(club);
        }

    }
}
