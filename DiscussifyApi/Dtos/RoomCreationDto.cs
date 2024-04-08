using System.ComponentModel.DataAnnotations;

namespace DiscussifyApi.Dtos
{
    public class RoomCreationDto
    {
        [Required(ErrorMessage = "The userId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The Name is required.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; set; }
    }
}
