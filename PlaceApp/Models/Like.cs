namespace PlaceApp.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int ContentType { get; set; }
        public string UserId { get; set; }
        public int LikeCount { get; set; }
        public DateTime LikeTime { get; set; }
        public DateTime? UnLikeTime { get; set; }
        public int IsLiked { get; set; }
    }
}
