using Microsoft.AspNetCore.Mvc;
using UserVault.Dtos;

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
        public ActionResult CreateUser([FromBody] UserDto userDto)
        {
            return BadRequest("User creation not implemented yet.");
        }
        [HttpPut("{id}")]
        public ActionResult UpdateUser([FromBody] UserDto userDto)
        {
            return BadRequest("User update not implemented yet.");
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromBody] UserDto userDto)
        {
            return BadRequest("User deletion not implemented yet.");
        }
    }
}
