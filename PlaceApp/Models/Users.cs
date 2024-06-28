namespace PlaceApp.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public List<Places> Places { get; set; }
        public List<PlaceLists> PlaceLists { get; set; }
        public List<SharePlace> SharePlaces { get; set; }   
        public List<SharePlaceList> SharePlacesList { get; set; }   
    }
}
