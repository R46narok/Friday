using Friday.Services.Authorization.Queries.User.GetApplicationUser;
using Grpc.Core;
using MediatR;

namespace Friday.Services.Authorization.Grpc.Services;

public class UserService : User.UserBase
{
    private readonly IMediator _mediator;

    public UserService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetUserResponse> GetUserById(GetUserRequest request, ServerCallContext context)
    {
        var query = new GetApplicationUserQuery {Id = request.Id};
        var result = (await _mediator.Send(query)).Result;

        var response = new GetUserResponse
        {
            UserName = result.UserName,
            Email = result.Email,
            PhoneNumber = result.PhoneNumber
        };

        return response;
    }
    
}