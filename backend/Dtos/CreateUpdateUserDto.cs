using System.ComponentModel.DataAnnotations;

namespace UserVault.Dtos
{
    public class CreateUpdateUserDto : IValidatableObject
    {
        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(150)]
        public string Lastname { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [RegularExpression("Male|Female", ErrorMessage = "Sex must be 'Male' or 'Female'.")]
        public string Sex { get; set; }
        public List<CustomPropertyDto> CustomProperties { get; set; }

        public CreateUpdateUserDto(string firstname, string lastname, DateTime dateOfBirth, string sex, List<CustomPropertyDto> customProperties)
        {
            Firstname = firstname;
            Lastname = lastname;
            DateOfBirth = dateOfBirth;
            Sex = sex;
            CustomProperties = customProperties;
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth > DateTime.UtcNow)
            {
                yield return new ValidationResult(
                    "Date of birth cannot be in the future.",
                    new[] { nameof(DateOfBirth) }
                );
            }
        }
    }
}
