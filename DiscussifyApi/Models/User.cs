namespace DiscussifyApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DateTimeCreated { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}

