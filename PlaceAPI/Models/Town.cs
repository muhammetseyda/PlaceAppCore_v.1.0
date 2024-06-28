namespace PlaceAPI.Models
{
    public class Town
    {
        public int Id { get; set; }
        public int TownCode { get; set; }
        public int CityId { get; set; }
        public string TownName { get; set; }
    }
}
