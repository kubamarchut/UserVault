using UserVault.Dtos;

namespace UserVault.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public List<CustomPropertyDto> CustomProperties { get; set; }
        public UserDto(int id, string firstname, string lastname, DateTime dateOfBirth, string sex, List<CustomPropertyDto> customProperties)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            DateOfBirth = dateOfBirth;
            Sex = sex;
            CustomProperties = customProperties;
        }
    }
}
