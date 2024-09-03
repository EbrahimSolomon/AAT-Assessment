using AAT_Assessment3.Server.Data;
using AAT_Assessment3.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AAT_Assessment3.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(IRegistrationRepository registrationRepository, IEventRepository eventRepository, ILogger<RegistrationController> logger)
        {
            _registrationRepository = registrationRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        [HttpPost("{eventId}/register")]
        public async Task<ActionResult> RegisterForEvent(int eventId, [FromBody] RegistrationRequest request)
        {
            try
            {
                var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
                if (eventItem == null)
                    return NotFound("Event not found");

                if (eventItem.AvailableSeats <= 0)
                    return BadRequest("No seats available for this event");

                var existingRegistration = await _registrationRepository.GetRegistrationByEventAndUserAsync(eventId, request.UserEmail);
                if (existingRegistration != null)
                    return BadRequest("User is already registered for this event");

                var referenceNumber = Guid.NewGuid().ToString().Substring(0, 8);

                var registration = new Registration
                {
                    EventId = eventId,
                    UserEmail = request.UserEmail,
                    RegistrationDate = DateTime.UtcNow,
                    ReferenceNumber = referenceNumber
                };

                await _registrationRepository.RegisterForEventAsync(registration);

                eventItem.AvailableSeats -= 1;
                await _eventRepository.UpdateEventAsync(eventItem);

                _logger.LogInformation("User {UserEmail} registered for event {EventId} with reference number {ReferenceNumber}", request.UserEmail, eventId, referenceNumber);

                return Ok(new { registration.ReferenceNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering for the event");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

