using hsemanagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsemanagement.Services
{
    public class EventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ottenere tutti gli eventi
        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        // Ottenere gli eventi in un intervallo di date
        public async Task<List<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Events
                .Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        // Ottenere un evento per ID
        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        // Creare un nuovo evento
        public async Task<Event> CreateEventAsync(Event @event)
        {
            @event.CreatedAt = DateTime.Now;
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return @event;
        }

        // Aggiornare un evento
        public async Task<Event> UpdateEventAsync(int id, Event updatedEvent)
        {
            var @event = await GetEventByIdAsync(id);
            if (@event == null)
                return null;

            @event.Title = updatedEvent.Title;
            @event.Description = updatedEvent.Description;
            @event.StartDate = updatedEvent.StartDate;
            @event.EndDate = updatedEvent.EndDate;
            @event.Color = updatedEvent.Color;
            @event.AllDay = updatedEvent.AllDay;
            @event.UpdatedAt = DateTime.Now;

            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            return @event;
        }

        // Eliminare un evento
        public async Task<bool> DeleteEventAsync(int id)
        {
            var @event = await GetEventByIdAsync(id);
            if (@event == null)
                return false;

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
