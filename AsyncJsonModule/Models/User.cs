namespace AsyncJsonModule.Models
{
    public class User
    {
        public int id { get; set; }
        public string email { get; set; } = string.Empty;
        public string login { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}