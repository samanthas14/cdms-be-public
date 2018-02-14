

namespace services.Models
{
    public class County
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubprojectId { get; set; }
        public int ProjectId { get; set; }
    }
}