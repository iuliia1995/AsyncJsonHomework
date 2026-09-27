using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;
using AsyncJsonModule.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace AsyncJsonModule.Repositories.Json
{
    public class NoteJsonRepository : INoteJsonRepository
    {
        private static readonly string FilePath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Data", "notes.json"));

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public async Task<List<Note>> LoadNotesAsync()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
                    await File.WriteAllTextAsync(FilePath, "[]");
                    return new List<Note>();
                }

                string json;
                using (var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read,
                    FileShare.Read, bufferSize: 4096, useAsync: true))
                using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    json = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(json)) return new List<Note>();

                return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке заметок: {ex.Message}");
                return new List<Note>();
            }
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            var notes = await LoadNotesAsync();
            return notes.FirstOrDefault(n => n.id == id) ?? new Note();
        }

        public async Task AddNoteAsync(string title, string content, Guid ownerId)
        {
            try
            {
                var notes = await LoadNotesAsync();

                var newNote = new Note
                {
                    id = Guid.NewGuid(),
                    title = title,
                    content = content,
                    ownerId = ownerId,
                    createdAt = DateTime.UtcNow
                };

                notes.Add(newNote);
                await SaveNotesAsync(notes);
                Console.WriteLine($"[УСПЕХ] Заметка добавлена. ID: {newNote.id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] Не удалось добавить заметку: {ex.Message}");
            }
        }

        public async Task<bool> UpdateNoteByIdAsync(Guid id, string newTitle, string newContent)
        {
            try
            {
                var notes = await LoadNotesAsync();
                var note = notes.FirstOrDefault(n => n.id == id);
                if (note == null) return false;

                note.title = newTitle;
                note.content = newContent;

                await SaveNotesAsync(notes);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении заметки: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteNoteByIdAsync(Guid id)
        {
            try
            {
                var notes = await LoadNotesAsync();
                var note = notes.FirstOrDefault(n => n.id == id);
                if (note == null) return false;

                notes.Remove(note);
                await SaveNotesAsync(notes);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении заметки: {ex.Message}");
                return false;
            }
        }

        public async Task SaveNotesAsync(List<Note> notes)
        {
            try
            {
                string json = JsonSerializer.Serialize(notes, _jsonOptions);

                using (var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write,
                    FileShare.None, bufferSize: 4096, useAsync: true))
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении заметок: {ex.Message}");
            }
        }

        // ДЗ заглушки
        public Task<List<Note>> GetNotesByOwnerIdAsync(Guid ownerId)
        {
            Console.WriteLine($"[ЗАГЛУШКА] GetNotesByOwnerIdAsync({ownerId}) — ожидает БД.");
            return Task.FromResult(new List<Note>());
        }

        public Task<bool> DeleteNotesByOwnerIdAsync(Guid ownerId)
        {
            Console.WriteLine($"[ЗАГЛУШКА] DeleteNotesByOwnerIdAsync({ownerId}) — ожидает БД.");
            return Task.FromResult(false);
        }
    }
}