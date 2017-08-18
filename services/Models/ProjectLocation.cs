namespace services.Models
{
// not using this yet    
    public class ProjectLocation
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        public int LocationId { get; set; }
        public int DatasetId { get; set; }

        public string Label { get; set; }
        public string Description { get; set; }
    }
}