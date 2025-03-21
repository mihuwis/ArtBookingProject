using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public SeatController(ArtBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CREATE
        [HttpPost("create")]
        public ActionResult<Seat> CreateSeat([FromBody] Seat seat)
        {
            if (seat == null)
            {
                return BadRequest("Invalid seat data.");
            }

            _dbContext.Seats.Add(seat);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetSeatById), new { id = seat.SeatId }, seat);
        }

        // GET ALL
        [HttpGet("all")]
        public ActionResult<IEnumerable<Seat>> GetAllSeats()
        {
            var seats = _dbContext.Seats
                .Include(s => s.Area)
                .ToList();

            return Ok(seats);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<Seat> GetSeatById(int id)
        {
            var seat = _dbContext.Seats
                .Include(s => s.Area)
                .FirstOrDefault(s => s.SeatId == id);

            if (seat == null)
            {
                return NotFound($"Seat with ID {id} not found.");
            }

            return Ok(seat);
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult UpdateSeat(int id, [FromBody] Seat updatedSeat)
        {
            if (updatedSeat == null || updatedSeat.SeatId != id)
            {
                return BadRequest("Invalid seat data or ID mismatch.");
            }

            var existingSeat = _dbContext.Seats
                .Include(s => s.Area)
                .FirstOrDefault(s => s.SeatId == id);

            if (existingSeat == null)
            {
                return NotFound($"Seat with ID {id} not found.");
            }

            // Update fields
            existingSeat.SeatNumber = updatedSeat.SeatNumber;
            existingSeat.AreaId = updatedSeat.AreaId;

            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteSeat(int id)
        {
            var seat = _dbContext.Seats.FirstOrDefault(s => s.SeatId == id);
            if (seat == null)
            {
                return NotFound($"Seat with ID {id} not found.");
            }

            _dbContext.Seats.Remove(seat);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
