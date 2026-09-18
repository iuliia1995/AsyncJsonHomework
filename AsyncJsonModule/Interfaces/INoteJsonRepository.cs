using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface INoteJsonRepository
    {
        Task<List<Note>> LoadNotesAsync();
        Task<Note> GetNoteByIdAsync(int id);
        Task AddNoteAsync(string title, string content, int ownerId);
        Task<bool> UpdateNoteByIdAsync(int id, string newTitle, string newContent);
        Task<bool> DeleteNoteByIdAsync(int id);
        Task SaveNotesAsync(List<Note> notes);

        // лаб
        Task<List<Note>> GetNotesByOwnerIdAsync(int ownerId);
        Task<bool> DeleteNotesByOwnerIdAsync(int ownerId);
    }
}