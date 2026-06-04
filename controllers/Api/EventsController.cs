using SaasWeb_APP.Models;
using SaasWeb_APP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaasWeb_APP.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/events
        // Restituisce tutti gli eventi nel formato FullCalendar
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                // Il filtro per tenant è automatico via SESSION_CONTEXT
                var events = await _eventService.GetAllEventsAsync();
                var calendarEvents = new List<object>();

                foreach (var @event in events)
                {
                    calendarEvents.Add(new
                    {
                        id = @event.Id,
                        title = @event.Title,
                        start = @event.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        end = @event.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        backgroundColor = @event.Color,
                        borderColor = @event.Color,
                        allDay = @event.AllDay,
                        extendedProps = new
                        {
                            description = @event.Description
                        }
                    });
                }

                return Ok(calendarEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Errore nel recupero degli eventi", error = ex.Message });
            }
        }

        // GET: api/events/range?start=2026-06-01&end=2026-06-30
        // Restituisce gli eventi in un intervallo di date
        [HttpGet("range")]
        public async Task<IActionResult> GetEventsByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                var events = await _eventService.GetEventsByDateRangeAsync(start, end);
                var calendarEvents = new List<object>();

                foreach (var @event in events)
                {
                    calendarEvents.Add(new
                    {
                        id = @event.Id,
                        title = @event.Title,
                        start = @event.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        end = @event.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        backgroundColor = @event.Color,
                        borderColor = @event.Color,
                        allDay = @event.AllDay,
                        extendedProps = new
                        {
                            description = @event.Description
                        }
                    });
                }

                return Ok(calendarEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Errore nel recupero degli eventi", error = ex.Message });
            }
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            try
            {
                var @event = await _eventService.GetEventByIdAsync(id);
                if (@event == null)
                    return NotFound(new { message = "Evento non trovato" });

                return Ok(@event);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Errore nel recupero dell'evento", error = ex.Message });
            }
        }

        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event @event)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdEvent = await _eventService.CreateEventAsync(@event);
                return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Errore nella creazione dell'evento", error = ex.Message });
            }
        }

        // PUT: api/events/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event @event)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedEvent = await _eventService.UpdateEventAsync(id, @event);
                if (updatedEvent == null)
                    return NotFound(new { message = "Evento non trovato" });

                return Ok(updatedEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Errore nell'aggiornamento dell'evento", error = ex.Message });
            }
        }

        // DELETE: api/events/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var result = await _eventService.DeleteEventAsync(id);
                if (!result)
                    return NotFound(new { message = "Evento non trovato" });

                return Ok(new { message = "Evento eliminato con successo" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Errore nell'eliminazione dell'evento", error = ex.Message });
            }
        }
    }
}
