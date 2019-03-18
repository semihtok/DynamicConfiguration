namespace DynamicConfiguration.Core
{
    public interface IConfigurationReader
    {
        T GetValue<T>(string key);
    }
}