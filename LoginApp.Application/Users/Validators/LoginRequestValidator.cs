using FluentValidation;
using LoginApp.Application.Users.RequestDtos;

namespace LoginApp.Application.Users.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .Must(u => !string.IsNullOrWhiteSpace(u)).WithMessage(UserMessages.EmailEmpty)
            .Must(UserRequestRule.EmailFormatValid).WithMessage(UserMessages.EmailInvalid);
        RuleFor(x => x.Password)
            .Must(p => !string.IsNullOrWhiteSpace(p)).WithMessage(UserMessages.PasswordEmpty);
    }
}
