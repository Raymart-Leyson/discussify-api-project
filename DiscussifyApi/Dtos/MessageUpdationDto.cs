using System.ComponentModel.DataAnnotations;

namespace DiscussifyApi.Dtos
{
    public class MessageUpdationDto
    {
        [Required(ErrorMessage = "The content is required.")]
        public string? Content { get; set; }
    }
}
