using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Services
{
    public class EventService : IEventService
    {
        private readonly IEventJsonRepository _eventRepository;

        public EventService(IEventJsonRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.LoadEventsAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _eventRepository.GetEventByIdAsync(id);
        }
        public async Task<bool> AddEventAsync(Event newEvent)
        {
            if (newEvent == null)
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Событие не может быть null.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(newEvent.name))
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Название события не должно быть пустым.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(newEvent.description))
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Описание события не должно быть пустым.");
                return false;
            }

            if (newEvent.date < DateTime.Now)
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Дата события не может быть в прошлом.");
                return false;
            }

            if (newEvent.MaxParticipants <= 0)
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] MaxParticipants должен быть больше 0.");
                return false;
            }

            await _eventRepository.AddEventAsync(newEvent);
            return true;
        }

        public async Task<bool> UpdateEventAsync(int id, Event updatedEvent)
        {
            if (updatedEvent == null)
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Событие не может быть null.");
                return false;
            }

            return await _eventRepository.UpdateEventByIdAsync(id, updatedEvent);
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            return await _eventRepository.DeleteEventByIdAsync(id);
        }

        public async Task<List<Event>> GetFutureEventsAsync()
        {
            return await _eventRepository.GetFutureEventsAsync();
        }

        public async Task<List<Event>> SearchEventsByNameAsync(string name)
        {
            return await _eventRepository.SearchEventsByNameAsync(name);
        }
    }
}