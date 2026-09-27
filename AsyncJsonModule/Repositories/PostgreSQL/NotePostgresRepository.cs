using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;
using AsyncJsonModule.Repositories;
using Dapper;
using Npgsql;

namespace AsyncJsonModule.Repositories.PostgreSQL
{
    public class NotePostgresRepository : INoteJsonRepository
    {
        private readonly string _connectionString;

        public NotePostgresRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Note>> LoadNotesAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id,
                                      title,
                                      content,
                                      owner_id AS ownerId,
                                      created_at AS createdAt
                               FROM ""Notes""";

                var notes = await connection.QueryAsync<Note>(sql);
                return notes.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка загрузки заметок: {ex.Message}");
                return new List<Note>();
            }
        }

        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id,
                                      title,
                                      content,
                                      owner_id AS ownerId,
                                      created_at AS createdAt
                               FROM ""Notes""
                               WHERE id = @Id";

                var note = await connection.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id });
                return note ?? new Note();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка получения заметки {id}: {ex.Message}");
                return new Note();
            }
        }

        public async Task AddNoteAsync(string title, string content, Guid ownerId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                DateTime now = DateTime.UtcNow;

                string sql = @"INSERT INTO ""Notes"" 
                                   (id, title, content, owner_id, folder_id, created_at, updated_at, is_favorite, is_archive, note_type)
                               VALUES 
                                   (@Id, @Title, @Content, @OwnerId, NULL, @CreatedAt, @UpdatedAt, FALSE, FALSE, NULL)";

                await connection.ExecuteAsync(sql, new
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Content = content,
                    OwnerId = ownerId,
                    CreatedAt = now,
                    UpdatedAt = now
                });

                Console.WriteLine($"[POSTGRES] Заметка '{title}' добавлена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка добавления заметки: {ex.Message}");
            }
        }

        public async Task<bool> UpdateNoteByIdAsync(Guid id, string newTitle, string newContent)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"UPDATE ""Notes""
                               SET title = @Title,
                                   content = @Content,
                                   updated_at = @UpdatedAt
                               WHERE id = @Id";

                int rows = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    Title = newTitle,
                    Content = newContent,
                    UpdatedAt = DateTime.UtcNow
                });

                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка обновления заметки: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteNoteByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"DELETE FROM ""Notes"" WHERE id = @Id";

                int rows = await connection.ExecuteAsync(sql, new { Id = id });
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка удаления заметки: {ex.Message}");
                return false;
            }
        }

        public Task SaveNotesAsync(List<Note> notes)
        {
            return Task.CompletedTask;
        }

        public async Task<List<Note>> GetNotesByOwnerIdAsync(Guid ownerId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id,
                                      title,
                                      content,
                                      owner_id AS ownerId,
                                      created_at AS createdAt
                               FROM ""Notes""
                               WHERE owner_id = @OwnerId";

                var notes = await connection.QueryAsync<Note>(sql, new { OwnerId = ownerId });
                return notes.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка получения заметок владельца {ownerId}: {ex.Message}");
                return new List<Note>();
            }
        }

        public async Task<bool> DeleteNotesByOwnerIdAsync(Guid ownerId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"DELETE FROM ""Notes"" WHERE owner_id = @OwnerId";

                int rows = await connection.ExecuteAsync(sql, new { OwnerId = ownerId });
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка удаления заметок владельца {ownerId}: {ex.Message}");
                return false;
            }
        }
    }
}