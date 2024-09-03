using AAT_Assessment3.Server.Data;
using AAT_Assessment3.Shared;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AAT_Assessment3.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventRepository eventRepository, ILogger<EventsController> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var events = await _eventRepository.GetEventsAsync(cancellationToken);
                _logger.LogInformation("Retrieved {Count} events from database", events.Count());
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving events from the database");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event newEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                var createdEvent = await _eventRepository.CreateEventAsync(newEvent, cancellationToken);
                _logger.LogInformation("Event created with ID {EventId}", createdEvent.EventId);
                return CreatedAtAction(nameof(GetEventsAsync), new { id = createdEvent.EventId }, createdEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new event");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(int id, Event updatedEvent, CancellationToken cancellationToken = default)
        {
            if (id != updatedEvent.EventId)
            {
                return BadRequest("Event ID mismatch");
            }

            try
            {
                var existingEvent = await _eventRepository.GetEventByIdAsync(id, cancellationToken);
                if (existingEvent == null)
                {
                    return NotFound("Event not found");
                }

                await _eventRepository.UpdateEventAsync(updatedEvent, cancellationToken);
                _logger.LogInformation("Event with ID {EventId} updated", updatedEvent.EventId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the event");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEvent = await _eventRepository.GetEventByIdAsync(id, cancellationToken);
                if (existingEvent == null)
                {
                    return NotFound("Event not found");
                }

                await _eventRepository.DeleteEventAsync(id, cancellationToken);
                _logger.LogInformation("Event with ID {EventId} deleted", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the event");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
