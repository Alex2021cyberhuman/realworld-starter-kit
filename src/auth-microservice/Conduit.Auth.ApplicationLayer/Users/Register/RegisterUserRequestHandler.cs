using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserRequestHandler
        : IRequestHandler<RegisterUserRequest, Outcome<UserResponse>>
    {
        #region IRequestHandler<RegisterUserRequest,Outcome<UserResponse>> Members

        public Task<Outcome<UserResponse>> Handle(
            RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
