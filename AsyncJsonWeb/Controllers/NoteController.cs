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
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Note>> GetNoteById(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null || note.id == Guid.Empty)
                return NotFound($"Заметка с ID={id} не найдена.");
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult> AddNote([FromBody] Note newNote)
        {
            if (newNote == null)
                return BadRequest("Тело запроса пустое.");

            var result = await _noteService.AddNoteAsync(newNote.title, newNote.content, newNote.ownerId);
            if (!result)
                return BadRequest("Некорректные данные: title/content не пустые, ownerId не Guid.Empty.");

            return Ok("Заметка успешно добавлена.");
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateNote(Guid id, [FromBody] Note updatedNote)
        {
            if (updatedNote == null)
                return BadRequest("Тело запроса пустое.");

            var result = await _noteService.UpdateNoteAsync(id, updatedNote.title, updatedNote.content);
            if (!result)
                return NotFound($"Заметка с ID={id} не найдена или данные некорректны.");
            return Ok($"Заметка ID={id} обновлена.");
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteNote(Guid id)
        {
            var result = await _noteService.DeleteNoteAsync(id);
            if (!result)
                return NotFound($"Заметка с ID={id} не найдена.");
            return Ok($"Заметка ID={id} удалена.");
        }

        [HttpGet("owner/{ownerId:guid}")]
        public async Task<ActionResult<List<Note>>> GetNotesByOwnerId(Guid ownerId)
        {
            var notes = await _noteService.GetNotesByOwnerIdAsync(ownerId);
            return Ok(notes);
        }

        [HttpDelete("owner/{ownerId:guid}")]
        public async Task<ActionResult> DeleteNotesByOwnerId(Guid ownerId)
        {
            var result = await _noteService.DeleteNotesByOwnerIdAsync(ownerId);
            if (!result)
                return Ok($"[ЗАГЛУШКА] Удаление заметок пользователя {ownerId} пока не реализовано (ожидает БД).");
            return Ok($"Заметки пользователя {ownerId} удалены.");
        }
    }
}