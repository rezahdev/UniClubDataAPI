using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniClubDataAPI.Data;
using UniClubDataAPI.Models;
using UniClubDataAPI.Models.Dto;

namespace UniClubDataAPI.Controllers
{
    [Route("api/clubs")]
    [ApiController]
    public class ClubDataAPIController: ControllerBase
    {
        private readonly ApplicationDBContext _db;

        private struct _errorMessage
        {
            public const string NotFound = "Club not found (club Id may be invalid)";
            public const string BadRequest = "Invalid request (request parameters may be invalid)";
            public const string NoContent = "Request successful";
        }

        public ClubDataAPIController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClubDTO>>> GetClubsAsync()
        {
            List<ClubDTO> clubList = await _db.Clubs.Select(c => new ClubDTO(c)).ToListAsync();
            return Ok(clubList);
        }

        [HttpGet("{id:int}", Name = "GetClub")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClubDTO>> GetClubAsync(int id)
        {
            if(id == 0)
            {
                return BadRequest(_errorMessage.BadRequest);
            }

            Club club = await _db.Clubs.FirstOrDefaultAsync(c => c.Id == id);

            if(club == null)
            {
                return NotFound(_errorMessage.NotFound);
            }
            return Ok(new ClubDTO(club));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClubAsync([FromBody]ClubDTO clubDTO)
        {
            if(clubDTO == null)
            {
                return BadRequest(_errorMessage.BadRequest);
            }

            Club club = await clubDTO.GetClubFromDTOAsync();
            club.CreatedDate = DateTime.Now;

            await _db.Clubs.AddAsync(club);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetClub", new { id = club.Id }, clubDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClubAsync(int id)
        {
            Club club = await _db.Clubs.FindAsync(id);

            if(club == null)
            {
                return NotFound(_errorMessage.NotFound);
            }

            _db.Clubs.Remove(club);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClubDTO>> UpdateClubAsync(int id, [FromBody]ClubDTO clubDTO)
        {
            if(clubDTO == null)
            {
                return BadRequest(_errorMessage.BadRequest);
            }

            bool clubExists = await _db.Clubs.AnyAsync(c => c.Id == id);
            if (!clubExists)
            {
                return NotFound(_errorMessage.NotFound);
            }

            clubDTO.Id = id;
            Club club = await clubDTO.GetClubFromDTOAsync();

            _db.Clubs.Update(club);
            await _db.SaveChangesAsync();

            return Ok(clubDTO);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClubDTO>> UpdatePartialClub(int id, JsonPatchDocument<ClubDTO> clubDTOP)
        {
            if (clubDTOP == null)
            {
                return BadRequest(_errorMessage.BadRequest);
            }

            Club currentClub = await _db.Clubs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (currentClub == null)
            {
                return NotFound(_errorMessage.NotFound);
            }

            ClubDTO currentClubDTO = new ClubDTO(currentClub);
            clubDTOP.ApplyTo(currentClubDTO, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(_errorMessage.BadRequest);
            }

            Club club = await currentClubDTO.GetClubFromDTOAsync();

            _db.Clubs.Update(club);
            await _db.SaveChangesAsync();

            return Ok(currentClubDTO);
        }
    }
}
