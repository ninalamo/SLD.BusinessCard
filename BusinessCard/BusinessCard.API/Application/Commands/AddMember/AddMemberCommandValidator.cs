using FluentValidation;

namespace BusinessCard.API.Application.Commands.AddMember;

public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    public AddMemberCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Occupation).NotEmpty();

        RuleFor(x => x.SocialMedia).Must(x => x.Any());
    }
}