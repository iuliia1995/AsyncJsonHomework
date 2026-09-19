using System;
using System.IO;
using AsyncJsonModule.Interfaces;

namespace AsyncJsonModule.DataPathProvider
{
    public class DataPathProvider : IDataPathProvider
    {
        private readonly string _basePath;

        public DataPathProvider()
        {
            _basePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Data"));
        }

        public string GetUsersPath() => Path.Combine(_basePath, "users.json");
        public string GetNotesPath() => Path.Combine(_basePath, "notes.json");
        public string GetEventsPath() => Path.Combine(_basePath, "events.json");
    }
}