using System.ComponentModel.DataAnnotations;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        [DataType(DataType.ImageUrl)]
        public string? Image { get; init; }
        
        [DataType(DataType.MultilineText)]
        public string? Biography { get; init; }
    }
}