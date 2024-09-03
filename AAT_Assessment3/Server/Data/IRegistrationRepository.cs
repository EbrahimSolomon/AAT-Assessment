using AAT_Assessment3.Shared;

namespace AAT_Assessment3.Server.Data
{
    public interface IRegistrationRepository
    {
        Task<Registration> GetRegistrationByEventAndUserAsync(int eventId, string userEmail);
        Task RegisterForEventAsync(Registration registration);
    }
}
