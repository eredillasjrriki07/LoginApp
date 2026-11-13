using LoginApp.Domain.Common;

namespace LoginApp.Domain.Users;

public sealed class Username : ValueObject
{
    public const int MaxLength = 50;
    public string Value { get; }

    public Username(string value)
    {
        var trimmed = value?.Trim() ?? string.Empty;
        Rule.Require(trimmed.Length > 0, "Username is required");
        Rule.Require(trimmed.Length <= MaxLength, $"Username must be {MaxLength} or fewer.");
        Value = trimmed;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
