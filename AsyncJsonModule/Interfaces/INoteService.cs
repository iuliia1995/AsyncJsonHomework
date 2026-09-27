using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface INoteService
    {
        Task<List<Note>> GetAllNotesAsync();
        Task<Note> GetNoteByIdAsync(Guid id);
        Task<bool> AddNoteAsync(string title, string content, Guid ownerId);
        Task<bool> UpdateNoteAsync(Guid id, string title, string content);
        Task<bool> DeleteNoteAsync(Guid id);

        // ДЗ заглушки — реализуем после полного перехода на БД
        Task<List<Note>> GetNotesByOwnerIdAsync(Guid ownerId);
        Task<bool> DeleteNotesByOwnerIdAsync(Guid ownerId);
    }
}