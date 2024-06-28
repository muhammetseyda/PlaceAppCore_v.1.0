namespace Entities.Models
{
    public class Places
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string City  { get; set; }
        public string Town { get; set; }
        public string? Address { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
        public string? WebSiteLink { get; set; }
        public string? MapsLink { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdateOn { get; set; }
        public bool Went { get; set; }
        public bool RetryWent { get; set; }
        public string? Description { get; set; }
        public string? InstaUrl { get; set; }
        public float AtmosphereRating { get; set; }
        public float FoodRating { get; set; }
        public float ServiceRating { get; set; }
        public string? UserRating { get; set; }
        public Users Users { get; set; }
    }
}
