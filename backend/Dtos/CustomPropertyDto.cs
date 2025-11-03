using System.ComponentModel.DataAnnotations;

namespace UserVault.Dtos
{
    public class CustomPropertyDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Value { get; set; }
        public CustomPropertyDto(int id, string name, string value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
