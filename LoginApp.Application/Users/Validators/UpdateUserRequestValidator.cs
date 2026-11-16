using FluentValidation;
using LoginApp.Application.Users.RequestDtos;

namespace LoginApp.Application.Users.Validators;

public sealed class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .Must(u => !string.IsNullOrWhiteSpace(u)).WithMessage(UserMessages.UsernameEmpty)
            .Must(UserRequestRule.UsernameLengthValid).WithMessage(UserMessages.UsernameTooLong);
        RuleFor(x => x.Email)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(UserMessages.EmailEmpty)
            .Must(UserRequestRule.EmailLengthValid).WithMessage(UserMessages.EmailTooLong)
            .Must(UserRequestRule.EmailFormatValid).WithMessage(UserMessages.EmailInvalid);
    }
}
