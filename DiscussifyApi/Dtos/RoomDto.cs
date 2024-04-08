using DiscussifyApi.Models;

namespace DiscussifyApi.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? DateTimeCreated { get; set; }
        public string? DateTimeUpdated { get; set; }
    }
}
