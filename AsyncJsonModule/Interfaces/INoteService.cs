using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface INoteService
    {
        Task<List<Note>> GetAllNotesAsync();
        Task<Note> GetNoteByIdAsync(int id);
        Task<bool> AddNoteAsync(string title, string content, int ownerId);
        Task<bool> UpdateNoteAsync(int id, string title, string content);
        Task<bool> DeleteNoteAsync(int id);
        Task<List<Note>> GetNotesByOwnerIdAsync(int ownerId);
        Task<bool> DeleteNotesByOwnerIdAsync(int ownerId);
    }
}