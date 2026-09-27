using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Repositories.Json;
using AsyncJsonModule.Repositories.PostgreSQL;
using AsyncJsonModule.Services;
using AsyncJsonModule.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string postgresConnectionString = builder.Configuration.GetConnectionString("PostgresConnection")
    ?? throw new InvalidOperationException("Строка подключения 'PostgresConnection' не найдена в appsettings.json");

builder.Services.AddSingleton<IUserJsonRepository>(sp =>
    new UserPostgresRepository(postgresConnectionString));
builder.Services.AddSingleton<INoteJsonRepository>(sp =>
    new NotePostgresRepository(postgresConnectionString));
builder.Services.AddSingleton<IEventJsonRepository>(sp =>
    new EventPostgresRepository(postgresConnectionString));

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<INoteService, NoteService>();
builder.Services.AddSingleton<IEventService, EventService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();