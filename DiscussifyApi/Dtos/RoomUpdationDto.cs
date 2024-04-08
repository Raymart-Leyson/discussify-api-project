using System.ComponentModel.DataAnnotations;

namespace DiscussifyApi.Dtos
{
    public class RoomUpdationDto
    {
        [Required(ErrorMessage = "The Name is required.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; set; }
    }
}
