using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtEventController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public ArtEventController(ArtBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArtEvent>> GetAll()
        {
            return Ok(_dbContext.ArtEvents.Include(e => e.ScheduleItems).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<ArtEvent> GetById(int id)
        {
            var entity = _dbContext.ArtEvents.Include(e => e.ScheduleItems).FirstOrDefault(e => e.ArtEventId == id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public ActionResult<ArtEvent> Create([FromBody] ArtEvent entity)
        {
            _dbContext.ArtEvents.Add(entity);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = entity.ArtEventId }, entity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ArtEvent updatedEntity)
        {
            if (id != updatedEntity.ArtEventId) return BadRequest();
            _dbContext.Entry(updatedEntity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _dbContext.ArtEvents.Find(id);
            if (entity == null) return NotFound();
            _dbContext.ArtEvents.Remove(entity);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
