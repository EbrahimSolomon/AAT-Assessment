using AAT_Assessment3.Shared;
using Dapper;
using Microsoft.Data.Sqlite;

namespace AAT_Assessment3.Server.Data
{
    public class RegistrationRepository:IRegistrationRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<RegistrationRepository> _logger;

        public RegistrationRepository(string connectionString, ILogger<RegistrationRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Registration> GetRegistrationByEventAndUserAsync(int eventId, string userEmail)
        {
            await using var connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM Registrations WHERE EventId = @EventId AND UserEmail = @UserEmail";
            return await connection.QuerySingleOrDefaultAsync<Registration>(query, new { EventId = eventId, UserEmail = userEmail });
        }

        public async Task RegisterForEventAsync(Registration registration)
        {
            await using var connection = new SqliteConnection(_connectionString);
            var query = "INSERT INTO Registrations (EventId, UserEmail, RegistrationDate, ReferenceNumber) VALUES (@EventId, @UserEmail, @RegistrationDate, @ReferenceNumber)";
            await connection.ExecuteAsync(query, registration);
        }

    }
}
