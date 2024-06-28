namespace PlaceApp.Models.Profile
{
    public class ProfileViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public List<Places> Places { get; set; }
        public List<PlaceLists> PlaceLists { get; set; }
        public List<SharePlaceList> SharePlaceList { get; set; }
    }
}
