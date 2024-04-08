using DiscussifyApi.Models;

namespace DiscussifyApi.Dtos
{
    public class UserRoomDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? DateTimeCreated { get; set; }
        public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();
    }
}
