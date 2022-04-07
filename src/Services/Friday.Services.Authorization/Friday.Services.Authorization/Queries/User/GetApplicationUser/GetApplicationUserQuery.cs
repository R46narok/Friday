using AutoMapper;
using Friday.Domain.Common;
using Friday.Services.Authorization.DTOs;
using Friday.Services.Authorization.Services;
using MediatR;

namespace Friday.Services.Authorization.Queries.User.GetApplicationUser;

public class GetApplicationUserQuery : IRequest<ApiResponse<GetUserDto>>
{
    public string Id { get; set; }
}

public class GetApplicationUserQueryHandler : IRequestHandler<GetApplicationUserQuery, ApiResponse<GetUserDto>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetApplicationUserQueryHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<GetUserDto>> Handle(GetApplicationUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByIdAsync(request.Id);
        var dto = _mapper.Map<GetUserDto>(user);
        
        return new ApiResponse<GetUserDto>(dto);
    }
}