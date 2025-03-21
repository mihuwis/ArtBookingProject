using BusinessModel.Data;
using BusinessModel.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ArtBookingDBContext _dbContext;

        public UserController(ArtBookingDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CREATE
        [HttpPost("create")]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        // GET ALL
        [HttpGet("all")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _dbContext.Users
                .Include(u => u.ArtOrganization)
                .ToList();

            return Ok(users);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _dbContext.Users
                .Include(u => u.ArtOrganization)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return Ok(user);
        }

        // UPDATE
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (updatedUser == null || updatedUser.UserId != id)
            {
                return BadRequest("Invalid user data or ID mismatch.");
            }

            var existingUser = _dbContext.Users
                .Include(u => u.ArtOrganization)
                .FirstOrDefault(u => u.UserId == id);

            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            // Update fields
            existingUser.LoginName = updatedUser.LoginName;
            existingUser.PasswordHash = updatedUser.PasswordHash;
            existingUser.Email = updatedUser.Email;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.UserRole = updatedUser.UserRole;
            existingUser.ArtOrganizationId = updatedUser.ArtOrganizationId;

            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
