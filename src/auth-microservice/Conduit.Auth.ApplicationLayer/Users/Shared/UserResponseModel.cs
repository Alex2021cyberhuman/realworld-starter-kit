using System.ComponentModel.DataAnnotations;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class UserResponseModel
    {
        public UserResponseModel(User user, TokenOutput token)
        {
            Username = user.Username;
            Email = user.Email;
            Image = user.Image;
            Biography = user.Biography;
            Token = token.AccessToken;
        }

        [Required]
        [DataType(DataType.Text)]
        public string Token { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.ImageUrl)]
        public string? Image { get; init; }

        [DataType(DataType.MultilineText)]
        public string? Biography { get; init; }
    }
}
