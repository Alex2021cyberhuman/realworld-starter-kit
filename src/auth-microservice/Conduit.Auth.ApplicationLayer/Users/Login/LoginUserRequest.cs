using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Passwords;
using Conduit.Auth.Domain.Users.Repositories;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Login
{
    public class LoginUserRequest
        : IRequest<Outcome<UserResponseModel>>,
            IRequest<Outcome<UserResponse>>
    {
        public LoginUserRequest(LoginUserModel user)
        {
            User = user;
        }

        public LoginUserModel User { get; set; }
    }

    public class LoginUserModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class LoginUserRequestHandler
        : IRequestHandler<LoginUserRequest, Outcome<UserResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserRequestHandler(
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

        #region IRequestHandler<LoginUserRequest,Outcome<UserResponse>> Members

        public async Task<Outcome<UserResponse>> Handle(
            LoginUserRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.FindUserByPasswordEmailAsync(
                request.User.Password,
                request.User.Email,
                _passwordManager,
                cancellationToken);
            if (user is null)
                return Outcome.New<UserResponse>(OutcomeType.Banned);
            var token = await _tokenProvider.CreateTokenAsync(
                user,
                cancellationToken);
            var response = new UserResponse(user, token);
            return Outcome.New(response);
        }

        #endregion
    }
}
