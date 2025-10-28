using UserVault.Dtos;

namespace UserVault.Model
{
    public class CustomProperty
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }

        public CustomPropertyDto ToDto()
        {
            return new CustomPropertyDto(Id, Name, Value);
        }
    }
}
