namespace PlaceApp.Models.ViewModel
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }

    }
}
