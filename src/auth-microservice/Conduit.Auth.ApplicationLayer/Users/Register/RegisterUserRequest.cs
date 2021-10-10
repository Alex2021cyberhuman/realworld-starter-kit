﻿using System.ComponentModel.DataAnnotations;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserRequest : IRequest<Outcome<UserResponse>>
    {
        [Required]
        public UserRequest UserRequest { get; set; } = new();
    }
}