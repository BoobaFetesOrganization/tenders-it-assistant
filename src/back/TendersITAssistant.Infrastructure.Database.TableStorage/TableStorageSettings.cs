namespace TendersITAssistant.Infrastructure.Database.TableStorage
{
    public struct TableStorageSettings
    {
        public string ConnectionString { get; }

        public TableStorageSettings(string? connectionString)
        {
            if(connectionString is null) throw new ArgumentNullException(nameof(connectionString));
            ConnectionString = connectionString;
        }
    }
}