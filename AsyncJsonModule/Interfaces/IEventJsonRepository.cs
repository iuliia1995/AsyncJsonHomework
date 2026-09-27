using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Repositories
{
    public interface IEventJsonRepository
    {
        Task<List<Event>> LoadEventsAsync();
        Task<Event> GetEventByIdAsync(Guid id);
        Task AddEventAsync(Event newEvent);
        Task<bool> UpdateEventByIdAsync(Guid id, Event updatedEvent);
        Task<bool> DeleteEventByIdAsync(Guid id);
        Task SaveEventsAsync(List<Event> events);
        Task<List<Event>> GetFutureEventsAsync();
        Task<List<Event>> SearchEventsByNameAsync(string name);
    }
}