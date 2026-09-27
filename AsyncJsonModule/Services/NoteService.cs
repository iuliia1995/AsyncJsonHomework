using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;
using AsyncJsonModule.Repositories;

namespace AsyncJsonModule.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteJsonRepository _noteRepository;

        public NoteService(INoteJsonRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<List<Note>> GetAllNotesAsync()
        {
            return await _noteRepository.LoadNotesAsync();
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            return await _noteRepository.GetNoteByIdAsync(id);
        }

        public async Task<bool> AddNoteAsync(string title, string content, Guid ownerId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Название заметки не должно быть пустым.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Содержимое заметки не должно быть пустым.");
                return false;
            }

            if (ownerId == Guid.Empty)
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] OwnerId не может быть пустым.");
                return false;
            }

            await _noteRepository.AddNoteAsync(title, content, ownerId);
            return true;
        }

        public async Task<bool> UpdateNoteAsync(Guid id, string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Title и content не должны быть пустыми.");
                return false;
            }

            return await _noteRepository.UpdateNoteByIdAsync(id, title, content);
        }

        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            return await _noteRepository.DeleteNoteByIdAsync(id);
        }

        public async Task<List<Note>> GetNotesByOwnerIdAsync(Guid ownerId)
        {
            Console.WriteLine($"[ЗАГЛУШКА] GetNotesByOwnerIdAsync({ownerId}) — ожидает БД.");
            return await Task.FromResult(new List<Note>());
        }

        public async Task<bool> DeleteNotesByOwnerIdAsync(Guid ownerId)
        {
            Console.WriteLine($"[ЗАГЛУШКА] DeleteNotesByOwnerIdAsync({ownerId}) — ожидает БД.");
            return await Task.FromResult(false);
        }
    }
}