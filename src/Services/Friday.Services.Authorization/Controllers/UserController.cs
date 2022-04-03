using AutoMapper;
using Friday.Domain.Common;
using Friday.Services.Authorization.Commands.User.CreateApplicationUser;
using Friday.Services.Authorization.Common.Queries.User.GetApplicationUser;
using Friday.Services.Authorization.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Friday.Services.Authorization.Controllers;

[ApiController, Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<string>>> CreateUser([FromBody] CreateUserDto dto)
    {
        var command = _mapper.Map<CreateApplicationUserCommand>(dto);

        var response = await _mediator.Send(command);

        if (response.IsValid)
            return Ok(response);
        
        return BadRequest(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(string id)
    {
        var query = new GetApplicationUserQuery {Id = id};
        var response = await _mediator.Send(query);
        
        if (response.IsValid)
            return Ok(response);

        return NotFound(response);
    }
}