using marka_api.Data;
using marka_api.Interfaces;
using marka_api.Models;
using marka_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace marka_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public UserController(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }
            var createdUser = await _userRepository.CreateUser(user);
            return CreatedAtAction(nameof(GetAllUsers), new { id = createdUser.id }, createdUser);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return user;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            var result = await _userRepository.DeleteUser(id);
            if (!result)
            {
                return NotFound("User not found.");
            }
            return NoContent();
        }
    }
}
