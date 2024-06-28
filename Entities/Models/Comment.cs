namespace Entities.Models
{
    public class Comment
    {
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
        public int ListId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Users? Users { get; set; }
        public int LikeCount { get; set; }
        public int IsLiked { get; set; }

    }
}
