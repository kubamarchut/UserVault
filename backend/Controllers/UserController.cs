using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Data;
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
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        public ActionResult<UserDto> CreateUser([FromBody] CreateUpdateUserDto userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            var user = Model.User.FromDto(userDto);

            try
            {
                _userRepository.AddUser(user);

                var customProperties = user.GetCustomProperties().ToList();
                for (int i = 0; i < userDto.CustomProperties.Count; i++)
                {
                    userDto.CustomProperties[i].Id = customProperties[i].Id;
                }

                var newUser = _userRepository.GetUserById(user.Id);

                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, newUser.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult UpdateUser(int id, [FromBody] CreateUpdateUserDto userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            try
            {
                var user = Model.User.FromDto(id, userDto);

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

        [HttpGet("export/xlsx")]
        public ActionResult ExportUsersToXlsx()
        {
            var users = _userRepository.GetAllUsers();

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("User Data");

            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("ID");
            headerRow.CreateCell(1).SetCellValue("Firstname");
            headerRow.CreateCell(2).SetCellValue("Lastname");
            headerRow.CreateCell(3).SetCellValue("Title");
            headerRow.CreateCell(4).SetCellValue("Age");

            int rowNum = 1;
            foreach (var user in users)
            {
                IRow row = sheet.CreateRow(rowNum++);

                row.CreateCell(0).SetCellValue(user.Id);
                row.CreateCell(1).SetCellValue(user.FirstName);
                row.CreateCell(2).SetCellValue(user.LastName);
                row.CreateCell(3).SetCellValue(user.GetTitle());
                row.CreateCell(4).SetCellValue(user.GetAge());
            }
            
            using (var stream = new MemoryStream())
            {
                workbook.Write(stream);
                var fileContents = stream.ToArray();

                const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"UserData-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";

                return File(fileContents, contentType, fileName);
            }
        }
    }
}
