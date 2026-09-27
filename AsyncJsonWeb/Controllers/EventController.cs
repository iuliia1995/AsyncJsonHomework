using System;
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
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Event>> GetEventById(Guid id)
        {
            var ev = await _eventService.GetEventByIdAsync(id);
            if (ev == null || ev.id == Guid.Empty)
                return NotFound($"Событие с ID={id} не найдено.");
            return Ok(ev);
        }

        [HttpPost]
        public async Task<ActionResult> AddEvent([FromBody] Event newEvent)
        {
            if (newEvent == null)
                return BadRequest("Тело запроса пустое.");

            var result = await _eventService.AddEventAsync(newEvent);
            if (!result)
                return BadRequest("Некорректные данные события. Проверьте: name/description не пустые, дата не в прошлом, MaxParticipants > 0.");

            return Ok("Событие успешно добавлено.");
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] Event updatedEvent)
        {
            if (updatedEvent == null)
                return BadRequest("Тело запроса пустое.");

            var result = await _eventService.UpdateEventAsync(id, updatedEvent);
            if (!result)
                return NotFound($"Событие с ID={id} не найдено.");
            return Ok($"Событие ID={id} обновлено.");
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result)
                return NotFound($"Событие с ID={id} не найдено.");
            return Ok($"Событие ID={id} удалено.");
        }

        [HttpGet("future")]
        public async Task<ActionResult<List<Event>>> GetFutureEvents()
        {
            var events = await _eventService.GetFutureEventsAsync();
            return Ok(events);
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<List<Event>>> SearchEventsByName(string name)
        {
            var events = await _eventService.SearchEventsByNameAsync(name);
            return Ok(events);
        }
    }
}