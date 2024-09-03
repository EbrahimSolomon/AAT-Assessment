using AAT_Assessment3.Shared;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace AAT_Assessment3.Server.Data
{
    public class EventRepository : IEventRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<EventRepository> _logger;

        public EventRepository(string connectionString, ILogger<EventRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Event> CreateEventAsync(Event newEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                if (newEvent.AvailableSeats > newEvent.TotalSeats)
                {
                    _logger.LogError("AvailableSeats {AvailableSeats} cannot be greater than TotalSeats {TotalSeats} for event {EventName}",
                                     newEvent.AvailableSeats, newEvent.TotalSeats, newEvent.Name);
                    throw new ArgumentException("AvailableSeats cannot be greater than TotalSeats.");
                }

                await using var connection = new SqliteConnection(_connectionString);
                var query = "INSERT INTO Events (Name, Date, TotalSeats, AvailableSeats) VALUES (@Name, @Date, @TotalSeats, @AvailableSeats); SELECT last_insert_rowid();";
                var eventId = await connection.ExecuteScalarAsync<int>(query, newEvent);

                newEvent.EventId = eventId;
                _logger.LogInformation("Created new event with ID {EventId}", eventId);
                return newEvent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new event");
                throw;
            }
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await using var connection = new SqliteConnection(_connectionString);
                var query = "SELECT * FROM Events";
                var events = await connection.QueryAsync<Event>(query);

                _logger.LogInformation("Retrieved {Count} events from database", events.AsList().Count);
                return events;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving events from the database");
                throw;
            }
        }

        public async Task<Event> GetEventByIdAsync(int eventId, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var connection = new SqliteConnection(_connectionString);
                var query = "SELECT * FROM Events WHERE EventId = @EventId";
                var eventItem = await connection.QuerySingleOrDefaultAsync<Event>(query, new { EventId = eventId });

                return eventItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the event by ID");
                throw;
            }
        }


        public async Task UpdateEventAsync(Event updatedEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var connection = new SqliteConnection(_connectionString);
                var query = "UPDATE Events SET Name = @Name, Date = @Date, TotalSeats = @TotalSeats, AvailableSeats = @AvailableSeats WHERE EventId = @EventId";
                await connection.ExecuteAsync(query, updatedEvent);

                _logger.LogInformation("Updated event with ID {EventId}", updatedEvent.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the event");
                throw;
            }
        }

        public async Task DeleteEventAsync(int eventId, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var connection = new SqliteConnection(_connectionString);
                var query = "DELETE FROM Events WHERE EventId = @EventId";
                await connection.ExecuteAsync(query, new { EventId = eventId });

                _logger.LogInformation("Deleted event with ID {EventId}", eventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the event");
                throw;
            }
        }
    }
 }
