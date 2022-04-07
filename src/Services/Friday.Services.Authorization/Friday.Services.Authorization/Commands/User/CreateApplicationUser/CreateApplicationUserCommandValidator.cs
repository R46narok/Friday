using FluentValidation;

namespace Friday.Services.Authorization.Commands.User.CreateApplicationUser;

public class CreateApplicationUserCommandValidator : AbstractValidator<CreateApplicationUserCommand>
{
    public CreateApplicationUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();
    }
}