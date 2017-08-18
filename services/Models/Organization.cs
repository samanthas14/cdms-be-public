namespace services.Models
{
    public class Organization
    {
        public const int DEFAULT_ORGANIZATION_ID = 1;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}