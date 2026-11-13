using FluentValidation;
using LoginApp.Application.Users.RequestDtos;
using LoginApp.Domain.Users;

namespace LoginApp.Application.Users.Validators;

public sealed class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestValidator()
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

internal static class UserRequestRule
{
    public static bool UsernameLengthValid(string? username) => (username?.Trim().Length ?? 0) <= Username.MaxLength;
    public static bool EmailLengthValid(string? email) => (email?.Trim().Length ?? 0) <= Email.MaxLength;
    public static bool EmailFormatValid(string email) => email.Contains('@');
}