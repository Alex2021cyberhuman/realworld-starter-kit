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

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserRequestHandler
        : IRequestHandler<RegisterUserRequest, Outcome<UserResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RegisterUserRequest> _validator;

        public RegisterUserRequestHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITokenProvider tokenProvider,
            IPasswordManager passwordManager,
            IValidator<RegisterUserRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenProvider = tokenProvider;
            _passwordManager = passwordManager;
            _validator = validator;
        }

        #region IRequestHandler<RegisterUserRequest,Outcome<UserResponse>> Members

        public async Task<Outcome<UserResponse>> Handle(
            RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Outcome.Reject<UserResponse>(validationResult);
            var user = await CreateUserAsync(request, cancellationToken);
            var token =
                await _tokenProvider.CreateTokenAsync(user, cancellationToken);
            var response = new UserResponse(user, token);
            return Outcome.New(response);
        }

        #endregion

        private async Task<User> CreateUserAsync(
            RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            var newUser = _mapper.Map<RegisterUserModel, User>(request.User);
            newUser = newUser with
            {
                Password = _passwordManager.HashPassword(
                    request.User.Password,
                    newUser)
            };
            var user = await _unitOfWork.CreateUserAsync(
                newUser,
                cancellationToken);
            return user;
        }
    }
}
