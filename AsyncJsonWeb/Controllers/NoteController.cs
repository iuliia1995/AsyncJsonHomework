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
        private readonly INoteJsonRepository _noteRepository;

        public NoteController(INoteJsonRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> GetAllNotes()
        {
            var notes = await _noteRepository.LoadNotesAsync();
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNoteById(int id)
        {
            var note = await _noteRepository.GetNoteByIdAsync(id);
            if (note == null || note.id == 0)
            {
                return NotFound($"Заметка с ID={id} не найдена.");
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult> AddNote([FromBody] Note newNote)
        {
            if (newNote == null)
            {
                return BadRequest("Тело запроса пустое.");
            }

            await _noteRepository.AddNoteAsync(newNote.title, newNote.content, newNote.ownerId);
            return Ok("Заметка успешно добавлена.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNote(int id, [FromBody] Note updatedNote)
        {
            if (updatedNote == null)
            {
                return BadRequest("Тело запроса пустое.");
            }

            var result = await _noteRepository.UpdateNoteByIdAsync(id, updatedNote.title, updatedNote.content);
            if (!result)
            {
                return NotFound($"Заметка с ID={id} не найдена.");
            }
            return Ok($"Заметка ID={id} обновлена.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id)
        {
            var result = await _noteRepository.DeleteNoteByIdAsync(id);
            if (!result)
            {
                return NotFound($"Заметка с ID={id} не найдена.");
            }
            return Ok($"Заметка ID={id} удалена.");
        }

        // ==================== ДОМАШНЕЕ ЗАДАНИЕ ====================

        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<List<Note>>> GetNotesByOwnerId(int ownerId)
        {
            var notes = await _noteRepository.GetNotesByOwnerIdAsync(ownerId);
            return Ok(notes);
        }

        [HttpDelete("owner/{ownerId}")]
        public async Task<ActionResult> DeleteNotesByOwnerId(int ownerId)
        {
            var result = await _noteRepository.DeleteNotesByOwnerIdAsync(ownerId);
            if (!result)
            {
                return Ok($"[ЗАГЛУШКА] Удаление заметок пользователя {ownerId} пока не реализовано (ожидает БД).");
            }
            return Ok($"Заметки пользователя {ownerId} удалены.");
        }
    }
}