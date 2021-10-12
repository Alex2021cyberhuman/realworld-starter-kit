using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Passwords;
using Conduit.Auth.Domain.Users.Repositories;
using FluentValidation;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, Outcome<UserResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateUserRequest> _validator;

        public UpdateUserRequestHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITokenProvider tokenProvider,
            IPasswordManager passwordManager,
            IValidator<UpdateUserRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenProvider = tokenProvider;
            _passwordManager = passwordManager;
            _validator = validator;
        }

        public async Task<Outcome<UserResponse>> Handle(
            UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Outcome.Reject<UserResponse>(validationResult);
            var user = await UpdateUserAsync(request, cancellationToken);
            var token =
                await _tokenProvider.CreateTokenAsync(user, cancellationToken);
            var response = new UserResponse(user, token);
            return Outcome.New(response);
        }
        
        private async Task<User> UpdateUserAsync(
            UpdateUserRequest request,
            CancellationToken cancellationToken)
        {
            var newUser = _mapper.Map<UpdateUserModel, User>(request.User);
            var user = await _unitOfWork.HashPasswordAndUpdateUserAsync(
                newUser,
                _passwordManager,
                cancellationToken);
            return user;
        }
    }
}