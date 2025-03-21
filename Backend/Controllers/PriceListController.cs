using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriceListController : ControllerBase
{
    private readonly ArtBookingDBContext _dbContext;

    public PriceListController(ArtBookingDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public ActionResult<PriceList> CreatePriceList([FromBody] PriceList priceList)
    {
        if (priceList == null)
        {
            return BadRequest("Invalid price entry data.");
        }
        
        _dbContext.PriceLists.Add(priceList);
        _dbContext.SaveChanges();
        
        return CreatedAtAction(nameof(GetPriceListById), new { id = priceList.PriceListId }, priceList);
    }

        // GET ALL
        [HttpGet]
        public ActionResult<IEnumerable<PriceList>> GetAllPriceLists()
        {
            var priceLists = _dbContext.PriceLists
                .Include(p => p.Venue)
                .Include(p => p.PriceEntries)
                .ToList();

            return Ok(priceLists);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<PriceList> GetPriceListById(int id)
        {
            var priceList = _dbContext.PriceLists
                .Include(p => p.Venue)
                .Include(p => p.PriceEntries)
                .FirstOrDefault(p => p.PriceListId == id);

            if (priceList == null)
            {
                return NotFound($"PriceList with ID {id} not found.");
            }

            return Ok(priceList);
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult UpdatePriceList(int id, [FromBody] PriceList updatedPriceList)
        {
            if (updatedPriceList == null || updatedPriceList.PriceListId != id)
            {
                return BadRequest("Invalid price list data or ID mismatch.");
            }

            var existingPriceList = _dbContext.PriceLists
                .Include(p => p.Venue)
                .Include(p => p.PriceEntries)
                .FirstOrDefault(p => p.PriceListId == id);

            if (existingPriceList == null)
            {
                return NotFound($"PriceList with ID {id} not found.");
            }

            // Update fields
            existingPriceList.Name = updatedPriceList.Name;
            existingPriceList.VenueId = updatedPriceList.VenueId;

            // Update PriceEntries
            existingPriceList.PriceEntries.Clear();
            foreach (var entry in updatedPriceList.PriceEntries)
            {
                var existingEntry = _dbContext.PriceEntries.FirstOrDefault(e => e.PriceEntryId == entry.PriceEntryId);
                if (existingEntry != null)
                {
                    existingPriceList.PriceEntries.Add(existingEntry);
                }
                else
                {
                    existingPriceList.PriceEntries.Add(entry);
                }
            }

            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeletePriceList(int id)
        {
            var priceList = _dbContext.PriceLists.FirstOrDefault(p => p.PriceListId == id);
            if (priceList == null)
            {
                return NotFound($"PriceList with ID {id} not found.");
            }

            _dbContext.PriceLists.Remove(priceList);
            _dbContext.SaveChanges();

            return NoContent();
        }
    
    
}