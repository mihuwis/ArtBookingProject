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

        [HttpGet]
        public ActionResult<IEnumerable<Venue>> GetAll()
        {
            return Ok(_dbContext.Venues.Include(v => v.Areas).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Venue> GetById(int id)
        {
            var entity = _dbContext.Venues.Include(v => v.Areas).FirstOrDefault(v => v.VenueId == id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public ActionResult<Venue> Create([FromBody] Venue entity)
        {
            _dbContext.Venues.Add(entity);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = entity.VenueId }, entity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Venue updatedEntity)
        {
            if (id != updatedEntity.VenueId) return BadRequest();
            _dbContext.Entry(updatedEntity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _dbContext.Venues.Find(id);
            if (entity == null) return NotFound();
            _dbContext.Venues.Remove(entity);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}

