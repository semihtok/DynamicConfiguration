using DynamicConfiguration.Data.Service;
using DynamicConfiguration.Data.Service.Abstract;

namespace DynamicConfiguration.Data.Factory
{
    public class ConfigurationServiceFactory
    {
        public ConfigurationServiceBase ProduceReader(string readerType, string connectionString)
        {
            // Can be multiple reader type for future
            switch (readerType)
            {
                case "Redis":
                    return new RedisConfigurationService(connectionString);
            }
            return null;
        }
    }
}