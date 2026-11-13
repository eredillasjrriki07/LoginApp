using LoginApp.Application.Common;
using LoginApp.Domain.Common;
using System.Text.Json;

namespace LoginApp.Infrastructure.Persistence.Events;

internal sealed class DomainEventRecorder(LoginAppDbContext _db, IClock _clock) : IDomainEventRecorder
{
    public void Record(string eventType, string entityType, string entityId, object payload)
    {
        _db.DomainEvents.Add(new DomainEventRow
        {
            EventType = eventType,
            EntityType = entityType,
            EntityId = entityId,
            Payload = JsonSerializer.Serialize(payload, payload.GetType()),
            OccuredAt = _clock.UtcNow
        });
    }
}
