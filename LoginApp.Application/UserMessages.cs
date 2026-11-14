using LoginApp.Domain.Users;

namespace LoginApp.Application;

public static class UserMessages
{
    public const string NotFound = "User not found.";
    public const string UsernameEmpty = "Username is required.";
    public static readonly string UsernameTooLong = $"Username must be {Username.MaxLength} or fewer.";
    public const string EmailEmpty = "Email is required.";
    public static readonly string EmailTooLong = $"Email must be {Email.MaxLength} or fewer.";
    public const string EmailInvalid = "Email is invalid.";
    public static readonly string UserInactive = UserRules.Inactive;
    public const string PasswordEmpty = "Password is required.";
}
