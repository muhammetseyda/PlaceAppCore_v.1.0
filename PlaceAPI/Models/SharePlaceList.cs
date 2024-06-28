namespace PlaceAPI.Models
{
    public class SharePlaceList
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ListId { get; set; }
        public string UserName { get; set; }
        public string? UserDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ListName { get; set; }
        public string? ListDescription { get; set; }
        public List<Places> Places { get; set; }
        public List<SharePlace> SharePlace { get; set; }
        public string? PlaceIds { get; set; }
    }
}
