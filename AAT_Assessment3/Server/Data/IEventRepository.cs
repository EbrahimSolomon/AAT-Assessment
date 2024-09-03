using AAT_Assessment3.Shared;

namespace AAT_Assessment3.Server.Data
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetEventsAsync(CancellationToken cancellationToken = default);
        Task<Event> CreateEventAsync(Event newEvent, CancellationToken cancellationToken = default);
        Task<Event> GetEventByIdAsync(int eventId, CancellationToken cancellationToken = default);
        Task UpdateEventAsync(Event updatedEvent, CancellationToken cancellationToken = default);
        Task DeleteEventAsync(int eventId, CancellationToken cancellationToken = default);
    }


}
