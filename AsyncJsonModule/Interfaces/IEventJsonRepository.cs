using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface IEventJsonRepository
    {
        Task<List<Event>> LoadEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task AddEventAsync(Event newEvent);
        Task<bool> UpdateEventByIdAsync(int id, Event updatedEvent);
        Task<bool> DeleteEventByIdAsync(int id);
        Task SaveEventsAsync(List<Event> events);

        // Задание 2. получение будущих событий
        Task<List<Event>> GetFutureEventsAsync();

        // Задание 3. поиск событий по названию
        Task<List<Event>> SearchEventsByNameAsync(string name);
    }
}