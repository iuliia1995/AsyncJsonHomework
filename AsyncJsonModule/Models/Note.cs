using System;

namespace AsyncJsonModule.Models
{
    public class Note
    {
        public Guid id { get; set; }
        public string title { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public Guid ownerId { get; set; }   // было int, стало Guid
        public DateTime createdAt { get; set; }
    }
}