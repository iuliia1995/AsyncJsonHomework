using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Repositories;
using AsyncJsonModule.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<INoteJsonRepository, NoteJsonRepository>();
builder.Services.AddSingleton<IUserJsonRepository, UserJsonRepository>();
builder.Services.AddSingleton<IEventJsonRepository, EventJsonRepository>();

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

app.UseAuthorization();
app.MapControllers();

app.Run();