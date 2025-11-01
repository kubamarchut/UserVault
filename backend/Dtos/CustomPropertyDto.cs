namespace UserVault.Dtos
{
    public class CustomPropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public CustomPropertyDto(int id, string name, string value)
        {
            Id = id;
            Name = name;
            Value = value;
        }
    }
}
