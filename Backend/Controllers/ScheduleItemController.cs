using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleItemController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public ScheduleItemController(ArtBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CREATE
        [HttpPost("create")]
        public ActionResult<ScheduleItem> CreateScheduleItem([FromBody] ScheduleItem scheduleItem)
        {
            if (scheduleItem == null)
            {
                return BadRequest("Invalid schedule item data.");
            }

            _dbContext.ScheduleItems.Add(scheduleItem);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetScheduleItemById), new { id = scheduleItem.ScheduleItemId }, scheduleItem);
        }

        // GET ALL
        [HttpGet("all")]
        public ActionResult<IEnumerable<ScheduleItem>> GetAllScheduleItems()
        {
            var scheduleItems = _dbContext.ScheduleItems
                .Include(s => s.ArtEvent)
                .Include(s => s.Venue)
                .ToList();

            return Ok(scheduleItems);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<ScheduleItem> GetScheduleItemById(int id)
        {
            var scheduleItem = _dbContext.ScheduleItems
                .Include(s => s.ArtEvent)
                .Include(s => s.Venue)
                .FirstOrDefault(s => s.ScheduleItemId == id);

            if (scheduleItem == null)
            {
                return NotFound($"ScheduleItem with ID {id} not found.");
            }

            return Ok(scheduleItem);
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult UpdateScheduleItem(int id, [FromBody] ScheduleItem updatedScheduleItem)
        {
            if (updatedScheduleItem == null || updatedScheduleItem.ScheduleItemId != id)
            {
                return BadRequest("Invalid schedule item data or ID mismatch.");
            }

            var existingScheduleItem = _dbContext.ScheduleItems
                .Include(s => s.ArtEvent)
                .Include(s => s.Venue)
                .FirstOrDefault(s => s.ScheduleItemId == id);

            if (existingScheduleItem == null)
            {
                return NotFound($"ScheduleItem with ID {id} not found.");
            }

            // Update fields
            existingScheduleItem.EventDate = updatedScheduleItem.EventDate;
            existingScheduleItem.ArtEventId = updatedScheduleItem.ArtEventId;
            existingScheduleItem.VenueId = updatedScheduleItem.VenueId;

            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteScheduleItem(int id)
        {
            var scheduleItem = _dbContext.ScheduleItems.FirstOrDefault(s => s.ScheduleItemId == id);
            if (scheduleItem == null)
            {
                return NotFound($"ScheduleItem with ID {id} not found.");
            }

            _dbContext.ScheduleItems.Remove(scheduleItem);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
