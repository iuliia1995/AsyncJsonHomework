using System;

namespace AsyncJsonModule.Models
{
    public class Event
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public DateTime date { get; set; }
        public int participants { get; set; }
        public int MaxParticipants { get; set; }
    }
}