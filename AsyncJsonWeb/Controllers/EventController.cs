using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncJsonWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventJsonRepository _eventRepository;

        public EventController(IEventJsonRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetAllEvents()
        {
            var events = await _eventRepository.LoadEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var ev = await _eventRepository.GetEventByIdAsync(id);
            if (ev == null || ev.id == 0)
            {
                return NotFound($"Событие с ID={id} не найдено.");
            }
            return Ok(ev);
        }

        // Задание 4. валидация данных выполняется внутри репозитория (AddEventAsync).
        [HttpPost]
        public async Task<ActionResult> AddEvent([FromBody] Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Тело запроса пустое.");
            }

            // Задание 4. полная валидация — в репозитории,базовая проверка.
            if (string.IsNullOrWhiteSpace(newEvent.name) ||
                string.IsNullOrWhiteSpace(newEvent.description) ||
                newEvent.date < System.DateTime.Now ||
                newEvent.MaxParticipants <= 0)
            {
                return BadRequest("Некорректные данные события - имя/описание пустые, дата в прошлом или MaxParticipants <= 0.");
            }

            await _eventRepository.AddEventAsync(newEvent);
            return Ok("Событие успешно добавлено.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            if (updatedEvent == null)
            {
                return BadRequest("Тело запроса пустое.");
            }

            var result = await _eventRepository.UpdateEventByIdAsync(id, updatedEvent);
            if (!result)
            {
                return NotFound($"Событие с ID={id} не найдено.");
            }
            return Ok($"Событие ID={id} обновлено.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var result = await _eventRepository.DeleteEventByIdAsync(id);
            if (!result)
            {
                return NotFound($"Событие с ID={id} не найдено.");
            }
            return Ok($"Событие ID={id} удалено.");
        }

        // Задание 2. метод, возвращающий будущие событие
        [HttpGet("future")]
        public async Task<ActionResult<List<Event>>> GetFutureEvents()
        {
            var events = await _eventRepository.GetFutureEventsAsync();
            return Ok(events);
        }

        // Задание 3. метод поиска событие по названию
        [HttpGet("search/{name}")]
        public async Task<ActionResult<List<Event>>> SearchEventsByName(string name)
        {
            var events = await _eventRepository.SearchEventsByNameAsync(name);
            return Ok(events);
        }
    }
}