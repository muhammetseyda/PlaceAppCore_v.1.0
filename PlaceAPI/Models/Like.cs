namespace PlaceAPI.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string UserId { get; set; }
        public int LikeCount { get; set; }
        public DateTime LikeTime { get; set; }
    }
}
