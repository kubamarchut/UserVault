using Microsoft.AspNetCore.Mvc;
using UserVault.Dtos;
using UserVault.Model;

namespace UserVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Repositories.UserRepository _userRepository;
        public UsersController(Repositories.UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public ActionResult<List<UserDto>> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            var usersDto = users.Select(user => user.ToDto()).ToList();

            return Ok(usersDto);
        }
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = user.ToDto();
            return Ok(userDto);
        }
        [HttpPost]
        public ActionResult<UserDto> CreateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            var user = Model.User.FromDto(userDto);

            try
            {
                _userRepository.AddUser(user);

                userDto.Id = user.Id;
                var customProperties = user.GetCustomProperties().ToList();
                for (int i = 0; i < userDto.CustomProperties.Count; i++)
                {
                    userDto.CustomProperties[i].Id = customProperties[i].Id;
                }

                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            if (id != userDto.Id)
                return BadRequest("User ID in URL does not match ID in body.");

            try
            {
                var user = Model.User.FromDto(userDto);

                _userRepository.UpdateUser(user);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {id} not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid data: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating user: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            try
            {
                _userRepository.RemoveUser(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting user: {ex.Message}");
            }
        }

    }
}
