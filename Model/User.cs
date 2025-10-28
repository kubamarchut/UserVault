using UserVault.Dtos;

namespace UserVault.Model
{
    public enum Sex
    {
        Male,
        Female,
    }
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }

        private List<CustomProperty> CustomProperties = new();


        public User(int id, string firstname, string lastname, DateTime dateOfBirth, Sex sex)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            DateOfBirth = dateOfBirth;
            Sex = sex;
        }

        public void AddCustomProperty(CustomProperty property)
        {
            CustomProperties.Add(property);
        }

        public IEnumerable<CustomProperty> GetCustomProperties()
        {
            return CustomProperties;
        }

        public string ShowCustomProperties()
        {
            return string.Join(", ", CustomProperties.Select(p => $"{p.Name}: {p.Value}"));
        }

        public string GetTitle()
        {
            return this.Sex == Sex.Male ? "Pan" : "Pani";
        }

        public int GetAge()
        {
            int age = DateTime.UtcNow.Year - DateOfBirth.Year;
            if (DateOfBirth < DateTime.UtcNow.AddYears(age))
            {
                age--;
            }
            return age;
        }
        public UserDto ToDto()
        {
            return new UserDto(Id, FirstName, LastName, DateOfBirth, Sex.ToString(), CustomProperties.Select(p => p.ToDto()).ToList());
        }
        public static User FromDto(UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            var user = new User(
                userDto.Id,
                userDto.Firstname,
                userDto.Lastname,
                userDto.DateOfBirth,
                Enum.TryParse<Sex>(userDto.Sex, true, out var sex) ? sex : throw new ArgumentException("Invalid sex value")
            );

            if (userDto.CustomProperties != null)
            {
                foreach (var propDto in userDto.CustomProperties)
                {
                    user.AddCustomProperty(CustomProperty.FromDto(propDto, user.Id));
                }
            }

            return user;
        }
    }
}
