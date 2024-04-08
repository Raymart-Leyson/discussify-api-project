using System.ComponentModel.DataAnnotations;
using DiscussifyApi.Models;

namespace DiscussifyApi.Dtos
{
    public class TokenRenewDto
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        public string? RefreshToken { get; set; }
    }
}
