namespace services.Models
{
    public class UserPreference
    {
        public const string DATASETS = "Datasets";
        public const string FAVORITES = "Favorites";
        public const string PROJECTS = "Projects";

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}