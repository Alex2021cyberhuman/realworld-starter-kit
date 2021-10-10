﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Passwords;
using Conduit.Auth.Domain.Users.Repositories;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserRequestHandler
        : IRequestHandler<RegisterUserRequest, Outcome<UserResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ITokenProvider _tokenProvider;
        private readonly IPasswordManager _passwordManager;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserRequestHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITokenProvider tokenProvider,
            IPasswordManager passwordManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenProvider = tokenProvider;
            _passwordManager = passwordManager;
        }

        #region IRequestHandler<RegisterUserRequest,Outcome<UserResponse>> Members

        public async Task<Outcome<UserResponse>> Handle(
            RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            var newUser =
                _mapper.Map<RegisterUserModel, User>(request.RegisterUserModel);
            newUser = newUser with
            {
                Password = _passwordManager.HashPassword(
                    request.RegisterUserModel.Password,
                    newUser)
            };
            var user = await _unitOfWork.CreateUserAsync(
                newUser,
                cancellationToken);
            var token =
                await _tokenProvider.CreateTokenAsync(user, cancellationToken);
            var response = new UserResponse(user, token);
            return Outcome.New(response);
        }

        #endregion
    }
}
