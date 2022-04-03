using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Friday.Domain.Common;
using Friday.Services.Authorization.Commands.User.CreateApplicationUser;
using Friday.Services.Authorization.Controllers;
using Friday.Services.Authorization.DTOs;
using Friday.Services.Authorization.Entities;
using Friday.Services.Authorization.Identity;
using Friday.Services.Authorization.Profiles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute.ReceivedExtensions;
using Xunit;

namespace Friday.Tests.Services.Authorization;

public class UserControllerTests
{
    public UserControllerTests()
    {
        
    }

    [Fact]
    public async Task CreateUser_ShouldCreateUser()
    {
        var command = new CreateApplicationUserCommand()
        {
            UserName = "johndoe",
            Email = "johndoe@email.com",
            PhoneNumber = "123456789"
        };

        var mockedIdentity = MockIdentityService();
        
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationUserProfile()));
        var mapper = new Mapper(configuration);

        var validator = new CreateApplicationUserCommandValidator();
        var result = await validator.ValidateAsync(command);
        Assert.True(result.IsValid);
        
        var handler = new CreateApplicationUserCommandHandler(mockedIdentity.Object, mapper);
        var apiResponse = await handler.Handle(command, CancellationToken.None);
        Assert.True(apiResponse.IsValid);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnOk()
    {
        var createUserDto = new CreateUserDto
        {
            UserName = "johndoe",
            Email = "johndoe@email.com",
            PhoneNumber = "123456789",
            Password = "@Password123"
        };

        var result = await CallCreateUser(createUserDto);
        
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnBadRequest()
    {
        var createUserDto = new CreateUserDto
        {
            UserName = "",
            Email = "johndoe@email.com",
            PhoneNumber = "123456789",
            Password = "@Password123"
        };

        var result = await CallCreateUser(createUserDto);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }

    private async Task<ObjectResult> CallCreateUser(CreateUserDto createUserDto)
    {
        var identityService = MockIdentityService();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationUserProfile()));
        var mapper = new Mapper(configuration);

        var mediator =
            MockMediator<ApiResponse<string>,
                CreateApplicationUserCommand,
                CreateApplicationUserCommandHandler,
                CreateApplicationUserCommandValidator>(
                new CreateApplicationUserCommandHandler(identityService.Object, mapper),
                new ApiResponse<string>(null, new[] {"Error"}));

        var controller = new UserController(mediator.Object, mapper);
        var result = (await controller.CreateUser(createUserDto)).Result as ObjectResult;

        return result;
    }
    
    private Mock<IMediator> MockMediator<TResponse, TRequest, THandler, TValidator>(THandler handler, TResponse invalid) 
        where TRequest : IRequest<TResponse>, new()
        where THandler : IRequestHandler<TRequest, TResponse>
        where TValidator : AbstractValidator<TRequest>, new()
    {
        var mediator = new Mock<IMediator>();

        var pipeline = TResponse (TRequest request) =>
        {
            var validator = new TValidator();
            var result = validator.Validate(request);
            if (!result.IsValid)
                return invalid;
            
            var response = handler.Handle(request, CancellationToken.None);
            response.Wait();

            return response.Result;
        };
        
        mediator.Setup(x => x.Send(It.IsAny<TRequest>(), CancellationToken.None))
            .Returns((TRequest request, CancellationToken token) => Task.FromResult(pipeline(request)));

        return mediator;
    }
    
    private Mock<IIdentityService> MockIdentityService()
    {
        var mockedIdentity = new Mock<IIdentityService>();
        mockedIdentity.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(new IdentityResultMock(true));
        
        mockedIdentity.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new ApplicationUser { UserName = "johndoe", Email = "johndoe@email.com", PhoneNumber = "123456789"});

        return mockedIdentity;
    }
}

public class IdentityResultMock : IdentityResult
{
    public IdentityResultMock(bool succeeded) 
    {
        base.Succeeded = succeeded;
    }
}