using FluentValidation;
using Friday.Services.Authorization.Common.Queries.User.GetApplicationUser;
using Friday.Services.Authorization.Entities;
using Microsoft.AspNetCore.Identity;

namespace Friday.Services.Authorization.Queries.User.GetApplicationUser;

public class GetApplicationUserQueryValidator : AbstractValidator<GetApplicationUserQuery>
{
    public GetApplicationUserQueryValidator(UserManager<ApplicationUser> userManager)
    {
        RuleFor(query => query.Id)
            .MustAsync(async (id, _) => await userManager.FindByIdAsync(id) != null)
            .WithErrorCode("Not found")
            .WithMessage("User with the given id does not exist");
    }
}