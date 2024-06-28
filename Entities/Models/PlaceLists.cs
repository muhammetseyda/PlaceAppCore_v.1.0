namespace Entities.Models
{
    public class PlaceLists
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ListName { get; set; }
        public string? ListDescription { get; set; }
        public List<Places> Places { get; set; }
        public string PlaceIds { get; set; }
        public Users Users { get; set; }
    }
}
