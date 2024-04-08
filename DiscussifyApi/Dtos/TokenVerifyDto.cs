using System.ComponentModel.DataAnnotations;
using DiscussifyApi.Models;

namespace DiscussifyApi.Dtos
{
    public class TokenVerifyDto
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        public string? RefreshToken { get; set; }
    }
}
