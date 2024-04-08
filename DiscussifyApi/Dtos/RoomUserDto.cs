using DiscussifyApi.Models;

namespace DiscussifyApi.Dtos
{
    public class RoomUserDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? DateTimeCreated { get; set; }
        public string? DateTimeUpdated { get; set; }
    }
}
