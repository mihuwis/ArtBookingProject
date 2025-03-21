using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public TicketController(ArtBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CREATE
        [HttpPost("create")]
        public ActionResult<Ticket> CreateTicket([FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Invalid ticket data.");
            }

            _dbContext.Tickets.Add(ticket);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.TicketId }, ticket);
        }

        // GET ALL
        [HttpGet("all")]
        public ActionResult<IEnumerable<Ticket>> GetAllTickets()
        {
            var tickets = _dbContext.Tickets
                .Include(t => t.ScheduleItem)
                .Include(t => t.Area)
                .Include(t => t.Seat)
                .ToList();

            return Ok(tickets);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<Ticket> GetTicketById(int id)
        {
            var ticket = _dbContext.Tickets
                .Include(t => t.ScheduleItem)
                .Include(t => t.Area)
                .Include(t => t.Seat)
                .FirstOrDefault(t => t.TicketId == id);

            if (ticket == null)
            {
                return NotFound($"Ticket with ID {id} not found.");
            }

            return Ok(ticket);
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult UpdateTicket(int id, [FromBody] Ticket updatedTicket)
        {
            if (updatedTicket == null || updatedTicket.TicketId != id)
            {
                return BadRequest("Invalid ticket data or ID mismatch.");
            }

            var existingTicket = _dbContext.Tickets
                .Include(t => t.ScheduleItem)
                .Include(t => t.Area)
                .Include(t => t.Seat)
                .FirstOrDefault(t => t.TicketId == id);

            if (existingTicket == null)
            {
                return NotFound($"Ticket with ID {id} not found.");
            }

            // Update fields
            existingTicket.ScheduleItemId = updatedTicket.ScheduleItemId;
            existingTicket.AreaId = updatedTicket.AreaId;
            existingTicket.SeatId = updatedTicket.SeatId;
            existingTicket.Price = updatedTicket.Price;

            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteTicket(int id)
        {
            var ticket = _dbContext.Tickets.FirstOrDefault(t => t.TicketId == id);
            if (ticket == null)
            {
                return NotFound($"Ticket with ID {id} not found.");
            }

            _dbContext.Tickets.Remove(ticket);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
