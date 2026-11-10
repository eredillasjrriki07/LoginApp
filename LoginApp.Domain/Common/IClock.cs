namespace LoginApp.Domain.Common;

public interface IClock
{
    DateTime UtcNow { get; }
}
