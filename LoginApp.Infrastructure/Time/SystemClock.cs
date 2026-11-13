using LoginApp.Domain.Common;

namespace LoginApp.Infrastructure.Time;

internal sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
