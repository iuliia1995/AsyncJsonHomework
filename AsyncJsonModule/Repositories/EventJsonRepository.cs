using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Repositories
{
    public class EventJsonRepository : IEventJsonRepository
    {
        private static readonly string FilePath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Data", "events.json"));

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public async Task<List<Event>> LoadEventsAsync()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                    await File.WriteAllTextAsync(FilePath, "[]");
                    return new List<Event>();
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

                return JsonSerializer.Deserialize<List<Event>>(json) ?? new List<Event>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке событий {ex.Message}");
                return new List<Event>();
            }
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            var events = await LoadEventsAsync();
            return events.FirstOrDefault(e => e.id == id) ?? new Event();
        }

        public async Task AddEventAsync(Event newEvent)
        {
            try
            {
                // Задание 4. проверка данных при создании события
                if (string.IsNullOrWhiteSpace(newEvent.name))
                {
                    Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Название не должно быть пустым.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(newEvent.description))
                {
                    Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Описание не должно быть пустым.");
                    return;
                }
                if (newEvent.date < DateTime.Now)
                {
                    Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Дата не должна быть в прошлом.");
                    return;
                }
                if (newEvent.MaxParticipants <= 0)
                {
                    Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Количество участников должно быть больше 0.");
                    return;
                }

                var events = await LoadEventsAsync();
                int nextId = events.Any() ? events.Max(e => e.id) + 1 : 1;
                newEvent.id = nextId;

                events.Add(newEvent);
                await SaveEventsAsync(events);
                Console.WriteLine($"[УСПЕХ] Событие добавлено ID {nextId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] Не удалось добавить событие {ex.Message}");
            }
        }

        public async Task<bool> UpdateEventByIdAsync(int id, Event updatedEvent)
        {
            try
            {
                var events = await LoadEventsAsync();
                var ev = events.FirstOrDefault(e => e.id == id);
                if (ev == null) return false;

                ev.name = updatedEvent.name;
                ev.description = updatedEvent.description;
                ev.date = updatedEvent.date;
                ev.participants = updatedEvent.participants;
                ev.MaxParticipants = updatedEvent.MaxParticipants;

                await SaveEventsAsync(events);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении события {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteEventByIdAsync(int id)
        {
            try
            {
                var events = await LoadEventsAsync();
                var ev = events.FirstOrDefault(e => e.id == id);
                if (ev == null) return false;

                events.Remove(ev);
                await SaveEventsAsync(events);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении события {ex.Message}");
                return false;
            }
        }

        public async Task SaveEventsAsync(List<Event> events)
        {
            try
            {
                string json = JsonSerializer.Serialize(events, _jsonOptions);

                using (var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write,
                    FileShare.None, bufferSize: 4096, useAsync: true))
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении событий {ex.Message}");
            }
        }

        // Задание 2. возвращаем будущие события
        public async Task<List<Event>> GetFutureEventsAsync()
        {
            var events = await LoadEventsAsync();
            return events.Where(e => e.date > DateTime.Now).ToList();
        }

        // Задание 3. поиск событий по названию
        public async Task<List<Event>> SearchEventsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return new List<Event>();

            var events = await LoadEventsAsync();
            return events.Where(e => e.name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}