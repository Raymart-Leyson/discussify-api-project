using System.ComponentModel.DataAnnotations;

namespace DiscussifyApi.Dtos
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "The emailAddress is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the emailAddress is 50 characters.")]
        [RegularExpression(
            @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$",
            ErrorMessage = "The email address is not valid"
        )]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the password is 50 characters.")]
        [RegularExpression(
            "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$",
            ErrorMessage = "The password must have at least eight characters, one letter, and one number")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "The firstName is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the firstName is 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "The lastName is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the lastName is 50 characters.")]
        public string? LastName { get; set; }
    }
}

