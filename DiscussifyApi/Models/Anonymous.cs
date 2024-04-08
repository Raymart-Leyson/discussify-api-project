namespace DiscussifyApi.Models
{
    public class Anonymous
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string? Name { get; set; }
        public string? DateTimeCreated { get; set; }
    }
}
