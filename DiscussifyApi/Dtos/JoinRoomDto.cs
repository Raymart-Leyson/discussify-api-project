using System.ComponentModel.DataAnnotations;

namespace DiscussifyApi.Dtos
{
    public class JoinRoomDto
    {
        [Required(ErrorMessage = "Room code is required")]
        public string? Code { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
    }
}
