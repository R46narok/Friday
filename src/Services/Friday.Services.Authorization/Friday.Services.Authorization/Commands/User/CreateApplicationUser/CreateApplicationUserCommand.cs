using AutoMapper;
using Friday.Domain.Common;
using Friday.Services.Authorization.Entities;
using Friday.Services.Authorization.Services;
using MediatR;

namespace Friday.Services.Authorization.Commands.User.CreateApplicationUser;

public class CreateApplicationUserCommand : IRequest<ApiResponse<string>>
{
    public string UserName { get; set; }
        
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Password { get; set; }
}

public class CreateApplicationUserCommandHandler : IRequestHandler<CreateApplicationUserCommand, ApiResponse<string>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public CreateApplicationUserCommandHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<string>> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<ApplicationUser>(request);
        var result = await _identityService.CreateAsync(user, request.Password);

        user.CreatedDateTime = DateTime.Now;
        user.UpdatedDateTime = user.CreatedDateTime;

        if (result.Succeeded)
        {
            var created = await _identityService.FindByNameAsync(request.UserName);
            return new ApiResponse<string>(created.Id);
        }
        
        return new ApiResponse<string>(null, result.Errors.Select(x => x.Description));
    }
}