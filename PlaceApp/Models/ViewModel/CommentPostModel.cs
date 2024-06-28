namespace PlaceApp.Models.ViewModel
{
    public class CommentPostModel
    {
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
        public int ListId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
