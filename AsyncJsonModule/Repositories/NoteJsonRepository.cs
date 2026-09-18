using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;
using System.Text.Encodings.Web;

namespace AsyncJsonModule.Repositories
{
    public class NoteJsonRepository : INoteJsonRepository
    {
        private static readonly string FilePath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Data", "notes.json"));

        public async Task<List<Note>> LoadNotesAsync()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    // Создаём пустой файл заметок, если его нет
                    Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
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

                if (string.IsNullOrWhiteSpace(json))
                {
                    json = "[]";
                }

                return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке заметок {ex.Message}");
                return new List<Note>();
            }
        }

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            var notes = await LoadNotesAsync();
            return notes.FirstOrDefault(n => n.id == id) ?? new Note();
        }

        public async Task AddNoteAsync(string title, string content, int ownerId)
        {
            try
            {
                var notes = await LoadNotesAsync();
                int nextId = notes.Any() ? notes.Max(n => n.id) + 1 : 1;

                var newNote = new Note
                {
                    id = nextId,
                    title = title,
                    content = content,
                    ownerId = ownerId,
                    createdAt = DateTime.UtcNow
                };

                notes.Add(newNote);
                await SaveNotesAsync(notes);
                Console.WriteLine($"[УСПЕХ] Заметка добавлена. ID {nextId}, OwnerId {ownerId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] Не удалось добавить заметку {ex.Message}");
            }
        }

        public async Task<bool> UpdateNoteByIdAsync(int id, string newTitle, string newContent)
        {
            try
            {
                var notes = await LoadNotesAsync();
                var note = notes.FirstOrDefault(n => n.id == id);
                if (note == null)
                {
                    Console.WriteLine($"Заметка с ID={id} не найдена.");
                    return false;
                }

                note.title = newTitle;
                note.content = newContent;

                await SaveNotesAsync(notes);
                Console.WriteLine($"[УСПЕХ] Заметка ID={id} обновлена.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении заметки {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteNoteByIdAsync(int id)
        {
            try
            {
                var notes = await LoadNotesAsync();
                var note = notes.FirstOrDefault(n => n.id == id);
                if (note == null)
                {
                    Console.WriteLine($"Заметка с ID={id} не найдена.");
                    return false;
                }

                notes.Remove(note);
                await SaveNotesAsync(notes);
                Console.WriteLine($"[УСПЕХ] Заметка ID={id} удалена.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении заметки {ex.Message}");
                return false;
            }
        }

        public async Task SaveNotesAsync(List<Note> notes)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Чтоб русские буквы писались читаемо
                };
                string json = JsonSerializer.Serialize(notes, options);

                using (var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write,
                    FileShare.None, bufferSize: 4096, useAsync: true))
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении заметок {ex.Message}");
            }
        }

        // Заглушки для методов, работающих с OwnerId.
        // Реальная реализация будет добавлена после подключения базы данных.

        public Task<List<Note>> GetNotesByOwnerIdAsync(int ownerId)
        {
            Console.WriteLine($"[ЗАГЛУШКА] GetNotesByOwnerIdAsync({ownerId}) — метод пока не реализован (ожидает БД).");
            return Task.FromResult(new List<Note>());
        }

        public Task<bool> DeleteNotesByOwnerIdAsync(int ownerId)
        {
            Console.WriteLine($"[ЗАГЛУШКА] DeleteNotesByOwnerIdAsync({ownerId}) — метод пока не реализован (ожидает БД).");
            return Task.FromResult(false);
        }
    }
}