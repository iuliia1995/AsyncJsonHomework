using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<bool> AddEventAsync(Event newEvent);
        Task<bool> UpdateEventAsync(int id, Event updatedEvent);
        Task<bool> DeleteEventAsync(int id);
        Task<List<Event>> GetFutureEventsAsync();
        Task<List<Event>> SearchEventsByNameAsync(string name);
    }
}