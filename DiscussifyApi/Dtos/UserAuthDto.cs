using System.ComponentModel.DataAnnotations;

namespace DiscussifyApi.Dtos
{
    public class UserAuthDto
    {
        [Required(ErrorMessage = "The emailAddress is required.")]
        public string? EmailAddress { get; set; }
        
        [Required(ErrorMessage = "The password is required.")]
        public string? Password { get; set; }
    }
}
