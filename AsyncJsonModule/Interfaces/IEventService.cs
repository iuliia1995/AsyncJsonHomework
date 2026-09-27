using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(Guid id);
        Task<bool> AddEventAsync(Event newEvent);
        Task<bool> UpdateEventAsync(Guid id, Event updatedEvent);
        Task<bool> DeleteEventAsync(Guid id);

        // ДЗ
        Task<List<Event>> GetFutureEventsAsync();
        Task<List<Event>> SearchEventsByNameAsync(string name);
    }
}