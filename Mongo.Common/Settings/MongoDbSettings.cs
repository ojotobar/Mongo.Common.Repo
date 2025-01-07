namespace Mongo.Common.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; init; } = string.Empty;
        public string DatabaseName { get; init; } = string.Empty;

        public static MongoDbSettings Initialize(string connectionString, string databaseName)
        {
            return new MongoDbSettings
            {
                ConnectionString = connectionString,
                DatabaseName = databaseName
            };
        }
    }
}
