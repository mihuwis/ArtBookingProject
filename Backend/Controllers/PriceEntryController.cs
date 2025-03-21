using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceEntryController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public PriceEntryController(ArtBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CREATE
        [HttpPost]
        public ActionResult<PriceEntry> CreatePriceEntry([FromBody] PriceEntry priceEntry)
        {
            if (priceEntry == null)
            {
                return BadRequest("Invalid price entry data.");
            }

            _dbContext.PriceEntries.Add(priceEntry);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetPriceEntryById), new { id = priceEntry.PriceEntryId }, priceEntry);
        }

        // GET ALL
        [HttpGet]
        public ActionResult<IEnumerable<PriceEntry>> GetAllPriceEntries()
        {
            var priceEntries = _dbContext.PriceEntries
                .Include(p => p.PriceList)
                .Include(p => p.Area)
                .ToList();

            return Ok(priceEntries);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<PriceEntry> GetPriceEntryById(int id)
        {
            var priceEntry = _dbContext.PriceEntries
                .Include(p => p.PriceList)
                .Include(p => p.Area)
                .FirstOrDefault(p => p.PriceEntryId == id);

            if (priceEntry == null)
            {
                return NotFound($"PriceEntry with ID {id} not found.");
            }

            return Ok(priceEntry);
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult UpdatePriceEntry(int id, [FromBody] PriceEntry updatedPriceEntry)
        {
            if (updatedPriceEntry == null || updatedPriceEntry.PriceEntryId != id)
            {
                return BadRequest("Invalid price entry data or ID mismatch.");
            }

            var existingPriceEntry = _dbContext.PriceEntries
                .Include(p => p.PriceList)
                .Include(p => p.Area)
                .FirstOrDefault(p => p.PriceEntryId == id);

            if (existingPriceEntry == null)
            {
                return NotFound($"PriceEntry with ID {id} not found.");
            }

            // Update fields
            existingPriceEntry.Price = updatedPriceEntry.Price;
            existingPriceEntry.PriceListId = updatedPriceEntry.PriceListId;
            existingPriceEntry.AreaId = updatedPriceEntry.AreaId;

            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeletePriceEntry(int id)
        {
            var priceEntry = _dbContext.PriceEntries.FirstOrDefault(p => p.PriceEntryId == id);
            if (priceEntry == null)
            {
                return NotFound($"PriceEntry with ID {id} not found.");
            }

            _dbContext.PriceEntries.Remove(priceEntry);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
