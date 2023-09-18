using FluentValidation;

namespace BusinessCard.Application.Application.Commands.AddMemberWithIdentityKey;

public class AddMemberWithIdentityKeyCommandValidator : AbstractValidator<AddMemberWithIdentityKeyCommand>
{
    public AddMemberWithIdentityKeyCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Occupation).NotEmpty();
        RuleFor(x => x.IdentityId).NotEmpty();

        RuleFor(x => x.SocialMedia).NotEmpty();
    }
}