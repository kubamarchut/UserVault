namespace UserVault.Model
{
    public class CustomProperty
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
    }
}
