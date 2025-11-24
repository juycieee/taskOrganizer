namespace TaskOrganizer.Models
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = "";
        public string DatabaseName { get; set; } = "";
        public string UsersCollection { get; set; } = "";
    }
}
