using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Repositories
{
    public interface INoteJsonRepository
    {
        Task<List<Note>> LoadNotesAsync();
        Task<Note> GetNoteByIdAsync(Guid id);
        Task AddNoteAsync(string title, string content, Guid ownerId);
        Task<bool> UpdateNoteByIdAsync(Guid id, string newTitle, string newContent);
        Task<bool> DeleteNoteByIdAsync(Guid id);
        Task SaveNotesAsync(List<Note> notes);
        Task<List<Note>> GetNotesByOwnerIdAsync(Guid ownerId);
        Task<bool> DeleteNotesByOwnerIdAsync(Guid ownerId);
    }
}