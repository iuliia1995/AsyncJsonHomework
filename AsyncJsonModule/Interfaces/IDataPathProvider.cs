namespace AsyncJsonModule.Interfaces
{
    public interface IDataPathProvider
    {
        string GetUsersPath();
        string GetNotesPath();
        string GetEventsPath();
    }
}