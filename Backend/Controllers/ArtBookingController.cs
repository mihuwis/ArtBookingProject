using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtBookingController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public ArtBookingController(ArtBookingDBContext dBContext) {
            _dbContext = dBContext;
        }

        [HttpPost]
        public ActionResult<ArtOrganization> CreateOrganization([FromBody] ArtOrganization organization)
        {
            if (organization == null)
            {
                return BadRequest("Invalid organization data.");
            }

            _dbContext.ArtOrganizations.Add(organization);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetOrganizationById), new { id = organization.ArtOrganizationId }, organization);
        }

        [HttpGet("{id}")]
        public ActionResult<ArtOrganization> GetOrganizationById(int id)
        {
            var organization = _dbContext.ArtOrganizations.FirstOrDefault(o => o.ArtOrganizationId == id);
            if (organization == null)
            {
                return NotFound($"Organization with ID {id} not found.");
            }
            return Ok(organization);
        }
        // Get All - Punkt C - czyli pobranie wszystkich
        [HttpGet]
        public ActionResult<IEnumerable<ArtOrganization>> GetAllOrganizations()
        {
            var organizations = _dbContext.ArtOrganizations.Include(o => o.Users).ToList();
            return Ok(organizations);
        }

        // Edycja 
        [HttpPut("{id}")]
        public ActionResult UpdateOrganization(int id, [FromBody] ArtOrganization updatedOrganization)
        {
            if (updatedOrganization == null || updatedOrganization.ArtOrganizationId != id)
            {
                return BadRequest("Invalid organization data or ID mismatch.");
            }

            var existingOrganization = _dbContext.ArtOrganizations.Include(o => o.Users).FirstOrDefault(o => o.ArtOrganizationId == id);
            if (existingOrganization == null)
            {
                return NotFound($"Organization with ID {id} not found.");
            }

            // Update fields
            existingOrganization.Name = updatedOrganization.Name;
            existingOrganization.Description = updatedOrganization.Description;
            existingOrganization.Email = updatedOrganization.Email;

            // Update users
            existingOrganization.Users.Clear();
            foreach (var user in updatedOrganization.Users)
            {
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.UserId == user.UserId);
                if (existingUser != null)
                {
                    existingOrganization.Users.Add(existingUser);
                }
                else
                {
                    existingOrganization.Users.Add(user);
                }
            }

            _dbContext.SaveChanges();
            return NoContent();
        }


        // Delete - Punkt B - czyli usuniecie
        [HttpDelete("{id}")]
        public ActionResult DeleteOrganization(int id)
        {
            var organization = _dbContext.ArtOrganizations.FirstOrDefault(o => o.ArtOrganizationId == id);
            if (organization == null)
            {
                return NotFound($"Organization with ID {id} not found.");
            }

            _dbContext.ArtOrganizations.Remove(organization);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}

