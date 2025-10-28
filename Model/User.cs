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
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public DateTime CreatedAt { get; set; }

        private List<CustomProperty> CustomProperties = new();


        public User(int id, string firstname, string lastname, DateTime dateOfBirth, Sex sex)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            DateOfBirth = dateOfBirth;
            Sex = sex;
            CreatedAt = DateTime.UtcNow;
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
            return new UserDto(Id, Firstname, Lastname, DateOfBirth, Sex.ToString(), CustomProperties.Select(p => p.ToDto()).ToList());
        }
    }
}
