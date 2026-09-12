using System;

namespace AsyncJsonModule.Models
{
    public class Note
    {
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public int ownerId { get; set; }
        public DateTime createdAt { get; set; }
    }
}