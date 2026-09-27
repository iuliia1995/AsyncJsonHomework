using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncJsonModule.Models;
using AsyncJsonModule.Repositories;
using Dapper;
using Npgsql;

namespace AsyncJsonModule.Repositories.PostgreSQL
{
    public class EventPostgresRepository : IEventJsonRepository
    {
        private readonly string _connectionString;

        public EventPostgresRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Event>> LoadEventsAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id,
                                      name,
                                      description,
                                      date,
                                      participants,
                                      max_participants AS MaxParticipants
                               FROM ""Events""";

                var events = await connection.QueryAsync<Event>(sql);
                return events.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка загрузки событий: {ex.Message}");
                return new List<Event>();
            }
        }

        public async Task<Event> GetEventByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id,
                                      name,
                                      description,
                                      date,
                                      participants,
                                      max_participants AS MaxParticipants
                               FROM ""Events""
                               WHERE id = @Id";

                var ev = await connection.QueryFirstOrDefaultAsync<Event>(sql, new { Id = id });
                return ev ?? new Event();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка получения события {id}: {ex.Message}");
                return new Event();
            }
        }

        public async Task AddEventAsync(Event newEvent)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"INSERT INTO ""Events"" 
                                   (id, name, description, date, participants, max_participants)
                               VALUES 
                                   (@Id, @Name, @Description, @Date, @Participants, @MaxParticipants)";

                await connection.ExecuteAsync(sql, new
                {
                    Id = Guid.NewGuid(),
                    Name = newEvent.name,
                    Description = newEvent.description,
                    Date = newEvent.date,
                    Participants = newEvent.participants,
                    MaxParticipants = newEvent.MaxParticipants
                });

                Console.WriteLine($"[POSTGRES] Событие '{newEvent.name}' добавлено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка добавления события: {ex.Message}");
            }
        }

        public async Task<bool> UpdateEventByIdAsync(Guid id, Event updatedEvent)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"UPDATE ""Events""
                               SET name = @Name,
                                   description = @Description,
                                   date = @Date,
                                   participants = @Participants,
                                   max_participants = @MaxParticipants
                               WHERE id = @Id";

                int rows = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    Name = updatedEvent.name,
                    Description = updatedEvent.description,
                    Date = updatedEvent.date,
                    Participants = updatedEvent.participants,
                    MaxParticipants = updatedEvent.MaxParticipants
                });

                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка обновления события: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteEventByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"DELETE FROM ""Events"" WHERE id = @Id";

                int rows = await connection.ExecuteAsync(sql, new { Id = id });
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка удаления события: {ex.Message}");
                return false;
            }
        }

        public Task SaveEventsAsync(List<Event> events)
        {
            return Task.CompletedTask;
        }

        public async Task<List<Event>> GetFutureEventsAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id,
                                      name,
                                      description,
                                      date,
                                      participants,
                                      max_participants AS MaxParticipants
                               FROM ""Events""
                               WHERE date > @Now
                               ORDER BY date";

                var events = await connection.QueryAsync<Event>(sql, new { Now = DateTime.Now });
                return events.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка получения будущих событий: {ex.Message}");
                return new List<Event>();
            }
        }

        public async Task<List<Event>> SearchEventsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Event>();

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id,
                                      name,
                                      description,
                                      date,
                                      participants,
                                      max_participants AS MaxParticipants
                               FROM ""Events""
                               WHERE name ILIKE @Pattern";

                var events = await connection.QueryAsync<Event>(sql, new { Pattern = $"%{name}%" });
                return events.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка поиска событий по '{name}': {ex.Message}");
                return new List<Event>();
            }
        }
    }
}