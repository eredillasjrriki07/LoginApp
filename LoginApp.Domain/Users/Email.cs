using LoginApp.Domain.Common;

namespace LoginApp.Domain.Users;

public sealed class Email : ValueObject
{
    public const int MaxLength = 50;
    public string Value { get; }

    public Email(string? value)
    {
        var trimmed = value?.Trim() ?? string.Empty;
        Rule.Require(trimmed.Length > 0, "Email is required.");
        Rule.Require(trimmed.Length <= MaxLength, $"Email must be {MaxLength} or fewer.");
        Rule.Require(trimmed.Contains('@'), "Invalid email.");
        Value = trimmed;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
