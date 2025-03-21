using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenueController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public VenueController(ArtBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CREATE
        [HttpPost("create")]
        public ActionResult<Venue> CreateVenue([FromBody] Venue venue)
        {
            if (venue == null)
            {
                return BadRequest("Invalid venue data.");
            }

            _dbContext.Venues.Add(venue);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetVenueById), new { id = venue.VenueId }, venue);
        }

        // GET ALL
        [HttpGet("all")]
        public ActionResult<IEnumerable<Venue>> GetAllVenues()
        {
            var venues = _dbContext.Venues
                .Include(v => v.ScheduleItems)
                .Include(v => v.PriceLists)
                .Include(v => v.Areas)
                .ToList();

            return Ok(venues);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<Venue> GetVenueById(int id)
        {
            var venue = _dbContext.Venues
                .Include(v => v.ScheduleItems)
                .Include(v => v.PriceLists)
                .Include(v => v.Areas)
                .FirstOrDefault(v => v.VenueId == id);

            if (venue == null)
            {
                return NotFound($"Venue with ID {id} not found.");
            }

            return Ok(venue);
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult UpdateVenue(int id, [FromBody] Venue updatedVenue)
        {
            if (updatedVenue == null || updatedVenue.VenueId != id)
            {
                return BadRequest("Invalid venue data or ID mismatch.");
            }

            var existingVenue = _dbContext.Venues
                .Include(v => v.ScheduleItems)
                .Include(v => v.PriceLists)
                .Include(v => v.Areas)
                .FirstOrDefault(v => v.VenueId == id);

            if (existingVenue == null)
            {
                return NotFound($"Venue with ID {id} not found.");
            }

            // Update fields
            existingVenue.Name = updatedVenue.Name;
            existingVenue.Address = updatedVenue.Address;
            existingVenue.Capacity = updatedVenue.Capacity;
            
            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteVenue(int id)
        {
            var venue = _dbContext.Venues.FirstOrDefault(v => v.VenueId == id);
            if (venue == null)
            {
                return NotFound($"Venue with ID {id} not found.");
            }

            _dbContext.Venues.Remove(venue);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
