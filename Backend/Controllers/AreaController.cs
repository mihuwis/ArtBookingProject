using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreaController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public AreaController(ArtBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CREATE
        [HttpPost]
        public ActionResult<Area> CreateArea([FromBody] Area area)
        {
            if (area == null)
            {
                return BadRequest("Invalid area data.");
            }

            _dbContext.Areas.Add(area);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetAreaById), new { id = area.AreaId }, area);
        }

        // GET ALL
        [HttpGet]
        public ActionResult<IEnumerable<Area>> GetAllAreas()
        {
            var areas = _dbContext.Areas
                .Include(a => a.Seats)
                .Include(a => a.Tickets)
                .ToList();
            return Ok(areas);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<Area> GetAreaById(int id)
        {
            var area = _dbContext.Areas
                .Include(a => a.Seats)
                .Include(a => a.Tickets)
                .FirstOrDefault(a => a.AreaId == id);

            if (area == null)
            {
                return NotFound($"Area with ID {id} not found.");
            }
            return Ok(area);
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult UpdateArea(int id, [FromBody] Area updatedArea)
        {
            if (updatedArea == null || updatedArea.AreaId != id)
            {
                return BadRequest("Invalid area data or ID mismatch.");
            }

            var existingArea = _dbContext.Areas
                .Include(a => a.Seats)
                .Include(a => a.Tickets)
                .FirstOrDefault(a => a.AreaId == id);

            if (existingArea == null)
            {
                return NotFound($"Area with ID {id} not found.");
            }

            // Update fields
            existingArea.Name = updatedArea.Name;
            existingArea.VenueId = updatedArea.VenueId;

            // Update seats
            existingArea.Seats.Clear();
            foreach (var seat in updatedArea.Seats)
            {
                var existingSeat = _dbContext.Seats.FirstOrDefault(s => s.SeatId == seat.SeatId);
                if (existingSeat != null)
                {
                    existingArea.Seats.Add(existingSeat);
                }
                else
                {
                    existingArea.Seats.Add(seat);
                }
            }

            // Update tickets
            existingArea.Tickets.Clear();
            foreach (var ticket in updatedArea.Tickets)
            {
                var existingTicket = _dbContext.Tickets.FirstOrDefault(t => t.TicketId == ticket.TicketId);
                if (existingTicket != null)
                {
                    existingArea.Tickets.Add(existingTicket);
                }
                else
                {
                    existingArea.Tickets.Add(ticket);
                }
            }

            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteArea(int id)
        {
            var area = _dbContext.Areas.FirstOrDefault(a => a.AreaId == id);
            if (area == null)
            {
                return NotFound($"Area with ID {id} not found.");
            }

            _dbContext.Areas.Remove(area);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
